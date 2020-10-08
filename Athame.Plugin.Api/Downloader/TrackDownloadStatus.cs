using System.ComponentModel;

namespace Athame.Plugin.Api.Downloader
{
    public enum TrackDownloadStatus
    {
        [Description("Ready")]
        Ready,

        [Description("Pre-processing")]
        PreProcess,

        [Description("Downloading album artwork")]
        DownloadingAlbumArtwork,

        [Description("Downloading track")]
        DownloadingTrack,

        [Description("Post-processing")]
        PostProcess,

        [Description("Writing tags")]
        WritingTags,

        [Description("Completed")]
        Completed
    }
}
