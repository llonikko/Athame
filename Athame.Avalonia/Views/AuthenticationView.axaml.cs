using Athame.Avalonia.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AvaloniaProgressRing;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Athame.Avalonia.Views
{
    public class AuthenticationView : ReactiveUserControl<AuthenticationViewModel>
    {
        public TextBlock HelpTextBlock => this.FindControl<TextBlock>("HelpTextBlock");
        public TextBlock ErrorTextBlock => this.FindControl<TextBlock>("ErrorTextBlock");

        public TextBox UsernameTextBox => this.FindControl<TextBox>("UsernameTextBox");
        public TextBox PasswordTextBox => this.FindControl<TextBox>("PasswordTextBox");

        public Button LoginButton => this.FindControl<Button>("LoginButton");

        public ProgressRing AuthenticationProgressRing => this.FindControl<ProgressRing>("AuthenticationProgressRing");

        public AuthenticationView()
        {
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.HasError, v => v.ErrorTextBlock.IsVisible)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.ErrorMessage, v => v.ErrorTextBlock.Text)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.HelpMessage, v => v.HelpTextBlock.Text)
                    .DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.Username, v => v.UsernameTextBox.Text)
                    .DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.Password, v => v.PasswordTextBox.Text)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.LoginCommand, v => v.LoginButton)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.IsAuthenticating, v => v.AuthenticationProgressRing.IsActive)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.IsAuthenticating, v => v.LoginButton.IsVisible, isAuthenticating => !isAuthenticating)
                    .DisposeWith(disposables);
            });

            InitializeComponent();
        }

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
