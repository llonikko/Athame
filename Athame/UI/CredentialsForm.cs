using System;
using System.Diagnostics;
using System.Media;
using System.Windows.Forms;
using Athame.Core.Logging;
using Athame.Core.Plugin;
using Athame.PluginAPI.Service;

namespace Athame.UI
{

    public partial class CredentialsForm : AthameDialog
    {
        private const string Lag = nameof(CredentialsForm);

        private readonly MusicService svc;
        private readonly IUsernamePasswordAuthenticationAsync usernamePasswordService;
        private readonly AuthenticationManager am = Program.DefaultAuthenticationManager;

        public CredentialsForm(MusicService service)
        {
            InitializeComponent();
            svc = service;
            usernamePasswordService = service.AsUsernamePasswordAuthenticatable();
            FillInInfo();
        }

        private void FillInInfo()
        {
            
            Text = String.Format(Text, svc.Info.Name);
            if (usernamePasswordService.SignInLinks == null) return;
            helpLabel.Text = usernamePasswordService.SignInHelpText ?? String.Format(helpLabel.Text, svc.Info.Name);
            foreach (var linkPair in usernamePasswordService.SignInLinks)
            {
                var button = new Button
                {
                    Text = linkPair.DisplayName,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    UseVisualStyleBackColor = true
                };
                urlToolTip.SetToolTip(button, linkPair.Link.ToString());
                // "This will work" - Joe, 28/09/16
                // ReSharper disable once AccessToForEachVariableInClosure
                button.Click += (sender, args) => Process.Start(linkPair.Link.ToString());
                linksPanel.Controls.Add(button);
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            var waitForm = TaskDialogHelper.CreateWaitDialog($"Signing into {svc.Info.Name}...", Handle);
            waitForm.Opened += async (o, args) => {
                var result = await am.Authenticate(svc, emailTextBox.Text, passwordTextBox.Text, true);
                waitForm.Close();
                if (result.Result)
                {
                    DialogResult = DialogResult.OK;
                    return;
                }
                if (result.Exception != null)
                {
                    Log.WriteException(Level.Error, Lag, result.Exception, "AM credential auth");
                }
                
                errorLabel.Text = "An error occurred while signing in. Please check your credentials and try again.";
                SystemSounds.Hand.Play();
                
            };
            waitForm.Show();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
