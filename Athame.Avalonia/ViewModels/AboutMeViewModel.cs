using Athame.Avalonia.Resources;
using Avalonia.Media.Imaging;
using ReactiveUI;
using Splat;
using System.Reactive;

namespace Athame.Avalonia.ViewModels
{
    public class AboutMeViewModel : ViewModelBase, IRoutableViewModel
    {
        public Bitmap AppLogo { get; }
        public ReactiveCommand<Unit, Unit> OKCommand { get; }

        public string UrlPathSegment => "About Me";
        public IScreen HostScreen { get; }

        public AboutMeViewModel()
        {
            HostScreen = Locator.Current.GetService<IScreen>();

            AppLogo = Images.AppLogo;

            OKCommand = ReactiveCommand.CreateFromObservable(
                () => HostScreen.Router.NavigateBack.Execute());
        }
    }
}
