using Athame.Avalonia.Models;
using Athame.Avalonia.ViewModels;
using Athame.Avalonia.Views;
using Athame.Core;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using Splat;

namespace Athame.Avalonia
{
    public class App : Application
    {
        public override void Initialize()
            => AvaloniaXamlLoader.Load(this);

        public override void RegisterServices()
        {
            Locator.CurrentMutable.RegisterConstant(new AthameApp());
            Locator.CurrentMutable.Register<IViewFor<SettingsViewModel>>(() => new SettingsView());
            Locator.CurrentMutable.Register<IViewFor<AboutMeViewModel>>(() => new AboutMeView());

            base.RegisterServices();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var app = Locator.Current.GetService<AthameApp>();
            app.InitApp();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = new MainWindow()
                {
                    DataContext = new MainWindowViewModel()
                };

                var folderBrowser = new FolderBrowserDialog(mainWindow);

                Locator.CurrentMutable.RegisterConstant(folderBrowser);
                Locator.CurrentMutable.RegisterConstant<IScreen>(mainWindow.ViewModel);

                desktop.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}