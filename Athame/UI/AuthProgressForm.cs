using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Athame.Core.Logging;
using Athame.Core.Plugin;
using Athame.PluginAPI.Service;

namespace Athame.UI
{
    public partial class AuthProgressForm : AthameDialog
    {
        private const string Lag = nameof(AuthProgressForm);

        private readonly ListViewItemAnimator animator = new ListViewItemAnimator(4, 15);
        private readonly AuthenticationManager am = Program.DefaultAuthenticationManager;
        private readonly Dictionary<MusicService, ListViewItem> serviceLvItems = new Dictionary<MusicService, ListViewItem>();

        private IEnumerable<MusicService> services;
        private bool isWorking = true;

        public event EventHandler HiddenOrClosed;

        private void Init(IEnumerable<MusicService> _services)
        {
            services = _services.Where(am.CanRestore);
            InitializeComponent();
            servicesListView.SmallImageList = GlobalImageList.Instance.ImageList;
            foreach (var musicService in services)
            {
                AddServiceListViewItem(musicService);
            }
        }

        public AuthProgressForm()
        {
            Init(Enumerable.Empty<MusicService>());
        }

        public AuthProgressForm(IEnumerable<MusicService> services)
        {
            Init(services);
        }

        private void AddServiceListViewItem(MusicService service)
        {
            var lvItem = new ListViewItem
            {
                Text = "Please wait..."
            };
            lvItem.SubItems.Add(service.Info.Name);
            lvItem.SubItems.Add(LocalisableAccountNameFormat.GetFormattedName(service.AsAuthenticatable().Account));
            serviceLvItems.Add(service, lvItem);
            animator.Add(lvItem);
            servicesListView.Items.Add(lvItem);
        }

        private ListViewItem FindListViewItem(MusicService service)
        {
            return serviceLvItems.ContainsKey(service) ? serviceLvItems[service] : null;
        }

        private void SetLvItemSuccessState(ListViewItem item)
        {
            animator.Remove(item);
            item.ImageKey = "done";
            item.Text = "Signed in successfully.";
        }

        private void SetLvItemFailState(ListViewItem item)
        {
            animator.Remove(item);
            item.ImageKey = "error";
            item.Text = "Error signing in.";
        }

        private async void RestoreAll()
        {
            isWorking = true;
            animator.Start();
            var failResults = new List<AuthenticationResult>();
            var restoreTasks = services.Select(service => am.Restore(service)).ToList();
            Log.Debug(Lag, $"Starting restore, restoreTasks.Count = {restoreTasks.Count}");
            while (restoreTasks.Count > 0)
            {
                var finishedTask = await Task.WhenAny(restoreTasks);
                restoreTasks.Remove(finishedTask);

                var result = await finishedTask;
                
                var lvItem = FindListViewItem(result.Service);
                if (result.Result)
                {
                    Log.Debug(Lag, $"Restore complete for {result}");
                    SetLvItemSuccessState(lvItem);
                }
                else
                {
                    if (result.Exception != null)
                    {
                        Log.WriteException(Level.Error, Lag, result.Exception, $"Restore failed for {result}");
                    }
                    else
                    {
                        Log.Debug(Lag, $"Restore complete for {result}");
                    }
                    failResults.Add(result);
                    SetLvItemFailState(lvItem);
                }
            }

            Log.Debug(Lag, "Finished restore");
            animator.Stop();
            isWorking = false;
            if (failResults.Count == 0)
            {
                Close();
            }
        }

        private void AuthProgressForm_Shown(object sender, EventArgs e)
        {
            RestoreAll();
        }

        private void AuthProgressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            if (!isWorking) return;
            e.Cancel = true;
            Hide();
        }

        private void hideButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AuthProgressForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            HiddenOrClosed?.Invoke(this, EventArgs.Empty);
        }

        private void AuthProgressForm_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible)
            {
                HiddenOrClosed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
