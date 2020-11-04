using System;
using Athame.Plugin.Api;
using Athame.Plugin.Api.Service;

namespace Athame.Core.Settings
{
    public class AthameSettings : SettingsFile, ICloneable
    {
        public MediaPreference GeneralPreference { get; set; }
        public MediaPreference PlaylistPreference { get; set; }

        public PlaylistFileType PlaylistFileType { get; set; }

        public bool DontSavePlaylistArtwork { get; set; }
        public bool WriteWatermark { get; set; }

        public bool ConfirmExit { get; set; }

        // Defaults
        public AthameSettings()
        {
            GeneralPreference = new MediaPreference
            {
                Location = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                PathFormat = "{AlbumArtistOrArtist}/{Album.Title}/{TrackNumber:00} {Title}"
            };

            PlaylistPreference = new MediaPreference
            {
                Location = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                PathFormat = "{Playlist.Title}/{Title} - {AlbumArtistOrArtist}"
            };

            DontSavePlaylistArtwork = true;
            PlaylistFileType = PlaylistFileType.M3U;
            WriteWatermark = true;
            ConfirmExit = true;
        }

        public MediaPreference GetPreference(MediaType type)
            => type == MediaType.Playlist
                ? PlaylistPreference.Clone() as MediaPreference
                : GeneralPreference.Clone() as MediaPreference;

        public object Clone()
            => new AthameSettings
            {
                SettingsPath = SettingsPath,
                GeneralPreference = GeneralPreference.Clone() as MediaPreference,
                PlaylistPreference = PlaylistPreference.Clone() as MediaPreference,
                DontSavePlaylistArtwork = DontSavePlaylistArtwork,
                PlaylistFileType = PlaylistFileType,
                WriteWatermark = WriteWatermark,
                ConfirmExit = ConfirmExit,
            };

        public override void Save()
            => JsonFileSettings.Save(this);
    }
}