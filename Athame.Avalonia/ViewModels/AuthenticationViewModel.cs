using Athame.Core;
using Athame.Core.Plugin;
using Athame.Plugin.Api.Service;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog;
using Splat;
using System;
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
        public string HelpText { get; }
        public string ErrorText { get; }

        public ReactiveCommand<Unit, bool> LoginCommand { get; }

        public AuthenticationViewModel(IMediaService service)
        {
            this.service = service;

            Title = $"{service.Name} sign in";

            HelpText = service.AsUsernamePasswordAuthenticatable().SignInHelpText
                ?? $"{service.Name} has not provided any help text.";

            ErrorText = "An error occurred while signing in. Please check your credentials and try again.";

            LoginCommand = ReactiveCommand.CreateFromTask(
                Authenticate,
                CanAuthenticate());

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

        private IObservable<bool> CanAuthenticate()
           => this.WhenAnyValue(
               x => x.Username,
               x => x.Password,
               (username, password) =>
                   !string.IsNullOrEmpty(username) &&
                   !string.IsNullOrEmpty(password));

        private async Task<bool> Authenticate()
        {
            var auth = Locator.Current.GetService<AthameApp>().AuthenticationManager;

            var result = await auth.Authenticate(service, Username, Password, true);
            if (result.IsSuccess)
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
