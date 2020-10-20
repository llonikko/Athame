using Athame.Avalonia.Models;
using Athame.Avalonia.Resources;
using Athame.Core.Download;
using Athame.Plugin.Api.Service;
using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace Athame.Avalonia.ViewModels
{
    public class MediaItemsViewModel : ViewModelBase
    {
        private readonly MediaDownloadSource source;
        private readonly ReadOnlyObservableCollection<MediaItem> mediaItems;

        public IObservable<bool> CanDownload { get; }

        public ReactiveCommand<MediaDownloadService, Unit> AddMediaCommand { get; }

        public IEnumerable<MediaDownloadService> Items
            => source.Items;

        public ReadOnlyObservableCollection<MediaItem> MediaItems 
            => mediaItems;

        public MediaItemsViewModel()
        {
            source = new MediaDownloadSource();
            
            source
                .Connect()
                .Transform(ms => new MediaItem(ms.Media))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out mediaItems)
                .DisposeMany()
                .Subscribe();

            CanDownload = this
                .WhenAnyValue(x => x.MediaItems.Count)
                .Select(count => count > 0 && mediaItems.Any(x => x.IsDownloadable()));

            AddMediaCommand = ReactiveCommand.Create<MediaDownloadService>(AddMedia);
        }

        public void UpdateTrackItem(Track track)
        {
            var item = mediaItems.Select(m => m.GetTrackItem(track)).First();
            item.ImageStatus = Images.Success;
        }

        private void AddMedia(MediaDownloadService media)
            => source.Add(media);
    }
}
