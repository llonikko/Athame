using Athame.Avalonia.ViewModels;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace Athame.Avalonia.Views
{
    public class MediaItemsView : ReactiveUserControl<MediaItemsViewModel>
    {
        public MediaItemsView()
        {
            this.WhenActivated(disposables =>
            {
            });

            InitializeComponent();
        }

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
