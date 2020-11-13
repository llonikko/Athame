using Athame.Core;
using Athame.Core.Plugin;
using Athame.Plugin.Api.Interface;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog;
using Splat;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Athame.Avalonia.ViewModels
{
    public class AuthenticationViewModel : ViewModelBase
    {
        private readonly IMediaService service;

        [Reactive]
        public string Username { get; set; }
        [Reactive]
        public string Password { get; set; }

        public bool IsAuthenticating { [ObservableAsProperty]get; }
        public bool HasError { [ObservableAsProperty]get; }
        public bool IsAuthenticated { [ObservableAsProperty]get; }

        public string Title { get; }
        public string HelpMessage { get; }
        public string ErrorMessage { get; }

        public ReactiveCommand<Unit, bool> LoginCommand { get; }

        public AuthenticationViewModel(IMediaService service)
        {
            this.service = service;

            Title = $"{service.Name} sign in";
            HelpMessage = service.AsUsernamePasswordAuthenticatable().SignInHelpText
                ?? $"{service.Name} has not provided any help text.";
            ErrorMessage = "An error occurred while signing in. Please check your credentials and try again.";

            var canAuthenticate = this
                .WhenAnyValue(
                    x => x.Username, 
                    x => x.Password, 
                    (username, password) => 
                        !string.IsNullOrEmpty(username) && 
                        !string.IsNullOrEmpty(password));
            LoginCommand = ReactiveCommand.CreateFromTask(
                Authenticate,
                canAuthenticate);
            LoginCommand.IsExecuting
                .ToPropertyEx(this, x => x.IsAuthenticating);
            LoginCommand
                .Select(success => !success)
                .ToPropertyEx(this, x => x.HasError);

            this.WhenAnyObservable(x => x.LoginCommand)
                .ToPropertyEx(this, x => x.IsAuthenticated);
        }

        public void Clear()
            => Username = Password = string.Empty;

        private async Task<bool> Authenticate()
        {
            var auth = Locator.Current.GetService<AthameApp>().AuthenticationManager;

            var result = await auth.Authenticate(service, Username, Password, true);
            if (result.IsAuthenticated)
            {
                return true;
            }
            if (result.Exception != null)
            {
                Log.Error(result.Exception, "Authentication error");
            }
            return false;
        }
    }
}
