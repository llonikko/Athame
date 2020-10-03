using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Athame.Avalonia.Views
{
    public class PathFormatHelpWindow : Window
    {
        public PathFormatHelpWindow()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
