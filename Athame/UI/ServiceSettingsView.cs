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
        private const string Lag = nameof(ServiceSettingsView);

        private readonly MusicService service;
        private readonly PluginInstance servicePlugin;
        private readonly IAuthenticatable authenticatable;
        private readonly AuthenticationManager am = Program.DefaultAuthenticationManager;
        private readonly AuthenticationUi aui;

        public ServiceSettingsView(PluginInstance servicePlugin)
        {
            this.servicePlugin = servicePlugin;
            service = servicePlugin.Service;
            authenticatable = service.AsAuthenticatable();
            InitializeComponent();
            aui = new AuthenticationUi(ParentForm);
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
                Log.WriteException(Level.Error, Lag, ex, "service.GetSettingsControl()");
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
                await aui.Authenticate(service);
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
            var form = aui.RestoreSingle(service);
            if (form == null) return;
            form.Closed += (o, args) =>
            {
                UpdateViews();
                servicePlugin.SettingsFile.Save();
            };
        }
    }
}
