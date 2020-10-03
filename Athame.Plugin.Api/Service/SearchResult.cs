using System.Threading.Tasks;

namespace Athame.Plugin.Api.Service
{
    /// <summary>
    /// Represents a search result. A search result is similar to a <see cref="PagedMethod{T}"/>, except it has
    /// multiple, pageable lists of information.
    /// </summary>
    public abstract class SearchResult
    {
        /// <summary>
        /// A list of top tracks, sorted by relevance.
        /// </summary>
        public PagedList<Track> TopTracks { get; set; }

        /// <summary>
        /// A list of albums.
        /// </summary>
        public PagedList<Album> Albums { get; set; }

        /// <summary>
        /// A list of artists.
        /// </summary>
        public PagedList<Artist> Artists { get; set; }

        /// <summary>
        /// A list of playlists.
        /// </summary>
        public PagedList<Playlist> Playlists { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="itemsPerPage">The amount of items per page to load.</param>
        protected SearchResult(int itemsPerPage)
        {
            ItemsPerPage = itemsPerPage;
        }

        /// <summary>
        /// The amount of items that are retrieved per page.
        /// </summary>
        public int ItemsPerPage { get; set; }

        /// <summary>
        /// Retrieves the next page of results asynchronously and appends the results to each media property.
        /// </summary>
        public abstract Task GetNextPageAsync();

        /// <summary>
        /// Retrieves each page sequentially until there are none left.
        /// </summary>
        public virtual async Task LoadAllPagesAsync()
        {
            while (
                (TopTracks != null && TopTracks.HasMoreItems) ||
                (Albums != null && Albums.HasMoreItems) ||
                (Artists != null && Artists.HasMoreItems) ||
                (Playlists != null && Playlists.HasMoreItems)
            )
            {
                await GetNextPageAsync();
            }
        }
    }
}
