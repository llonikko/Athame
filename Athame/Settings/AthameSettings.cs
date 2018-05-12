using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Athame.Core.DownloadAndTag;

namespace Athame.Settings
{
    public class MediaTypeSavePreference : ICloneable
    {
        public string SaveDirectory { get; set; }
        public string SaveFormat { get; set; }
        public bool AskForLocation { get; set; }

        public string GetPlatformSaveFormat()
        {
            return Path.DirectorySeparatorChar == '/' ? SaveFormat : SaveFormat.Replace('/', Path.DirectorySeparatorChar);
        }

        public object Clone()
        {
            return new MediaTypeSavePreference
            {
                AskForLocation = AskForLocation,
                SaveDirectory = String.Copy(SaveDirectory),
                SaveFormat = String.Copy(SaveFormat)
            };
        }
    }

    public class WindowPreference : ICloneable
    {
        public Point Location { get; set; }
        public Size Size { get; set; }
        public FormWindowState FormWindowState { get; set; }

        public object Clone()
        {
            return new WindowPreference
            {
                Location = Location,
                Size = Size,
                FormWindowState = FormWindowState
            };
        }
    }

    public class AthameSettings : ICloneable
    {
        // Defaults
        public AthameSettings()
        {
            AlbumArtworkSaveFormat = AlbumArtworkSaveFormat.DontSave;
            GeneralSavePreference = new MediaTypeSavePreference
            {
                AskForLocation = false,
                SaveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                SaveFormat = "{AlbumArtistOrArtist} - {Album.Title}/{TrackNumber} {Title}"
            };
            PlaylistSavePreference = new MediaTypeSavePreference
            {
                AskForLocation = false,
                SaveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                SaveFormat = "{PlaylistName}/{Title} - {AlbumArtistOrArtist}"
            };
            PlaylistSavePreferenceUsesGeneral = false;
            MainWindowPreference = new WindowPreference();
            SavePlaylist = SavePlaylistSetting.DontSave;
            ConfirmExit = true;
            IgnoreSaveArtworkWithPlaylist = true;
            KeepSystemAwake = false;
            WriteWatermarkTags = true;
        }

        public AlbumArtworkSaveFormat AlbumArtworkSaveFormat { get; set; }
        public MediaTypeSavePreference GeneralSavePreference { get; set; }
        public MediaTypeSavePreference PlaylistSavePreference { get; set; }
        public bool PlaylistSavePreferenceUsesGeneral { get; set; }
        public WindowPreference MainWindowPreference { get; set; }
        public SavePlaylistSetting SavePlaylist { get; set; }

        public bool ConfirmExit { get; set; }
        public bool IgnoreSaveArtworkWithPlaylist { get; set; }

        public bool KeepSystemAwake { get; set; }

        public bool WriteWatermarkTags { get; set; }

        public object Clone()
        {
            return new AthameSettings
            {
                PlaylistSavePreference = (MediaTypeSavePreference)PlaylistSavePreference.Clone(),
                GeneralSavePreference = (MediaTypeSavePreference)GeneralSavePreference.Clone(),
                AlbumArtworkSaveFormat = AlbumArtworkSaveFormat,
                IgnoreSaveArtworkWithPlaylist = IgnoreSaveArtworkWithPlaylist,
                SavePlaylist = SavePlaylist,
                WriteWatermarkTags = WriteWatermarkTags,
                PlaylistSavePreferenceUsesGeneral = PlaylistSavePreferenceUsesGeneral,
                KeepSystemAwake = KeepSystemAwake,
                ConfirmExit = ConfirmExit,
                MainWindowPreference = (WindowPreference)MainWindowPreference.Clone()
            };
        }
    }
}
