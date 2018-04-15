using System;

namespace Athame.Core.DownloadAndTag
{
    public class CollectionDownloadEventArgs : EventArgs
    {
        public EnqueuedCollection Collection { get; set; }

        public int CurrentCollectionIndex { get; set; }

        public int TotalNumberOfCollections { get; set; }
    }
}