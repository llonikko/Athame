using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Athame.Core.DownloadAndTag;
using Athame.Core.Logging;
using Athame.Core.Platform;
using Athame.Core.Plugin;
using Athame.Core.Search;
using Athame.Core.Settings;
using Athame.Core.Utils;
using Athame.PluginAPI.Downloader;
using Athame.PluginAPI.Service;
using Athame.Properties;
using Athame.Settings;
using Athame.UI.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace Athame.UI
{
    public partial class MainForm : AthameForm
    {
        /// <summary>
        /// Internal class for keeping track of an individual listitem's relation
        /// </summary>
        private class MediaItemTag
        {
            public EnqueuedCollection Collection { get; set; }
            public Track Track { get; set; }
            public int IndexInCollection { get; set; }
            public int GroupIndex { get; set; }
            public int GlobalItemIndex { get; set; }
            public Exception Exception { get; set; }
        }

        // Constants
        private new const string Tag = nameof(MainForm);
        private const string GroupHeaderFormat = "{2} {0}: {1} ({3} total, {4} available)";
        private const int RetryCount = 3;

        // Read-only instance vars
        private readonly TaskbarManager mTaskbarManager = TaskbarManager.Instance;
        private readonly MediaDownloadQueue mediaDownloadQueue;
        private readonly ListViewItemAnimator animator = new ListViewItemAnimator(4, 15);
        private readonly AuthenticationUi aui;

        // Instance vars
        private UrlResolver resolver;

        private ListViewItem mCurrentlySelectedQueueItem;
        private ListViewItem currentlyDownloadingItem;
        private CollectionDownloadEventArgs currentCollection;
        private bool isListViewDirty;
        private bool isWorking;
        


        public MainForm()
        {
            mediaDownloadQueue = new MediaDownloadQueue
            {
                Tagger = new TrackTagger(),
                UseTempFile = true
            };
            ApplySettings();

            InitializeComponent();
            queueListView.SmallImageList = GlobalImageList.Instance.ImageList;
            resolver = new UrlResolver(Program.DefaultPluginManager, Program.DefaultAuthenticationManager);
            aui = new AuthenticationUi(this);
            UnlockUi();

            // Add event handlers for MDQ
            mediaDownloadQueue.Exception += MediaDownloadQueue_Exception;
            mediaDownloadQueue.CollectionDequeued += MediaDownloadQueue_CollectionDequeued;
            mediaDownloadQueue.TrackDequeued += MediaDownloadQueue_TrackDequeued;
            mediaDownloadQueue.TrackDownloadCompleted += MediaDownloadQueue_TrackDownloadCompleted;
            mediaDownloadQueue.TrackDownloadProgress += MediaDownloadQueue_TrackDownloadProgress;
            //mediaDownloadQueue.TrackSkipped += MediaDownloadQueue_TrackSkipped;

            // Error handler for plugin loader
            Program.DefaultPluginManager.LoadException += DefaultPluginManagerOnLoadException;
        }

        private void MediaDownloadQueue_TrackSkipped(object sender, TrackDownloadEventArgs e)
        {
            if (currentlyDownloadingItem != null)
                animator.Remove(currentlyDownloadingItem);
        }


        private List<Exception> pluginLoadExceptions = new List<Exception>();

        private void DefaultPluginManagerOnLoadException(object sender, PluginLoadExceptionEventArgs pluginLoadExceptionEventArgs)
        {
            if (pluginLoadExceptionEventArgs.Exception.GetType() == typeof(PluginIncompatibleException))
            {
                TaskDialogHelper.ShowMessage("Incompatible plugin",
                    $"The plugin \"{pluginLoadExceptionEventArgs.PluginName}\" is incompatible with this version of Athame.",
                    icon: TaskDialogStandardIcon.Error, buttons: TaskDialogStandardButtons.Ok, owner: Handle);
            }
            else
            {
                pluginLoadExceptions.Add(pluginLoadExceptionEventArgs.Exception);
            }
            pluginLoadExceptionEventArgs.Continue = true;
        }

        /// <summary>
        /// Rounds a decimal less than 1 up then multiplies it by 100. If the result is greater than 100, returns 100, otherwise returns the result.
        /// </summary>
        /// <param name="percent">The percent value to convert.</param>
        /// <returns>An integer that is not greater than 100.</returns>
        private int PercentToInt(decimal percent)
        {
            var rounded = (int)(Decimal.Round(percent, 2, MidpointRounding.ToEven) * (decimal)100);
            return rounded > 100 ? 100 : rounded;
        }

        private void MediaDownloadQueue_TrackDownloadProgress(object sender, TrackDownloadEventArgs e)
        {
            collectionProgressBar.Value = PercentToInt(e.TotalProgress);
            totalProgressBar.Value +=
                PercentToInt(((decimal)(e.TotalProgress + currentCollection.CurrentCollectionIndex) /
                              currentCollection.TotalNumberOfCollections)) - totalProgressBar.Value;
            UpdateTotalStatusText();     
            SetGlobalProgress(totalProgressBar.Value);
            
            switch (e.State)
            {
                case DownloadState.PreProcess:
                    currentlyDownloadingItem.Text = "Downloading...";
                    UpdateCollectionStatusText("Pre-processing...");
                    break;
                case DownloadState.DownloadingAlbumArtwork:
                    UpdateCollectionStatusText("Downloading album artwork...");
                    break;
                case DownloadState.Downloading:
                    UpdateCollectionStatusText("Downloading track...");
                    break;
                case DownloadState.PostProcess:
                    UpdateCollectionStatusText("Post-processing...");
                    break;
                case DownloadState.WritingTags:
                    UpdateCollectionStatusText("Writing tags...");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void MediaDownloadQueue_TrackDownloadCompleted(object sender, TrackDownloadEventArgs e)
        {
            animator.Remove(currentlyDownloadingItem);
            collectionStatusLabel.Text = "Completed";
            currentlyDownloadingItem.ImageKey = "done";
            currentlyDownloadingItem.Text = "Completed";
        }

        private ListViewItem FindListViewItem(Track track)
        {
            return queueListView.Items.Cast<ListViewItem>()
                .FirstOrDefault(item => ((MediaItemTag) item.Tag).Track == track);
        }

        private void MediaDownloadQueue_TrackDequeued(object sender, TrackDownloadEventArgs e)
        {
            // this'll bite me in the ass someday
            if (!e.Track.IsDownloadable) return;
            currentlyDownloadingItem = FindListViewItem(e.Track);
            animator.Add(currentlyDownloadingItem);
        }

        private void MediaDownloadQueue_CollectionDequeued(object sender, CollectionDownloadEventArgs e)
        {
            currentCollection = e;
            UpdateTotalStatusText();
        }

        private void UpdateTotalStatusText()
        {
            totalStatusLabel.Text =
                $"[{totalProgressBar.Value}%] {currentCollection.CurrentCollectionIndex + 1}/{currentCollection.TotalNumberOfCollections}: " +
                $"{currentCollection.Collection.Type} \"{currentCollection.Collection.MediaCollection.Title}\"";
        }

        private void UpdateCollectionStatusText(string state)
        {
            collectionStatusLabel.Text = $"[{collectionProgressBar.Value}%] {state}";
        }

        private void MediaDownloadQueue_Exception(object sender, ExceptionEventArgs e)
        {
            Log.WriteException(Level.Error, Tag, e.Exception, "MDQ exception handler");
            var tag = (MediaItemTag)currentlyDownloadingItem?.Tag;
            if (tag == null)
            {
                Log.Error(Tag, "MDQ exception handler: currently downloading LV item tag is null!");
                throw e.Exception;
            }
            animator.Remove(currentlyDownloadingItem);
            currentlyDownloadingItem.ImageKey = "error";
            currentlyDownloadingItem.Text = "Error occurred while downloading";
            tag.Exception = e.Exception;
            e.SkipTo = ExceptionSkip.Item;

        }

        private string BuildFlags(IEnumerable<Metadata> metadata)
        {
            if (metadata == null) return "";
            var ret = new List<string>();
            foreach (var data in metadata)
            {
                if (!data.CanDisplay) continue;
                if (data.IsFlag)
                {
                    // If it's a flag, only display it if the value is "True" (i.e. Boolean.ToString())
                    if (data.Value == Boolean.TrueString)
                    {
                        ret.Add(data.Name);
                    }
                }
                else
                {
                    ret.Add($"{data.Name}={data.Value}");
                }
            }
            return String.Join(", ", ret);
        }

        #region Download queue manipulation

        private string MakeGroupHeader(EnqueuedCollection collection)
        {
            return String.Format(GroupHeaderFormat, collection.Type, 
                collection.MediaCollection.Title, collection.Service.Info.Name, collection.MediaCollection.Tracks.Count, 
                collection.MediaCollection.GetAvailableTracksCount());
        }

        private void AddToQueue(MusicService service, IMediaCollection item, string destination, string pathFormat)
        {
            var enqueuedItem = mediaDownloadQueue.Enqueue(service, item, destination, pathFormat);
            var header = MakeGroupHeader(enqueuedItem);
            var group = new ListViewGroup(header, HorizontalAlignment.Center);
            var groupIndex = queueListView.Groups.Add(group);
            for (var i = 0; i < item.Tracks.Count; i++)
            {
                var t = item.Tracks[i];
                var lvItem = new ListViewItem
                {
                    Group = group,
                    Tag = new MediaItemTag
                    {
                        Track = t,
                        Collection = enqueuedItem,
                        GroupIndex = groupIndex,
                        IndexInCollection = i
                    }
                };
                if (!t.IsDownloadable)
                {
                    Log.Warning(Tag, $"Adding non-downloadable track {service.Info.Name}/{t.Id}");
                    lvItem.BackColor = SystemColors.Control;
                    lvItem.ForeColor = SystemColors.GrayText;
                    lvItem.ImageKey = "not_downloadable";
                    lvItem.Text = "Unavailable";
                }
                else
                {
                    lvItem.ImageKey = "ready";
                    lvItem.Text = "Ready to download";
                }
                lvItem.SubItems.Add(t.DiscNumber + " / " + t.TrackNumber);
                lvItem.SubItems.Add(t.Title);
                lvItem.SubItems.Add(t.Artist.Name);
                lvItem.SubItems.Add(t.Album.Title);
                lvItem.SubItems.Add(BuildFlags(t.CustomMetadata));
                lvItem.SubItems.Add(Path.Combine(destination, t.GetBasicPath(enqueuedItem.PathFormat, item)));
                group.Items.Add(lvItem);
                queueListView.Items.Add(lvItem);
            }
        }

        private void RemoveCurrentlySelectedTracks()
        {
            if (mCurrentlySelectedQueueItem == null) return;
            var selectedItemsList = queueListView.SelectedItems.Cast<ListViewItem>().ToList();
            foreach (var listViewItem in selectedItemsList)
            {
                var item = (MediaItemTag)listViewItem.Tag;
                item.Collection.MediaCollection.Tracks.Remove(item.Track);
                queueListView.Items.Remove(listViewItem);
                if (item.Collection.MediaCollection.Tracks.Count == 0)
                {
                    mediaDownloadQueue.Remove(item.Collection);
                }
                var group = queueListView.Groups[item.GroupIndex];
                group.Header = MakeGroupHeader(item.Collection);
                
            }
            
            mCurrentlySelectedQueueItem = null;
        }

        private void RemoveCurrentlySelectedGroup()
        {
            if (mCurrentlySelectedQueueItem == null) return;
            // Remove internal queue item
            var item = (MediaItemTag)mCurrentlySelectedQueueItem.Tag;
            mediaDownloadQueue.Remove(item.Collection);
            // We have to remove the group from the ListView first...
            var group = mCurrentlySelectedQueueItem.Group;
            queueListView.Groups.Remove(group);
            // ...then remove all the items
            foreach (var groupItem in group.Items)
            {
                queueListView.Items.Remove((ListViewItem)groupItem);
            }
            mCurrentlySelectedQueueItem = null;
        }
        #endregion

        private void LockUi()
        {
            isWorking = true;
            idTextBox.Enabled = false;
            dlButton.Enabled = false;
            settingsButton.Enabled = false;
            pasteButton.Enabled = false;
            clearButton.Enabled = false;
            startDownloadButton.Enabled = false;
        }

        private void UnlockUi()
        {
            isWorking = false;
            idTextBox.Enabled = true;
            dlButton.Enabled = !String.IsNullOrWhiteSpace(idTextBox.Text);
            settingsButton.Enabled = true;
            pasteButton.Enabled = true;
            clearButton.Enabled = true;
            startDownloadButton.Enabled = mediaDownloadQueue.Count > 0;
        }

        private void SetGlobalProgress(int value)
        {
            if (value == 0)
            {
                mTaskbarManager.SetProgressState(TaskbarProgressBarState.NoProgress);
            }
            mTaskbarManager?.SetProgressValue(value, totalProgressBar.Maximum);
        }

        private void SetGlobalProgressState(ProgressBarState state)
        {
            switch (state)
            {
                case ProgressBarState.Normal:
                    mTaskbarManager?.SetProgressState(TaskbarProgressBarState.Normal);
                    break;
                case ProgressBarState.Error:
                    mTaskbarManager?.SetProgressState(TaskbarProgressBarState.Error);
                    break;
                case ProgressBarState.Warning:
                    mTaskbarManager?.SetProgressState(TaskbarProgressBarState.Paused);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void PresentException(Exception ex)
        {

            Log.WriteException(Level.Error, Tag, ex, "PresentException");
            SetGlobalProgressState(ProgressBarState.Error);
            var th = "An unknown error occurred";
            if (ex is ResourceNotFoundException)
            {
                th = "Resource not found";
            }
            else if (ex is InvalidSessionException)
            {
                th = "Invalid session/subscription expired";
            }
            TaskDialogHelper.ShowExceptionDialog(ex, th, "You may need to sign into this service again.", Handle);
        }

        private MediaTypeSavePreference PreferenceForType(MediaType type)
        {
            if (type == MediaType.Playlist && !Program.DefaultSettings.Settings.PlaylistSavePreferenceUsesGeneral)
            {
                return Program.DefaultSettings.Settings.PlaylistSavePreference;
            }
            return (MediaTypeSavePreference)Program.DefaultSettings.Settings.GeneralSavePreference.Clone();
        }
        private static bool IsWithinVisibleBounds(Point topLeft)
        {
            var screens = Screen.AllScreens;
            return (from screen in screens
                    where screen.WorkingArea.Contains(topLeft)
                    select screen).Any();
        }

        private void RestoreServices()
        {
            LockUi();
            var form = new AuthProgressForm(Program.DefaultPluginManager.ServicesEnumerable());
            form.HiddenOrClosed += (sender, args) => UnlockUi();
            form.Show(this);
        }

        #region Validation for URL
        public const string UrlInvalid = "Invalid URL. Check that the URL begins with \"http://\" or \"https://\".";
        public const string UrlNoService = "Can't download this URL.";
        public const string UrlNeedsAuthentication = "You need to sign in to {0} first. " + UrlNeedsAuthenticationLink1;
        public const string UrlNeedsAuthenticationLink1 = "Click here to sign in.";
        public const string UrlNeedsRestore = "Couldn't sign in to {0}. Go to Settings to attempt sign-in again.";
        public const string UrlNotParseable = "The URL does not point to a valid track, album, artist or playlist.";
        public const string UrlValidParseResult = "{0} from {1}";
        public const string UrlExceptionOccurred = "An exception occurred while trying to parse the URL.";

        private void ValidateEnteredUrl()
        {
            urlValidStateLabel.ResetText();
            urlValidStateLabel.Links.Clear();
            urlValidStateLabel.Image = Resources.error;
            urlValidStateLabel.Visible = true;
            dlButton.Enabled = false;
            
            switch (resolver.Parse(idTextBox.Text))
            {
                case UrlParseState.NullOrEmptyString:
                    urlValidStateLabel.Visible = false;
                    break;

                case UrlParseState.InvalidUrl:
                    urlValidStateLabel.Text = UrlInvalid;
                    break;

                case UrlParseState.NoServiceFound:
                    urlValidStateLabel.Text = UrlNoService;
                    break;

                case UrlParseState.ServiceNotAuthenticated:
                    var service = resolver.Service;
                    urlValidStateLabel.Text = String.Format(UrlNeedsAuthentication, service.Info.Name);
                    var linkIndex = urlValidStateLabel.Text.LastIndexOf(UrlNeedsAuthenticationLink1, StringComparison.Ordinal);
                    urlValidStateLabel.Links.Add(linkIndex, urlValidStateLabel.Text.Length, service);
                    break;

                case UrlParseState.NoMedia:
                    urlValidStateLabel.Text = UrlNotParseable;
                    break;

                case UrlParseState.Success:
                    urlValidStateLabel.Image = Resources.done;
                    urlValidStateLabel.Text = String.Format(UrlValidParseResult, resolver.ParseResult.Type, resolver.Service.Info.Name);
                    dlButton.Enabled = true;
                    break;

                case UrlParseState.Exception:
                    Log.WriteException(Level.Error, Tag, resolver.Exception, "ValidateEnteredUrl()");
                    urlValidStateLabel.Text = UrlExceptionOccurred;
                    break;

                case UrlParseState.ServiceNotRestored:
                    urlValidStateLabel.Text = String.Format(UrlNeedsRestore, resolver.Service.Info.Name);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region Easter egg

        private readonly string[] messages = { "Woo-hoo!", "We did it!", "Yusssss", "Alright!", "Sweet!", "Nice...." };
        private readonly Random random = new Random();

        private string GetCompletionMessage()
        {
            var messagesList = messages.ToList();
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                messagesList.Add("It's Friday, baby!");
            }
            return messagesList[random.Next(messagesList.Count)];
        }

        #endregion

        private void CleanQueueListView()
        {
            collectionStatusLabel.Text = "Ready to begin.";
            collectionProgressBar.Value = 0;
            SetGlobalProgress(0);
            SetGlobalProgressState(ProgressBarState.Normal);
            totalProgressBar.Value = 0;
            totalStatusLabel.Text = "Ready";
            queueListView.Groups.Clear();
            queueListView.Items.Clear();
            isListViewDirty = false;
        }

        #region MainForm event handlers and control event handlers

        private void button1_Click(object sender, EventArgs e)
        {
            ParseAndAddUrl(resolver);
        }

        private void ParseAndAddUrl(UrlResolver r)
        {
            if (!r.HasParsedUrl)
            {
                return;
            }
            if (isListViewDirty)
            {
                CleanQueueListView();
            }
#if !DEBUG
            try
            {
#endif
            // Don't add if the item is already enqueued.
            var isAlreadyInQueue = mediaDownloadQueue.ItemById(r.ParseResult.Id) != null;
            if (isAlreadyInQueue)
            {
                TaskDialogHelper.ShowMessage(owner: Handle, icon: TaskDialogStandardIcon.Error,
                    caption: "Cannot add to download queue",
                    message: "This item already exists in the download queue.",
                    buttons: TaskDialogStandardButtons.Ok);
            }

            // Ask for the location if required before we begin retrieval
            var prefType = PreferenceForType(r.ParseResult.Type);
            var saveDir = prefType.SaveDirectory;
            if (prefType.AskForLocation)
            {
                using (var folderSelectionDialog = new FolderBrowserDialog { Description = "Select a destination for this media:" })
                {
                    if (folderSelectionDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        saveDir = folderSelectionDialog.SelectedPath;
                    }
                    else
                    {
                        return;
                    }
                }
            }

            // Build wait dialog
            var retrievalWaitTaskDialog = new TaskDialog
            {
                Cancelable = false,
                Caption = "Athame",
                InstructionText = $"Getting {r.ParseResult.Type.ToString().ToLower()} details...",
                Text = $"{r.Service.Info.Name}: {r.ParseResult.Id}",
                StandardButtons = TaskDialogStandardButtons.Cancel,
                OwnerWindowHandle = Handle,
                ProgressBar = new TaskDialogProgressBar { State = TaskDialogProgressBarState.Marquee }
            };
            // Open handler
            retrievalWaitTaskDialog.Opened += async (o, args) =>
            {
                LockUi();
                var pathFormat = prefType.GetPlatformSaveFormat();
                try
                {
                    var media = await r.ResolveAsync();
                    AddToQueue(r.Service, media, saveDir, pathFormat);
                }
                catch (ResourceNotFoundException)
                {
                    TaskDialogHelper.ShowMessage(caption: "This media does not exist.",
                        message: "Ensure the provided URL is valid, and try again", owner: Handle,
                        buttons: TaskDialogStandardButtons.Ok, icon: TaskDialogStandardIcon.Information);
                }
                catch (NotImplementedException)
                {
                    TaskDialogHelper.ShowMessage(
                        owner: Handle, icon: TaskDialogStandardIcon.Warning, buttons: TaskDialogStandardButtons.Ok,
                    caption: $"'{r.ParseResult.Type}' is not supported yet.",
                    message: "You may be able to download it in a later release.");
                }
                catch (Exception ex)
                {
                    Log.WriteException(Level.Error, Tag, ex, "While attempting to resolve media");
                    TaskDialogHelper.ShowExceptionDialog(ex,
                        "An error occurred while trying to retrieve information for this media.",
                        "The provided URL may be invalid or unsupported.", Handle);
                }

                idTextBox.Clear();
                UnlockUi();
                retrievalWaitTaskDialog.Close();
            };
            // Show dialog
            retrievalWaitTaskDialog.Show();
#if !DEBUG
        }
            catch (Exception ex)
            {
                PresentException(ex);
            }
#endif
        }

        private void RestoreFormPositionAndSize()
        {
            if (!IsWithinVisibleBounds(Program.DefaultSettings.Settings.MainWindowPreference.Location) ||
                Program.DefaultSettings.Settings.MainWindowPreference.Location == new Point(0, 0))
            {
                CenterToScreen();
            }
            else
            {
                Location = Program.DefaultSettings.Settings.MainWindowPreference.Location;
            }

            var savedSize = Program.DefaultSettings.Settings.MainWindowPreference.Size;
            if (savedSize.Width < MinimumSize.Width && savedSize.Height < MinimumSize.Height)
            {
                Program.DefaultSettings.Settings.MainWindowPreference.Size = savedSize = MinimumSize;
            }
            Size = savedSize;

            if (Program.DefaultSettings.Settings.MainWindowPreference.FormWindowState == FormWindowState.Minimized)
            {
                return;
            }
            WindowState = Program.DefaultSettings.Settings.MainWindowPreference.FormWindowState;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RestoreFormPositionAndSize();
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            var absLoc = settingsButton.PointToScreen(new Point(0, settingsButton.Height));
            mMenu.Show(absLoc);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mediaDownloadQueue.Count > 0 && Program.DefaultSettings.Settings.ConfirmExit)
            {
                var msgResult = TaskDialogHelper.ShowMessage("Are you sure you want to exit Athame?",
                    "You have items waiting in the download queue.", TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No, TaskDialogStandardIcon.Warning, Handle);

                if (msgResult != TaskDialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }
            Program.DefaultSettings.Save();
        }

        private void idTextBox_TextChanged(object sender, EventArgs e)
        {
            ValidateEnteredUrl();
        }

        private void clearButton_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            idTextBox.Clear();
        }

        private void pasteButton_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            idTextBox.Clear();
            idTextBox.Paste();
        }

        private async void urlValidStateLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var svc = (MusicService)e.Link.LinkData;
            var auth = svc.AsAuthenticatable();
            if (auth.IsAuthenticated)
            {
                ValidateEnteredUrl();
                return;
            }
            if (auth.HasSavedSession)
            {
                var f = aui.RestoreSingle(svc);
                if (f != null)
                {
                    f.Closed += (o, args) => ValidateEnteredUrl();
                }
            }
            else
            {
                await aui.Authenticate(svc);
                ValidateEnteredUrl();
            }
        }

        private async Task StartDownload()
        {
            if (mediaDownloadQueue.Count == 0)
            {
                TaskDialogHelper.ShowMessage(owner: Handle, icon: TaskDialogStandardIcon.Error, buttons: TaskDialogStandardButtons.Ok, 
                    caption: "No tracks are in the queue.",
                    message:
                    "You can add tracks by copying the URL to an album, artist, track, or playlist and pasting it into Athame.");
                return;
            }
            isListViewDirty = true;

            KeepAwakeContext pwrCtx = null;
            try
            {
                if (Program.DefaultSettings.Settings.KeepSystemAwake)
                {
                    pwrCtx = KeepAwakeContext.Create(KeepAwakeRequirement.System);
                }
                animator.Start();
                LockUi();
                totalStatusLabel.Text = "Warming up...";
                await mediaDownloadQueue.StartDownloadAsync();
                totalStatusLabel.Text = "All downloads completed";
                collectionStatusLabel.Text = GetCompletionMessage();
                currentlyDownloadingItem = null;
                mediaDownloadQueue.Clear();
                SetGlobalProgress(0);
                SystemSounds.Beep.Play();
                this.Flash(FlashMethod.All | FlashMethod.TimerNoForeground, Int32.MaxValue, 0);
            }
            catch (Exception ex)
            {
                PresentException(ex);
            }
            finally
            {
                pwrCtx?.SetKeepAwakeRequirement(KeepAwakeRequirement.Clear);
                UnlockUi();
                animator.Stop();
            }
        }

        private async void startDownloadButton_Click(object sender, EventArgs e)
        {
            await StartDownload();
        }

        private void queueListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (!queueListView.FocusedItem.Bounds.Contains(e.Location)) return;
            mCurrentlySelectedQueueItem = queueListView.FocusedItem;
            // Only show context menu on right click
            if (e.Button != MouseButtons.Right) return;
            showCollectionInFileBrowserToolStripMenuItem.Enabled = GetCurrentlySelectedItemDir() != null;
            removeTrackToolStripMenuItem.Text = queueListView.SelectedItems.Count == 1 ? "Remove item" : "Remove items";
            queueMenu.Show(Cursor.Position);
        }



        private void removeGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveCurrentlySelectedGroup();
        }

        private void queueListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (queueListView.SelectedIndices.Count == 0) return;
            mCurrentlySelectedQueueItem = queueListView.SelectedItems[0];
        }

        private void queueListView_MouseHover(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog(this);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSettings();
        }

        private void ShowSettings()
        {
            var form = new SettingsForm();
            if (form.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }
            ApplySettings();
        }

        private void ApplySettings()
        {
            mediaDownloadQueue.SavePlaylist = Program.DefaultSettings.Settings.SavePlaylist;
            mediaDownloadQueue.Tagger.AlbumArtworkSaveFormat = Program.DefaultSettings.Settings.AlbumArtworkSaveFormat;
            mediaDownloadQueue.Tagger.IgnoreSaveArtworkWithPlaylist =
                Program.DefaultSettings.Settings.IgnoreSaveArtworkWithPlaylist;
            mediaDownloadQueue.Tagger.WriteWatermarkTags = Program.DefaultSettings.Settings.WriteWatermarkTags;

        }

        private void removeTrackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveCurrentlySelectedTracks();
        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            Program.DefaultSettings.Settings.MainWindowPreference.Location = Location;
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            Program.DefaultSettings.Settings.MainWindowPreference.Size = Size;
            Program.DefaultSettings.Settings.MainWindowPreference.FormWindowState = WindowState;
        }

        private void LoadAndInitPlugins()
        {
            Program.DefaultPluginManager.LoadAll();
            Program.DefaultPluginManager.InitAll(Program.DefaultApp);
            if (pluginLoadExceptions.Count > 0)
            {
                TaskDialogHelper.CreateMessageDialog("Plugin load error",
                    "One or more errors occurred while loading plugins. Some plugins may be unavailable. Check the log for more details.",
                    TaskDialogStandardButtons.Ok, TaskDialogStandardIcon.Warning, Handle).Show();
                pluginLoadExceptions.Clear();
            }
            if (!Program.DefaultPluginManager.AreAnyLoaded)
            {
                if (TaskDialogHelper.CreateMessageDialog("No plugins installed",
                    "No plugins could be found. If you have attempted to install a plugin, it may not be installed properly.",
                    TaskDialogStandardButtons.Ok, TaskDialogStandardIcon.Error, Handle).Show() != TaskDialogResult.No)
                {
                    Application.Exit();
                }

            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            LockUi();
            LoadAndInitPlugins();
            RestoreServices();
        }

        private void queueListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (e.Shift)
                {
                    RemoveCurrentlySelectedGroup();
                }
                else
                {
                    RemoveCurrentlySelectedTracks();
                }

            }
        }
        #endregion

        private string GetCurrentlySelectedItemDir()
        {
            if (mCurrentlySelectedQueueItem == null) return null;
            var tag = (MediaItemTag)mCurrentlySelectedQueueItem.Tag;
            var parentDir = Path.GetDirectoryName(Path.Combine(tag.Collection.Destination, tag.Track.GetBasicPath(tag.Collection.PathFormat, tag.Collection.MediaCollection)));
            return Directory.Exists(parentDir) ? parentDir : null;
        }

        private void showCollectionInFileBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dir = GetCurrentlySelectedItemDir();
            if (dir == null) return;
            Process.Start($"\"{dir}\"");
        }

        private void ShowDetails()
        {
            var tag = (MediaItemTag) mCurrentlySelectedQueueItem?.Tag;
            if (tag?.Exception == null) return;
            TaskDialogHelper.ShowExceptionDialog(tag.Exception, "An error occurred while downloading this track",
                "Check you can play this track on the web, check that you have a subscription, or try signing in and out.",
                Handle);
        }

        private void queueListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowDetails();
        }

        private void queueMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            removeTrackToolStripMenuItem.Enabled = !isWorking;
            removeGroupToolStripMenuItem.Enabled = !isWorking;
        }

        private void bulkAddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new BulkAddForm();
            if (form.ShowDialog(this) != DialogResult.OK) return;
            foreach (var formResolver in form.Resolvers)
            {
                ParseAndAddUrl(formResolver);
            }
        }
    }
}
