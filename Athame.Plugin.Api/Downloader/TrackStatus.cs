using System.ComponentModel;

namespace Athame.Plugin.Api.Downloader
{
    public enum TrackStatus
    {
        [Description("Ready")]
        Ready,

        [Description("Pre-processing")]
        PreProcess,

        [Description("Downloading artwork")]
        DownloadingArtwork,

        [Description("Downloading track")]
        DownloadingTrack,

        [Description("Post-processing")]
        PostProcess,

        [Description("Writing tags")]
        WritingTags,

        [Description("Download completed")]
        DownloadCompleted,

        [Description("Skipping track")]
        SkippingTrack,

        [Description("Download failed")]
        DownloadFailed
    }
}
