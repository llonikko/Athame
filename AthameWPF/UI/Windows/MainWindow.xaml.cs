using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Athame.PluginAPI;
using AthameWPF.UI.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace AthameWPF.UI.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double ScreenSizeFillPercent = 0.6;

        private List<Exception> pluginLoadExceptions = new List<Exception>();

        public MainWindow()
        {
            InitializeComponent();
            var workArea = SystemParameters.WorkArea;
            Width = (workArea.Width * ScreenSizeFillPercent);
            Height = (workArea.Height * ScreenSizeFillPercent);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
//            var pluginManager = App.Context.PluginManager;
//            var handle = new WindowInteropHelper(this).Handle;
//            pluginManager.LoadAll();
//            pluginManager.InitAll(new AthameApplication());
//            if (pluginLoadExceptions.Count > 0)
//            {
//                TaskDialogHelper.ShowMessage("Plugin load error",
//                    "One or more errors occurred while loading plugins. Some plugins may be unavailable. Check the log for more details.",
//                    TaskDialogStandardButtons.Ok, TaskDialogStandardIcon.Warning, handle);
//                pluginLoadExceptions.Clear();
//            }
//            if (!pluginManager.AreAnyLoaded)
//            {
//#if DEBUG
//                var buttons = TaskDialogStandardButtons.Ok | TaskDialogStandardButtons.Cancel;
//#else
//                var buttons = TaskDialogStandardButtons.Ok;
//#endif
//                if (TaskDialogHelper.ShowMessage("No plugins installed",
//                    "No plugins could be found. If you have attempted to install a plugin, it may not be installed properly.",
//                    buttons, TaskDialogStandardIcon.Error, handle) != TaskDialogResult.Cancel)
//                {
//                    Application.Current.Shutdown();
//                }
//
//            }
        }
    }
}