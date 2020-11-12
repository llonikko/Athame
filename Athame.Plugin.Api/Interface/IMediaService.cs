using Athame.Plugin.Api.Downloader;
using Athame.Plugin.Api.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Athame.Plugin.Api.Interface
{
    public interface IMediaService
    {
        /// <summary>
        /// Retrieves a track's downloadable form.
        /// </summary>
        /// <param name="track">The track to download.</param>
        /// <returns>A <see cref="TrackFile"/> containing file metadata and the URI of the track.</returns>
        Task<TrackFile> GetDownloadableTrackAsync(Track track);

        /// <summary>
        /// Retrieves a playlist. Note that it is up to the implementation to differentiate
        /// between different playlist types, if the music service specifies them.
        /// </summary>
        /// <param name="id">The playlist ID to retrieve.</param>
        /// <returns>A playlist on success, null otherwise.</returns>
        Task<Playlist> GetPlaylistAsync(string id);

        /// <summary>
        /// Retrieves the items in a playlist
        /// </summary>
        /// <param name="id">The playlist's ID to retrieve.</param>
        /// <param name="itemsPerPage">The number of items to retrieve per page.</param>
        /// <returns>A paged list of tracks.</returns>
        Task<IEnumerable<Track>> GetPlaylistItemsAsync(string id, int itemsPerPage);

        /// <summary>
        /// Parses a public-facing URL of a service, and returns the media type referenced and the identifier.
        /// </summary>
        /// <param name="uri">A URL to parse.</param>
        /// <returns>A <see cref="MediaDescriptor"/> containing a media type and ID.</returns>
        MediaDescriptor ParseUrl(Uri uri);

        /// <summary>
        /// Performs a text search and retrieves the results -- see <see cref="SearchResult"/> for what is returned.
        /// </summary>
        /// <param name="searchText">The text to search</param>
        /// <param name="mediaType">Which media to search for. This can be ignored for services which return all types regardless.</param>
        /// <param name="itemsPerPage">The number of items to retrieve per page.</param>
        /// <returns>A <see cref="SearchResult"/> containing top tracks, albums, or playlists.</returns>
        SearchResult Search(string searchText, MediaType mediaType, int itemsPerPage);

        /// <summary>
        /// Retrieves information for a specified artist.
        /// </summary>
        /// <param name="id">The artist's ID.</param>
        /// <returns>An artist.</returns>
        Task<Artist> GetArtistInfoAsync(string id);

        /// <summary>
        /// Retrieves the top tracks for an artist.
        /// </summary>
        /// <param name="id">The artist's ID.</param>
        /// <param name="itemsPerPage">How many items per page to retrieve.</param>
        /// <returns>A paged list of tracks.</returns>
        PagedList<Track> GetArtistTopTracks(string id, int itemsPerPage);

        /// <summary>
        /// Retrieves the albums released by an artist.
        /// </summary>
        /// <param name="id">The artist's ID</param>
        /// <param name="itemsPerPage">How many items per page to retrieve.</param>
        /// <returns>A paged list of albums.</returns>
        PagedList<Album> GetArtistAlbums(string id, int itemsPerPage);

        /// <summary>
        /// Retrieves an album.
        /// </summary>
        /// <param name="id">The album's identifier.</param>
        /// <param name="withTracks">Whether to return tracks or not. On some services, this may involve an extra API call. 
        /// Implementations are also allowed to return an object with tracks even if this is false.</param>
        /// <returns>An album, with or without tracks.</returns>
        Task<Album> GetAlbumAsync(string id, bool withTracks = false);

        /// <summary>
        /// Retrieves the metadata for a single track.
        /// </summary>
        /// <param name="id">The track's identifier.</param>
        /// <returns>A track.</returns>
        Task<Track> GetTrackAsync(string id);

        /// <summary>
        /// The base URI of the service. Entered URIs are compared on the Scheme and Host properties of each base URI, and if they match,
        /// <see cref="ParseUrl"/> is called.
        /// </summary>
        Uri[] BaseUri { get; }

        /// <summary>
        /// Returns a downloader for this service. The default implementation is <see cref="TrackDownloader"/>,
        /// but this method may be overridden with a custom downloader that implements <see cref="IDownloader"/>.
        /// </summary>
        /// <returns>A new concrete implementation of <see cref="IDownloader"/>.</returns>
        IDownloader GetDownloader();

        /// <summary>
        /// Retrieves the largest version of the picture available.
        /// </summary>
        /// <returns>A byte array of the picture's data.</returns>
        Task<MediaCover> GetMediaCoverAsync(string id);

        /// <summary>
        /// The name of the service. Required.
        /// </summary>
        string Name { get; }
    }
}
