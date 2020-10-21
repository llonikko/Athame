using Athame.Plugin.Api;
using System;

namespace Athame.Avalonia.ViewModels
{
    public class PluginDetailsViewModel : ViewModelBase
    {
        public string PluginName { get; }
        public string PluginDescription { get; }
        public string PluginAuthor { get; }
        public Uri PluginWebsite { get; }
        public int PluginVersion { get; }

        public PluginDetailsViewModel(IPlugin plugin)
        {
            PluginName = plugin.Name;
            PluginDescription = plugin.Description;
            PluginAuthor = plugin.Author;
            PluginWebsite = plugin.Website;
            PluginVersion = plugin.Version;
        }
    }
}
