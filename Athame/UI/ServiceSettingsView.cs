using System;
using System.Windows.Forms;
using Athame.Core.Logging;
using Athame.Core.Plugin;
using Athame.Core.Utils;
using Athame.PluginAPI.Service;
using Microsoft.WindowsAPICodePack.Dialogs;

// ReSharper disable SuspiciousTypeConversion.Global

namespace Athame.UI
{
    public partial class ServiceSettingsView : UserControl
    {
        private const string Tag = nameof(ServiceSettingsView);

        private readonly MusicService service;
        private readonly PluginInstance servicePlugin;
        private readonly IAuthenticatable authenticatable;
        private readonly AuthenticationManager am = Program.DefaultAuthenticationManager;

        public ServiceSettingsView(PluginInstance servicePlugin)
        {
            this.servicePlugin = servicePlugin;
            service = servicePlugin.Service;
            authenticatable = service.AsAuthenticatable();
            InitializeComponent();
            if (authenticatable == null)
            {
                authPanel.Visible = false;
            }
            else
            {
                UpdateViews();
            }
            try
            {
                var control = service.GetSettingsControl();
                control.Dock = DockStyle.Fill;
                servicePanel.Controls.Add(control);
            }
            catch (Exception ex)
            {
                Log.WriteException(Level.Error, Tag, ex, "service.GetSettingsControl()");
                var text = "An error occurred while trying to display the service's settings panel.\n\n" + ex;
                var label = new Label
                {
                    AutoSize = false,
                    Dock = DockStyle.Fill,
                    Padding = new Padding(10),
                    Text = text
                };
                servicePanel.Controls.Add(label);
            }
        }

        private void SetSignedOutState()
        {
            restoreButton.Visible = false;
            signInStatusLabel.Text = "Signed out";
            signInButton.Text = "Sign in";
        }

        private void SetSignedInState()
        {
            restoreButton.Visible = false;
            signInStatusLabel.Text = "Signed in as " +
                                     LocalisableAccountNameFormat.GetFormattedName(authenticatable.Account);
            signInButton.Text = "Sign out";
        }

        private void SetRestoreState()
        {
            restoreButton.Visible = true;
            signInStatusLabel.Text =
                "You have saved credentials, but there was an error trying to restore your session.";
        }

        private void UpdateViews()
        {
            if (authenticatable.IsAuthenticated)
            {
                SetSignedInState();
            }
            else if (authenticatable.HasSavedSession)
            {
                SetRestoreState();
            }
            else
            {
                SetSignedOutState();
            }
        }

        private async void signInButton_Click(object sender, EventArgs e)
        {
            if (!authenticatable.IsAuthenticated)
            {
                if (!am.CanAuthenticate(service))
                {
                    TaskDialogHelper.ShowMessage(caption: "Cannot authenticate this service right now.", 
                        message: "The service is already being authenticated in the background. Please try again in a moment.", 
                        icon: TaskDialogStandardIcon.Error, buttons: TaskDialogStandardButtons.Ok, owner: Handle);
                    return;
                }
                var authenticatableAsync = service.AsAuthenticatableAsync();
                if (authenticatableAsync == null)
                {
                    var dlg = new CredentialsForm(service);
                    dlg.ShowDialog();
                }
                else
                {
                    await authenticatableAsync.AuthenticateAsync();
                }
            }
            else
            {
                authenticatable.Reset();
            }
            UpdateViews();
            servicePlugin.SettingsFile.Save();

        }

        private void restoreButton_Click(object sender, EventArgs e)
        {
            if (!am.CanRestore(service))
            {
                TaskDialogHelper.ShowMessage(caption: "Cannot restore this service right now.",
                    message: "The service is already being restored in the background. Please try again in a moment.",
                    icon: TaskDialogStandardIcon.Error, buttons: TaskDialogStandardButtons.Ok, owner: Handle);
                return;
            }
            var form = new AuthProgressForm(new[] {service});
            form.Closed += (o, args) =>
            {
                UpdateViews();
                servicePlugin.SettingsFile.Save();
            };
            form.Show(this);
        }
    }
}
