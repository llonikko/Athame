using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Athame.Avalonia.Views
{
    public class PathFormatHelpWindow : Window
    {
        public PathFormatHelpWindow()
            => InitializeComponent();

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
