using System.Collections.Generic;
using System.Threading.Tasks;

namespace Athame.Plugin.Api.Service
{
    /// <summary>
    /// Represents a collection of items that are retrieved as pages from a service.
    /// </summary>
    /// <typeparam name="T">The type of each item.</typeparam>
    public abstract class PagedList<T>
    {
        private readonly List<T> items = new List<T>();

        protected PagedList(int itemsPerPage)
        {
            ItemsPerPage = itemsPerPage;
        }

        public IEnumerable<T> Items => items;

        public int ItemsPerPage { get; }
        
        public virtual int Count { get; }
        public virtual bool HasMoreItems { get; }

        public abstract Task<IEnumerable<T>> GetNextPageAsync();

        public async Task LoadAllPagesAsync()
        {
            while (HasMoreItems)
            {
                items.AddRange(
                    await GetNextPageAsync().ConfigureAwait(false));
            }
        }
    }
}
