using Athame.Avalonia.ViewModels;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;

namespace Athame.Avalonia.Views
{
    public class ServiceRestoreWindow : ReactiveWindow<ServiceRestoreWindowViewModel>
    {
        public static readonly ServiceRestoreWindow Null;

        public ServiceRestoreWindow()
        {
            this.WhenActivated(disposables =>
            {
            });

            Observable
                .FromEventPattern(this, nameof(Opened))
                .Select(e => Unit.Default)
                .InvokeCommand(this, x => x.ViewModel.RestoreCommand);

            InitializeComponent();
        }

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}