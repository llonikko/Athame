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
using Athame.Core.Search;
using Athame.PluginAPI.Service;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Athame.UI
{
    public partial class BulkAddForm : AthameDialog
    {
        private StringBuilder errorsStringBuilder = new StringBuilder();

        public readonly List<UrlResolver> Resolvers = new List<UrlResolver>();

        public BulkAddForm()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            errorsStringBuilder.Clear();
            Resolvers.Clear();
            ProcessUrls();
            if (errorsStringBuilder.Length > 0)
            {
                var messageDialog = TaskDialogHelper.CreateMessageDialog("Invalid URLs",
                    errorsStringBuilder.ToString().Trim(), TaskDialogStandardButtons.None, TaskDialogStandardIcon.Error,
                    Handle);

                var goBackButton = new TaskDialogButton("goBack", "Go back and fix URLs");
                goBackButton.Click += (o, args) =>
                {
                    messageDialog.Close();
                };

                var continueButton = new TaskDialogButton("continue", "Continue");
                continueButton.Click += (o, args) =>
                {
                    messageDialog.Close();
                    DialogResult = DialogResult.OK;
                };
                messageDialog.Controls.Add(goBackButton);
                messageDialog.Controls.Add(continueButton);

                messageDialog.Show();
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void ProcessUrls()
        {
            var lines = urlsTextBox.Lines;
            foreach (var line in lines)
            {
                if (String.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                var resolver = new UrlResolver(Program.DefaultPluginManager,
                    Program.DefaultAuthenticationManager);
                switch (resolver.Parse(line))
                {
                    case UrlParseState.NullOrEmptyString:
                        continue;
                    case UrlParseState.InvalidUrl:
                        AddErrorLine(line, MainForm.UrlInvalid);
                        continue;
                    case UrlParseState.NoServiceFound:
                        AddErrorLine(line, MainForm.UrlNoService);
                        continue;
                    case UrlParseState.ServiceNotAuthenticated:
                        AddErrorLine(line, String.Format(MainForm.UrlNeedsAuthentication, resolver.Service.Info.Name));
                        continue;
                    case UrlParseState.ServiceNotRestored:
                        AddErrorLine(line, String.Format(MainForm.UrlNeedsRestore, resolver.Service.Info.Name));
                        continue;
                    case UrlParseState.NoMedia:
                        AddErrorLine(line, MainForm.UrlNotParseable);
                        continue;
                    case UrlParseState.Success:
                        break;
                    case UrlParseState.Exception:
                        AddErrorLine(line, MainForm.UrlExceptionOccurred);
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                Resolvers.Add(resolver);
            }
        }

        private void AddErrorLine(string url, string message)
        {
            errorsStringBuilder.AppendFormat("{0}: {1}\n\n", url, message);
        }
    }
}
