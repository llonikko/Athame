using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Athame.DownloadAndTag;
using Athame.Plugin;
using Athame.PluginAPI.Service;
using Athame.Settings;
using Athame.Utils;

namespace Athame.UI
{
    public partial class SettingsForm : Form
    {
        private readonly AthameSettings defaults = Program.DefaultSettings.Settings;
        private readonly List<PluginInstance> services;
        private readonly RandomWords randomWords = new RandomWords();
        private readonly Album pathFormatSampleAlbum;
        private readonly Playlist pathFormatSamplePlaylist;

        public SettingsForm()
        {
            InitializeComponent();
            // General save
            saveToRadioButton.Checked = !defaults.GeneralSavePreference.AskForLocation;
            askWhereToSaveRadioButton.Checked = defaults.GeneralSavePreference.AskForLocation;
            saveLocLabel.Text = defaults.GeneralSavePreference.SaveDirectory;
            pathFormatTextBox.Text = defaults.GeneralSavePreference.SaveFormat;
            savePlaylistAsComboBox.SelectedIndex = (int)defaults.SavePlaylist;
            confirmExitCheckBox.Checked = defaults.ConfirmExit;
            ignoreAlbumArtworkCheckBox.Checked = defaults.IgnoreSaveArtworkWithPlaylist;

            // Inherit checkbox
            pldSameAsAlbumTrack.Checked = defaults.PlaylistSavePreferenceUsesGeneral;

            // Playlist save
            pldSaveToRadioButton.Checked = !defaults.PlaylistSavePreference.AskForLocation;
            pldAskWhereToSaveRadioButton.Checked = defaults.PlaylistSavePreference.AskForLocation;
            pldSaveLocLabel.Text = defaults.PlaylistSavePreference.SaveDirectory;
            pldPathFormatTextBox.Text = defaults.PlaylistSavePreference.SaveFormat;

            // Album artwork save setting
            switch (defaults.AlbumArtworkSaveFormat)
            {
                case AlbumArtworkSaveFormat.DontSave:
                    artworkDontSaveRadioButton.Checked = true;
                    break;
                case AlbumArtworkSaveFormat.AsCover:
                    artworkSaveAsFileRadioButton.Checked = true;
                    break;
                case AlbumArtworkSaveFormat.AsArtistAlbum:
                    artworkSaveAsFormattedFileRadioButton.Checked = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Populate services
            services = Program.DefaultPluginManager.Plugins;
            foreach (var service in services)
            {
                if (service.Info != null)
                {
                    servicesListBox.Items.Add(service.Info.Name);
                }
            }
            if (servicesListBox.Items.Count > 0) servicesListBox.SelectedIndex = 0;

            pathFormatSampleAlbum = new Album
            {
                Artist = new Artist { Name = randomWords.NewArtistName() },
                Title = randomWords.NewTitle(),
                Tracks = new List<Track>()
            };
            var track = new Track
            {
                Album = pathFormatSampleAlbum,
                Artist = pathFormatSampleAlbum.Artist,
                DiscNumber = 1,
                TrackNumber = 1,
                Genre = "Genre",
                Composer = randomWords.NewFullName(),
                Title = randomWords.NewTitle(),
                Year = DateTime.Now.Year
            };
            pathFormatSampleAlbum.Tracks.Add(track);

            pathFormatSamplePlaylist = new Playlist
            {
                Title = randomWords.NewVerbNounTitle(),
                Tracks = new List<Track>()
            };

            var plTrack = new Track
            {
                DiscNumber = 1,
                TrackNumber = 1,
                Genre = "Genre",
                Composer = randomWords.NewFullName(),
                Title = randomWords.NewTitle(),
                Year = DateTime.Now.Year
            };
            plTrack.Album = new Album {
                Artist = new Artist {Name = randomWords.NewArtistName()},
                Title = randomWords.NewTitle(),
                Tracks = new List<Track>()
            };
            plTrack.Artist = plTrack.Album.Artist;
            plTrack.Album.Tracks.Add(plTrack);
            pathFormatSamplePlaylist.Tracks.Add(plTrack);
        }


        #region ' General save event handlers 

        private void saveLocBrowseButton_Click(object sender, EventArgs e)
        {
            if (mFolderBrowserDialog.ShowDialog() != DialogResult.OK) return;
            defaults.GeneralSavePreference.SaveDirectory = mFolderBrowserDialog.SelectedPath;
            saveLocLabel.Text = defaults.GeneralSavePreference.SaveDirectory;
        }

        private void pathFormatTextBox_TextChanged(object sender, EventArgs e)
        {
            ValidateTrackFormat();
            defaults.GeneralSavePreference.SaveFormat = pathFormatTextBox.Text;
        }

        private void saveToRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            defaults.GeneralSavePreference.AskForLocation = !saveToRadioButton.Checked;
        }

        private void askWhereToSaveRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            defaults.GeneralSavePreference.AskForLocation = askWhereToSaveRadioButton.Checked;
        }
        #endregion

        #region ' Playlist save event handlers 

        private void pldSaveToRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            defaults.PlaylistSavePreference.AskForLocation = !pldSaveToRadioButton.Checked;
        }

        private void pldAskWhereToSaveRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            defaults.PlaylistSavePreference.AskForLocation = pldAskWhereToSaveRadioButton.Checked;
        }

        private void pldPathFormatTextBox_TextChanged(object sender, EventArgs e)
        {
            ValidatePlaylistFormat();
            defaults.PlaylistSavePreference.SaveFormat = pldPathFormatTextBox.Text;
        }

        private void pldSaveLocBrowseButton_Click(object sender, EventArgs e)
        {
            if (mFolderBrowserDialog.ShowDialog() != DialogResult.OK) return;
            defaults.PlaylistSavePreference.SaveDirectory = mFolderBrowserDialog.SelectedPath;
            saveLocLabel.Text = defaults.PlaylistSavePreference.SaveDirectory;
        }

        #endregion


        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Program.DefaultSettings.Save();
            selectedInstance?.SettingsFile.Save();
            DialogResult = DialogResult.OK;
        }

        private PluginInstance selectedInstance;
        private MusicService selectedService;

        private void servicesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedInstance = services[servicesListBox.SelectedIndex];
            selectedService = selectedInstance.Service;
            serviceUiPanel.Controls.Clear();
            if (selectedService.AsAuthenticatable() != null)
            {
                var ssv = new ServiceSettingsView(selectedInstance) {Dock = DockStyle.Fill};
                serviceUiPanel.Controls.Add(ssv);
            }
            else
            {
                serviceUiPanel.Controls.Add(selectedService.GetSettingsControl());
            }
            serviceNameLabel.Text = selectedService.Info.Name;
            serviceDescriptionLabel.Text = selectedService.Info.Description;
            serviceAuthorLabel.Text = selectedService.Info.Author;
            serviceWebsiteLabel.Text = selectedService.Info.Website.ToString();
            serviceVersionLabel.Text = selectedInstance.Assembly.GetName().Version.ToString();
        }

        private void formatHelpButton_Click(object sender, EventArgs e)
        {
            var formatForm = new PathFormatHelpForm();
            var rightTopCorner = new Point(Location.X + Width, Location.Y);
            formatForm.StartPosition = FormStartPosition.Manual;
            formatForm.Location = rightTopCorner;
            formatForm.Show(this);
            
        }
        private void pldSameAsAlbumTrack_CheckedChanged(object sender, EventArgs e)
        {
            defaults.PlaylistSavePreferenceUsesGeneral = pldSameAsAlbumTrack.Checked;
            pldOptionsGroupBox.Enabled = !pldSameAsAlbumTrack.Checked;
        }

        #region ' Album artwork radio buttons 
        private void artworkSaveAsFileRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (artworkSaveAsFileRadioButton.Checked) defaults.AlbumArtworkSaveFormat = AlbumArtworkSaveFormat.AsCover;
        }

        private void artworkSaveAsFormattedFileRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (artworkSaveAsFormattedFileRadioButton.Checked) defaults.AlbumArtworkSaveFormat = AlbumArtworkSaveFormat.AsArtistAlbum;
        }

        private void artworkDontSaveRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (artworkDontSaveRadioButton.Checked) defaults.AlbumArtworkSaveFormat = AlbumArtworkSaveFormat.DontSave;
        }
        #endregion

        private void openPluginDirButton_Click(object sender, EventArgs e)
        {
            Process.Start("\"" + Program.DefaultPluginManager.PluginDirectory + "\"");
        }

        private void serviceWebsiteLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (selectedService == null) return;
            Process.Start("\"" + selectedService.Info.Website.ToString() + "\"");
        }

        private void savePlaylistAsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            defaults.SavePlaylist = (SavePlaylistSetting)savePlaylistAsComboBox.SelectedIndex;
        }

        private void confirmExitCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            defaults.ConfirmExit = confirmExitCheckBox.Checked;
        }

        private void ignoreAlbumArtworkCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            defaults.IgnoreSaveArtworkWithPlaylist = confirmExitCheckBox.Checked;
        }

        private void ValidateTrackFormat()
        {
            ValidateFormat(pathFormatSampleAlbum, pathFormatTextBox.Text, compiledPathFormatLabel);
        }

        private void ValidatePlaylistFormat()
        {
            ValidateFormat(pathFormatSamplePlaylist, pldPathFormatTextBox.Text, compiledPlaylistPathFormatLabel);
        }

        private void ValidateFormat(IMediaCollection collection, string format, Label label)
        {
            label.ForeColor = Color.Red;
            try
            {
                label.ForeColor = Color.Black;
                label.Text = collection.Tracks[0].GetBasicPath(format, collection) + ".ext";
            }
            catch (FormatException)
            {
                label.Text = "Improperly formed format string - did you forget a { or }?";
            }
            catch (NullReferenceException)
            {
                label.Text =
                    "Undefined variable referenced - click the \"Help with path formats\" button to learn more.";
            }
            catch (Exception ex)
            {
                label.Text =
                    $"Unknown error ({ex.GetType().Name})";
            }
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            ValidateTrackFormat();
            ValidatePlaylistFormat();
        }
    }
}
