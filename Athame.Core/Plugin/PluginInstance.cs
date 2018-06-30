using System;
using System.Reflection;
using Athame.Core.Settings;
using Athame.PluginAPI;
using Athame.PluginAPI.Service;

namespace Athame.Core.Plugin
{
    public class PluginInstance
    {
        private IPlugin plugin;
        public PluginInfo Info { get; set; }

        public IPlugin Plugin
        {
            get { return plugin; }
            set
            {
                PluginType = value.GetType();
                plugin = value;
            }
        }

        public PluginContext Context { get; set; }
        public Assembly Assembly { get; set; }
        public string AssemblyDirectory { get; set; }
        public string Name { get; set; }
        public Version AssemblyFileVersion { get; set; }
        public SettingsFile SettingsFile { get; set; }
        public MusicService Service => Plugin as MusicService;
        public Type PluginType { get; private set; }

    }
}
