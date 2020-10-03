using Athame.Plugin.Api;
using Athame.Plugin.Api.Service;
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

        public List<IPlugin> Plugins { get; protected set; }
        public event EventHandler<PluginLoadExceptionEventArgs> PluginLoadException;

        public void LoadAll(string pluginsPath)
        {
            BeforeLoad();
            // Plugins are stored in format {PluginDirectory}/{PluginName}/Athame.Plugin.*.dll
            var dirs = Directory.GetDirectories(pluginsPath);
            foreach (var pluginDirectory in dirs)
            {
                var pluginName = Path.GetFileName(pluginDirectory);

                Log.Debug("Attempting to load {Plugin}", pluginName);
                try
                {
                    var location = Path.Combine(pluginDirectory, $"{PluginPrefix}.{pluginName}.dll");
                    var assembly = LoadPlugin(location);

                    ThrowIfPluginIncompatibleOrNotFound(assembly);

                    var plugin = Activate(assembly);
                    Plugins.Add(plugin);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "While loading plugin {Plugin}", pluginName);
                    var e = new PluginLoadExceptionEventArgs
                    {
                        PluginName = pluginName,
                        Exception = ex,
                        Continue = true
                    };
                    PluginLoadException?.Invoke(this, e);
                    if (!e.Continue)
                    {
                        return;
                    }
                }
            }
        }

        private void BeforeLoad()
        {
            if (Plugins != null)
            {
                throw new InvalidOperationException("Plugins can only be loaded once");
            }

            Plugins = new List<IPlugin>();
            // Cache current AppDomain loaded assemblies
            loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        }

        private Assembly LoadPlugin(string path)
        {
            Log.Debug("Loading plugin: {Path}", path);
            PluginLoadContext loadContext = new PluginLoadContext(path);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(path)));
        }

        private bool IsLoaded(AssemblyName assemblyName)
        {
            var isLoaded = loadedAssemblies.Any(assembly => assembly.GetName() == assemblyName);
            if (isLoaded)
            {
                Log.Warning("Attempted to load {AssemblyName} again!", assemblyName);
            }
            return isLoaded;
        }

        private void ThrowIfPluginIncompatibleOrNotFound(Assembly assembly)
        {
            // Check that it references some form of the Plugin API assembly
            AssemblyName apiName = assembly
                .GetReferencedAssemblies()
                .FirstOrDefault(a => a.Name == PluginApi.Name);

            if (apiName == default(AssemblyName))
            {
                throw new PluginLoadException("Plugin does not reference Athame.Plugin.API.", assembly.Location);
            }
            if (apiName.Version.Major != PluginApi.Version.Major)
            {
                throw new PluginIncompatibleException($"Wrong major version of Athame.Plugin.API referenced: expected {PluginApi}, found {apiName}");
            }
        }

        private IPlugin Activate(Assembly assembly)
        {
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
            return plugin;
        }

        public void InitAll(string settingsPath)
        {
            Log.Debug("Init plugins");
            if (Plugins == null)
            {
                throw new InvalidOperationException("InitAll can only be called after LoadAll");
            }

            foreach (var p in Plugins)
            {
                p.Init(settingsPath);   
                AddService(p.Service);
            }
        }

        private static readonly Regex FilenameRegex = new Regex(@"Athame.Plugin\.(?:.*)\.dll");
        private readonly Dictionary<Uri, IMediaService> servicesByUris = new Dictionary<Uri, IMediaService>();

        private void AddService(IMediaService service)
        {
            foreach (var uri in service.BaseUri)
            {
                servicesByUris.Add(uri, service);
            }
        }

        public IMediaService GetService(string name)
            => (from plugin in Plugins where plugin.Name == name select plugin.Service)
            .FirstOrDefault();

        public IMediaService GetService(Uri baseUri)
            => (from s in servicesByUris where s.Key.Scheme == baseUri.Scheme && s.Key.Host == baseUri.Host select s.Value)
            .FirstOrDefault();

        public IEnumerable<IMediaService> PluginServices
            => (from plugin in Plugins select plugin.Service)
            .ToArray();
    }
}
