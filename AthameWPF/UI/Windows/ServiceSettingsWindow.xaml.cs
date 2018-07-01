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
using System.Windows.Shapes;
using Athame.Core.Plugin;

namespace AthameWPF.UI.Windows
{
    /// <summary>
    /// Interaction logic for ServiceSettingsWindow.xaml
    /// </summary>
    public partial class ServiceSettingsWindow : Window
    {
        private PluginInstance instance;

        public ServiceSettingsWindow()
        {
            InitializeComponent();
        }

        public ServiceSettingsWindow(PluginInstance instance)
        {
            InitializeComponent();
            this.instance = instance;
            Title = $"{instance.Info.Name} settings";
            FormsHost.Child = instance.Service.GetSettingsControl();
        }
    }
}
