using System;
using System.Windows.Forms;
using Athame.Core.Plugin;
using Athame.PluginAPI.Service;
// ReSharper disable SuspiciousTypeConversion.Global

namespace Athame.UI
{
    public partial class ServiceSettingsView : UserControl
    {

        private readonly MusicService service;
        private readonly PluginInstance servicePlugin;
        private readonly IAuthenticatable authenticatable;

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
                UpdateLabels();
            }
            var control = service.GetSettingsControl();
            control.Dock = DockStyle.Fill;
            servicePanel.Controls.Add(control);
        }

        private void SetSignedOutState()
        {
            signInStatusLabel.Text = "Signed out";
            signInButton.Text = "Sign in";
        }

        private void SetSignedInState()
        {
            signInStatusLabel.Text = "Signed in as " +
                                     LocalisableAccountNameFormat.GetFormattedName(authenticatable.Account);
            signInButton.Text = "Sign out";
        }

        private void UpdateLabels()
        {
            if (authenticatable.IsAuthenticated)
            {
                SetSignedInState();
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
            UpdateLabels();
            servicePlugin.SettingsFile.Save();

        }
    }
}
