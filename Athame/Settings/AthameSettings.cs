using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Athame.Core.DownloadAndTag;

namespace Athame.Settings
{
    public class MediaTypeSavePreference
    {
        public string SaveDirectory { get; set; }
        public string SaveFormat { get; set; }
        public bool AskForLocation { get; set; }

        public string GetPlatformSaveFormat()
        {
            return Path.DirectorySeparatorChar == '/' ? SaveFormat : SaveFormat.Replace('/', Path.DirectorySeparatorChar);
        }

        public MediaTypeSavePreference Clone()
        {
            return new MediaTypeSavePreference
            {
                AskForLocation = AskForLocation,
                SaveDirectory = SaveDirectory,
                SaveFormat = SaveFormat
            };
        }
    }

    public class WindowPreference
    {
        public Point Location { get; set; }
        public Size Size { get; set; }

        public void CopyFrom(Form form)
        {
            Location = form.Location;
            Size = form.Size;
        }

        public void CopyTo(Form form)
        {
            form.Location = Location;
            form.Size = Size;
        }
    }

    public class AthameSettings
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
        }

        public AlbumArtworkSaveFormat AlbumArtworkSaveFormat { get; set; }
        public MediaTypeSavePreference GeneralSavePreference { get; set; }
        public MediaTypeSavePreference PlaylistSavePreference { get; set; }
        public bool PlaylistSavePreferenceUsesGeneral { get; set; }
        public WindowPreference MainWindowPreference { get; set; }
        public SavePlaylistSetting SavePlaylist { get; set; }

        public bool ConfirmExit { get; set; }
        public bool IgnoreSaveArtworkWithPlaylist { get; set; }
    }
}
