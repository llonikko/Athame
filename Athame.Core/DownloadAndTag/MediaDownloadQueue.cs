using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Athame.Core.Logging;
using Athame.Core.Utils;
using Athame.PluginAPI.Downloader;
using Athame.PluginAPI.Service;

namespace Athame.Core.DownloadAndTag
{
    public class MediaDownloadQueue : List<EnqueuedCollection>
    {
        private const string Tag = nameof(MediaDownloadQueue);

        public CancellationTokenSource CancellationTokenSource { get; set; }

        private readonly bool useTempFile;

        public MediaDownloadQueue(bool useTempFile)
        {
            this.useTempFile = useTempFile;
        }

        public EnqueuedCollection Enqueue(MusicService service, IMediaCollection collection, string destination, string pathFormat)
        {
            var item = new EnqueuedCollection
            {
                Destination = destination,
                Service = service,
                PathFormat = pathFormat,
                MediaCollection = collection,
                Type = MediaCollectionAsType(collection)
            };
            Add(item);
            return item;
        }

        #region Events
        /// <summary>
        /// Raised before a media collection is downloaded.
        /// </summary>
        public event EventHandler<CollectionDownloadEventArgs> CollectionDequeued;

        protected void OnCollectionDequeued(CollectionDownloadEventArgs e)
        {
            CollectionDequeued?.Invoke(this, e);
        }

        /// <summary>
        /// Raised before a track is downloaded.
        /// </summary>
        public event EventHandler<TrackDownloadEventArgs> TrackDequeued;

        protected void OnTrackDequeued(TrackDownloadEventArgs e)
        {
            TrackDequeued?.Invoke(this, e);
        }

        /// <summary>
        /// Raised after a track is downloaded.
        /// </summary>
        public event EventHandler<TrackDownloadEventArgs> TrackDownloadCompleted;

        protected void OnTrackDownloadCompleted(TrackDownloadEventArgs e)
        {

            TrackDownloadCompleted?.Invoke(this, e);
        }

        /// <summary>
        /// Raised when a track's download progress changes.
        /// </summary>
        public event EventHandler<TrackDownloadEventArgs> TrackDownloadProgress;

        protected void OnTrackDownloadProgress(TrackDownloadEventArgs e)
        {
            TrackDownloadProgress?.Invoke(this, e);
        }

        public event EventHandler<ExceptionEventArgs> Exception;

        protected void OnException(ExceptionEventArgs e)
        {
            Exception?.Invoke(this, e);

        }
        #endregion

        public int TrackCount
        {
            get
            {
                return this.Sum(collection => collection.MediaCollection.Tracks.Count);
            }
        }

        private ExceptionSkip skip;

        public async Task StartDownloadAsync(SavePlaylistSetting playlistSetting, bool ignoreSaveArtworkWithPlaylist,
            AlbumArtworkSaveFormat albumArtworkSaveFormat)
        {
            var queueView = new Queue<EnqueuedCollection>(this);
            while (queueView.Count > 0)
            {
                var currentItem = queueView.Dequeue();
                OnCollectionDequeued(new CollectionDownloadEventArgs
                {
                    Collection = currentItem,
                    CurrentCollectionIndex = (Count - queueView.Count) - 1,
                    TotalNumberOfCollections = Count
                });
                if (!(currentItem.MediaCollection is Playlist))
                {
                    playlistSetting = SavePlaylistSetting.DontSave;
                }
                if (await DownloadCollectionAsync(currentItem, playlistSetting, ignoreSaveArtworkWithPlaylist, albumArtworkSaveFormat)) continue;
                if (skip == ExceptionSkip.Fail)
                {
                    return;
                }
            }
        }

        public EnqueuedCollection ItemById(string id)
        {
            return (from item in this
                    where item.MediaCollection.Id == id
                    select item).FirstOrDefault();

        }

        private void EnsureParentDirectories(string path)
        {
            var parentPath = Path.GetDirectoryName(path);
            if (parentPath == null) return;
            Directory.CreateDirectory(parentPath);
        }

        private async Task<bool> DownloadCollectionAsync(EnqueuedCollection collection, SavePlaylistSetting savePlaylistSetting, bool ignoreSaveArtworkWithPlaylist,
            AlbumArtworkSaveFormat albumArtworkSaveFormat)
        {
            var tracksCollectionLength = collection.MediaCollection.Tracks.Count;
            var tracksQueue = new Queue<Track>(collection.MediaCollection.Tracks);
            var trackFiles = new List<TrackFile>(collection.MediaCollection.Tracks.Count);
            TrackDownloadEventArgs gEventArgs = null;
            while (tracksQueue.Count > 0)
            {
                var eventArgs = gEventArgs = new TrackDownloadEventArgs
                {
                    CurrentItemIndex = (tracksCollectionLength - tracksQueue.Count),
                    PercentCompleted = 0M,
                    State = DownloadState.PreProcess,
                    TotalItems = tracksCollectionLength,
                    
                    TrackFile = null
                };
                var currentItem = tracksQueue.Dequeue();

                OnTrackDequeued(eventArgs);

                try
                {
                    if (!currentItem.IsDownloadable)
                    {
                        continue;
                    }
                    OnTrackDownloadProgress(eventArgs);
                    if (currentItem.Album?.CoverPicture != null)
                    {
                        // Download album artwork if it's not cached
                        var albumSmid = currentItem.Album.GetSmid(collection.Service.Info.Name).ToString();
                        if (!ImageCache.Instance.HasItem(albumSmid))
                        {
                            eventArgs.State = DownloadState.DownloadingAlbumArtwork;
                            OnTrackDownloadProgress(eventArgs);
                            try
                            {
                                await ImageCache.Instance.AddByDownload(albumSmid, currentItem.Album.CoverPicture);
                            }
                            catch (Exception ex)
                            {
                                ImageCache.Instance.AddNull(albumSmid);
                                Log.WriteException(Level.Warning, Tag, ex, "Exception occurred when download album artwork:");
                            }
                        }
                    }
                    // Get the TrackFile
                    eventArgs.TrackFile = await collection.Service.GetDownloadableTrackAsync(currentItem);
                    var downloader = collection.Service.GetDownloader(eventArgs.TrackFile);
                    downloader.Progress += (sender, args) =>
                    {
                        eventArgs.Update(args);
                        OnTrackDownloadProgress(eventArgs);
                    };
                    downloader.Done += (sender, args) =>
                    {
                        eventArgs.State = DownloadState.PostProcess;
                        OnTrackDownloadProgress(eventArgs);
                    };

                    // Generate the path
                    var path = collection.GetPath(eventArgs.TrackFile);
                    var tempPath = path;
                    if (useTempFile) tempPath += "-temp";
                    EnsureParentDirectories(tempPath);
                    eventArgs.State = DownloadState.Downloading;

                    // Begin download
                    await downloader.DownloadAsyncTask(eventArgs.TrackFile, tempPath);
                    trackFiles.Add(eventArgs.TrackFile);

                    // Attempt to dispose the downloader, since the most probable case will be that it will
                    // implement IDisposable if it uses sockets
                    var disposableDownloader = downloader as IDisposable;
                    disposableDownloader?.Dispose();

                    // Write the tag
                    eventArgs.State = DownloadState.WritingTags;
                    OnTrackDownloadProgress(eventArgs);
                    var setting = albumArtworkSaveFormat;
                    if (ignoreSaveArtworkWithPlaylist && collection.Type == MediaType.Playlist)
                    {
                        setting = AlbumArtworkSaveFormat.DontSave;
                    }
                    TrackTagger.Write(collection.Service.Info.Name, currentItem, eventArgs.TrackFile, setting, tempPath);

                    // Rename to proper path
                    if (useTempFile)
                    {
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                        File.Move(tempPath, path);
                    }


                }
                catch (Exception ex)
                {
                    var exEventArgs = new ExceptionEventArgs { CurrentState = eventArgs, Exception = ex };
                    OnException(exEventArgs);
                    switch (exEventArgs.SkipTo)
                    {
                        case ExceptionSkip.Item:
                            continue;

                        case ExceptionSkip.Collection:
                        case ExceptionSkip.Fail:
                            skip = exEventArgs.SkipTo;
                            return false;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                // Raise the completed event even if an error occurred
                OnTrackDownloadCompleted(eventArgs);

                
            }

            // Write playlist if possible
            try
            {
                var writer = new PlaylistWriter(collection, trackFiles);
                switch (savePlaylistSetting)
                {
                    case SavePlaylistSetting.DontSave:
                        break;
                    case SavePlaylistSetting.M3U:
                        writer.WriteM3U8();
                        break;
                    case SavePlaylistSetting.PLS:
                        writer.WritePLS();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(savePlaylistSetting), savePlaylistSetting, null);
                }
            }
            catch (Exception ex)
            {
                var exEventArgs = new ExceptionEventArgs
                {
                    CurrentState = gEventArgs,
                    Exception = ex
                };
                OnException(exEventArgs);
                switch (exEventArgs.SkipTo)
                {
                    case ExceptionSkip.Item:
                        break;

                    case ExceptionSkip.Collection:
                    case ExceptionSkip.Fail:
                        skip = exEventArgs.SkipTo;
                        return false;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return true;
        }

        private static MediaType MediaCollectionAsType(IMediaCollection collection)
        {
            if (collection is Album)
            {
                return MediaType.Album;
            }
            if (collection is Playlist)
            {
                return MediaType.Playlist;
            }
            if (collection is SingleTrackCollection)
            {
                return MediaType.Track;
            }
            return MediaType.Unknown;
        }
    }
}
