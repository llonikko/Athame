using System;
using System.Collections.Generic;
using System.Windows;
using Athame.Core.Plugin;
using AthameWPF.Settings;
using AthameWPF.UI.Windows;

namespace AthameWPF.UI.ViewModels
{
    public class SettingsViewModel
    {
        public Window OwnerWindow { get; set; }

        public AthameSettings Settings { get; set; }

        public List<PluginInstance> PluginInstances => App.Context.PluginManager.Plugins;

        public ViewModelCommand ShowSettings { get; set; }

        public SettingsViewModel()
        {
            ShowSettings = new ViewModelCommand(ShowSettings_Execute);
        }

        private void ShowSettings_Execute(object o)
        {

            var instance = (PluginInstance) o;
            var serviceSettingsWindow = new ServiceSettingsWindow(instance);
            serviceSettingsWindow.ShowDialog();
        }
    }
}