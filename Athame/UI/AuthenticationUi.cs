using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Athame.Core.Logging;
using Athame.Core.Plugin;
using Athame.PluginAPI.Service;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Athame.UI
{
    public class AuthenticationUi
    {
        private const string Tag = nameof(AuthenticationUi);
        private readonly AuthenticationManager am = Program.DefaultAuthenticationManager;
        private readonly Form parent;

        public AuthenticationUi(Form form)
        {
            parent = form;
        }

        public AuthProgressForm RestoreSingle(MusicService service)
        {
            if (!am.CanRestore(service))
            {
                TaskDialogHelper.ShowMessage(caption: "Cannot restore this service right now.",
                    message: "The service is already being restored in the background. Please try again in a moment.",
                    icon: TaskDialogStandardIcon.Error, buttons: TaskDialogStandardButtons.Ok, owner: parent.Handle);
                return null;
            }
            var form = new AuthProgressForm(new[] { service });
            form.Show(parent);
            return form;
        }

        public async Task<bool> Authenticate(MusicService service)
        {
            if (!am.CanAuthenticate(service))
            {
                TaskDialogHelper.ShowMessage(caption: "Cannot authenticate this service right now.",
                    message: "The service is already being authenticated in the background. Please try again in a moment.",
                    icon: TaskDialogStandardIcon.Error, buttons: TaskDialogStandardButtons.Ok, owner: parent.Handle);
                return false;
            }
            var authenticatableAsync = service.AsAuthenticatableAsync();
            if (authenticatableAsync == null)
            {
                var dlg = new CredentialsForm(service);
                return dlg.ShowDialog() == DialogResult.OK;
            }
            var result = await am.Authenticate(service);
            if (result.Result) return true;
            if (result.Exception != null)
            {
                Log.WriteException(Level.Error, Tag, result.Exception, "AM custom auth");
                TaskDialogHelper.ShowExceptionDialog(result.Exception,
                    "An error occurred while attempting to sign in.",
                    "Make sure you have entered the correct credentials and your device has an active internet connection.\n\n" +
                    "The information below may be useful to the plugin's author.", parent.Handle);
            }
            else
            {
                TaskDialogHelper.ShowMessage("An error occurred while attempting to sign in.",
                    "Make sure you have entered the correct credentials and your device has an active internet connection.",
                    TaskDialogStandardButtons.Ok, TaskDialogStandardIcon.Error, parent.Handle);
            }
            return false;
        }

    }
}
