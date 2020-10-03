namespace Athame.Plugin.Api.Downloader
{
    public class ProgressInfo
    {
        public int PercentCompleted => TotalBytesToReceive < 0 
            ? 0 : TotalBytesToReceive == 0 
            ? 100 : (int)((100 * BytesReceived) / TotalBytesToReceive);

        public long BytesReceived { get; set; }
        public long TotalBytesToReceive { get; set; }
    }
}
