using System;
using Athame.Core.Download;
using Athame.Plugin.Api.Service;

namespace Athame.Core.Settings
{
    public class AthameSettings : ICloneable
    {
        public MediaPreference GeneralPreference { get; set; }
        public MediaPreference PlaylistPreference { get; set; }

        public PlaylistFileType PlaylistFileType { get; set; }
        public ArtworkFileName ArtworkFileName { get; set; }

        public bool PlaylistUsesGeneralPreference { get; set; }
        public bool DontSavePlaylistArtwork { get; set; }
        public bool WriteWatermark { get; set; }

        public bool ConfirmExit { get; set; }

        // Defaults
        public AthameSettings()
        {
            GeneralPreference = new MediaPreference
            {
                AskLocation = false,
                Location = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                PathFormat = "{AlbumArtistOrArtist}/{Album.Title}/{TrackNumber:00} {Title}"
            };

            PlaylistPreference = new MediaPreference
            {
                AskLocation = false,
                Location = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                PathFormat = "{Playlist.Title}/{Title} - {AlbumArtistOrArtist}"
            };

            PlaylistUsesGeneralPreference = false;
            DontSavePlaylistArtwork = true;
            ArtworkFileName = ArtworkFileName.AsCover;
            PlaylistFileType = PlaylistFileType.None;
            WriteWatermark = true;
            ConfirmExit = true;
        }

        public MediaPreference GetPreference(MediaType type)
            => (type == MediaType.Playlist && !PlaylistUsesGeneralPreference)
                ? PlaylistPreference
                : GeneralPreference.GetCopy();

        public object Clone()
            => new AthameSettings
            {
                GeneralPreference = GeneralPreference.GetCopy(),
                PlaylistPreference = PlaylistPreference.GetCopy(),
                PlaylistUsesGeneralPreference = PlaylistUsesGeneralPreference,
                DontSavePlaylistArtwork = DontSavePlaylistArtwork,
                ArtworkFileName = ArtworkFileName,
                PlaylistFileType = PlaylistFileType,
                WriteWatermark = WriteWatermark,
                ConfirmExit = ConfirmExit,
            };
    }
}