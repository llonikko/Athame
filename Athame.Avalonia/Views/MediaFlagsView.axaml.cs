using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Athame.Avalonia.Views
{
    public class MediaFlagsView : UserControl
    {
        public MediaFlagsView()
            => InitializeComponent();

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
