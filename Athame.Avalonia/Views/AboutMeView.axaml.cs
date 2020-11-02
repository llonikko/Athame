using Athame.Avalonia.ViewModels;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive.Disposables;

namespace Athame.Avalonia.Views
{
    public class AboutMeView : ReactiveUserControl<AboutMeViewModel>
    {
        public Image AppLogoImage 
            => this.FindControl<Image>("AppLogoImage");
        public Button OKButton 
            => this.FindControl<Button>("OKButton");

        public AboutMeView()
        {
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.AppLogo, v => v.AppLogoImage.Source)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.OKCommand, v => v.OKButton)
                    .DisposeWith(disposables);
            });

            InitializeComponent();
        }

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
