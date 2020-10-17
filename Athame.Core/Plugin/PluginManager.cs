using Athame.Plugin.Api;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Athame.Core.Plugin
{
    public class PluginManager
    {
        public const string PluginPrefix = "Athame.Plugin";
        
        private Assembly[] loadedAssemblies;
        private static readonly AssemblyName PluginApi;
        static PluginManager()
        {
            PluginApi =
                (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                 let name = assembly.GetName()
                 where name.Name == "Athame.Plugin.Api"
                 select name)
                .FirstOrDefault();
        }

        public ICollection<IPlugin> Plugins { get; }
        public event EventHandler<PluginLoadExceptionEventArgs> PluginLoadException;

        public PluginManager()
            => Plugins = new List<IPlugin>();

        public void LoadPlugins(string pluginsPath)
        {
            if (Plugins.Count > 0)
            {
                throw new InvalidOperationException("Plugins can only be loaded once");
            }
            // Cache current AppDomain loaded assemblies
            loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Plugins are stored in format {PluginDirectory}/{PluginName}/Athame.Plugin.*.dll
            var dirs = Directory.GetDirectories(pluginsPath);
            foreach (var pluginDirectory in dirs)
            {
                LoadPlugin(pluginDirectory);
            }
        }

        public void LoadPlugin(string pluginDirectory)
        {
            var pluginName = Path.GetFileName(pluginDirectory);
            Log.Debug("Attempting to load {Plugin}", pluginName);

            try
            {
                var location = Path.Combine(pluginDirectory, $"{PluginPrefix}.{pluginName}.dll");
                var assemblyName = new AssemblyName(Path.GetFileNameWithoutExtension(location));

                var isLoaded = loadedAssemblies.Any(assembly => assembly.GetName() == assemblyName);
                if (isLoaded)
                {
                    Log.Warning("Attempted to load {AssemblyName} again!", assemblyName);
                    return;
                }

                Log.Debug("Loading plugin: {Location}", location);
                var loadContext = new PluginLoadContext(location);
                var assembly = loadContext.LoadFromAssemblyName(assemblyName);

                AssemblyName apiName = assembly
                    .GetReferencedAssemblies()
                    .FirstOrDefault(a => a.Name == PluginApi.Name);
                if (apiName == default(AssemblyName))
                {
                    throw new PluginLoadException("Plugin does not reference Athame.Plugin.Api.", assembly.Location);
                }
                if (apiName.Version.Major != PluginApi.Version.Major)
                {
                    throw new PluginIncompatibleException($"Wrong major version of Athame.Plugin.Api referenced: expected {PluginApi}, found {apiName}");
                }

                // Only filter for types which can be instantiated and implement IPlugin somehow.
                var type = assembly
                    .GetExportedTypes()
                    .FirstOrDefault(t => typeof(IPlugin)
                    .IsAssignableFrom(t));
                if (type == default)
                {
                    throw new PluginLoadException("No exported types found implementing IPlugin.", assembly.Location);
                }

                // Activate base plugin
                var plugin = Activator.CreateInstance(type) as IPlugin;
                if (plugin.Version != PluginApi.Version.Major)
                {
                    throw new PluginIncompatibleException($"Plugin declares incompatible API version: expected {PluginApi.Version.Major}, found {plugin.Version}.");
                }

                Plugins.Add(plugin);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "While loading plugin {Plugin}", pluginName);
                PluginLoadException?.Invoke(this, new PluginLoadExceptionEventArgs
                {
                    PluginName = pluginName,
                    Exception = ex,
                    Continue = true
                });
            }
        }

        public void InitPlugins(string settingsPath)
        {
            Log.Debug("Init plugins");
            foreach (var p in Plugins)
            {
                p.Init(settingsPath);
            }
        }

        private static readonly Regex FilenameRegex = new Regex(@"Athame.Plugin\.(?:.*)\.dll");
    }
}
