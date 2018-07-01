using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AthameWPF.UI.Controls
{
    /// <summary>
    /// Interaction logic for PathToggleButton.xaml
    /// </summary>
    public sealed partial class PathButton
    {
        public PathButton()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(PathButton), new PropertyMetadata(default(string), TextChanged));

        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (PathButton) d;
            var newValue = (string) e.NewValue;
            var c = (TextBlock)self.FindName("ButtonText");
            if (c == null) return;
            c.Visibility = string.IsNullOrEmpty(newValue) ? Visibility.Collapsed : Visibility.Visible;
        }

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty PathDataProperty = DependencyProperty.Register(
            "PathData", typeof(Geometry), typeof(PathButton), new PropertyMetadata(default(Geometry)));

        public Geometry PathData
        {
            get { return (Geometry) GetValue(PathDataProperty); }
            set { SetValue(PathDataProperty, value); }
        }

        public static readonly DependencyProperty PathRenderSizeProperty = DependencyProperty.Register(
            "PathRenderSize", typeof(double), typeof(PathButton), new PropertyMetadata(36.0));

        public double PathRenderSize
        {
            get { return (double) GetValue(PathRenderSizeProperty); }
            set { SetValue(PathRenderSizeProperty, value); }
        }

        public static readonly DependencyProperty TextPathPositioningProperty = DependencyProperty.Register(
            "TextPathPositioning", typeof(Orientation), typeof(PathButton), new PropertyMetadata(Orientation.Horizontal));

        public Orientation TextPathPositioning
        {
            get { return (Orientation) GetValue(TextPathPositioningProperty); }
            set { SetValue(TextPathPositioningProperty, value); }
        }
    }
}
