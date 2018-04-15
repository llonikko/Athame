using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Athame.Core.Logging;
using Athame.Core.Settings;
using Athame.PluginAPI;
using Athame.PluginAPI.Service;

namespace Athame.Core.Plugin
{

    public class PluginManager
    {
        private const string Tag = nameof(PluginManager);

        public const string PluginDir = "Plugins";
        public const string PluginDllPrefix = "AthamePlugin.";
        public const string SettingsDir = "Plugin Data";
        public const string SettingsFileFormat = "{0} Settings.json";

        public const int ApiVersion = 3;

        private static readonly AssemblyName PluginApiAssemblyName;

        static PluginManager()
        {
            PluginApiAssemblyName = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                let name = assembly.GetName()
                where name.Name == "Athame.PluginAPI"
                select name).FirstOrDefault();
        }

        public string PluginDirectory { get; }

        public string SettingsDirectory { get; }

        public PluginManager(string pluginDir, string settingsDir)
        {
            PluginDirectory = pluginDir;
            SettingsDirectory = Path.Combine(settingsDir, SettingsDir);
            Directory.CreateDirectory(pluginDir);
            Directory.CreateDirectory(SettingsDirectory);
            // !! NOTE !! This will be invoked if an assembly, for whatever reason, is loaded during
            // the plugin load process.
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                var name = new AssemblyName(args.Name);

                // I am so sorry
                var pluginDirectory = (from plugin in Plugins
                                       where plugin.Assembly == args.RequestingAssembly || 
                                       (from referencedAssembly in plugin.Assembly.GetReferencedAssemblies()
                                        where referencedAssembly.FullName == args.RequestingAssembly.GetName().ToString()
                                        select true).Any()
                                       select plugin.AssemblyDirectory).FirstOrDefault();

                if (pluginDirectory == null) return null;

                // Parse name, get parent directory from requesting assembly's location, then
                // build a path.
                var referencedDllPath = Path.Combine(pluginDirectory, name.Name + ".dll");

                // Load the assembly no matter what.
                return Assembly.LoadFile(referencedDllPath);
            };
        }

        public List<PluginInstance> Plugins { get; private set; }

        public bool AreAnyLoaded { get; set; }

        public event EventHandler<PluginLoadExceptionEventArgs> LoadException;

        private Assembly[] loadedAssemblies;

        private bool IsAlreadyLoaded(AssemblyName assemblyName)
        {
            var result = loadedAssemblies.Any(assembly => assembly.GetName() == assemblyName);
            if (result)
            {
                Log.Warning(Tag, $"Attempted to load {assemblyName} again!");
            }
            return result;
        }

        private void Activate(PluginInstance instance)
        {
            Log.Debug(Tag, $"Attempting to load {instance.Name}");
            if (IsAlreadyLoaded(instance.Assembly.GetName())) return;
            // Check that it references some form of the Plugin API assembly
            AssemblyName pluginApiName;
            if (
            (pluginApiName =
                instance.Assembly.GetReferencedAssemblies()
                    .FirstOrDefault(name => name.Name == PluginApiAssemblyName.Name)) != null)
            {
                if (pluginApiName.Version.Major != PluginApiAssemblyName.Version.Major)
                {
                    throw new PluginIncompatibleException($"Wrong major version of Athame.PluginAPI referenced: expected {PluginApiAssemblyName}, found {pluginApiName}");
                }
            }
            else
            {
                throw new PluginLoadException("Plugin does not reference Athame.PluginAPI.", instance.AssemblyDirectory);
            }

            var types = instance.Assembly.GetExportedTypes();
            // Only filter for types which can be instantiated and implement IPlugin somehow.
            var implementingType = types.FirstOrDefault(
                type =>
                    !type.IsInterface &&
                    !type.IsAbstract &&
                    type.GetInterface(nameof(IPlugin)) != null);
            if (implementingType == null)
            {
                throw new PluginLoadException("No exported types found implementing IPlugin.",
                    instance.AssemblyDirectory);
            }
            // Activate base plugin
            var plugin = (IPlugin)Activator.CreateInstance(implementingType);
            if (plugin.ApiVersion != ApiVersion)
            {
                throw new PluginIncompatibleException($"Plugin declares incompatible API version: expected {ApiVersion}, found {plugin.ApiVersion}.");
            }
            instance.Info = plugin.Info;
            instance.Plugin = plugin;

            var servicePlugin = plugin as MusicService;
            var context = new PluginContext
            {
                PluginDirectory = instance.AssemblyDirectory
            };
            instance.Context = context;

            if (servicePlugin != null)
            {
                var settingsPath = Path.Combine(SettingsDirectory, String.Format(SettingsFileFormat, plugin.Info.Name));
                var settingsFile = new SettingsFile(settingsPath, servicePlugin.Settings.GetType(),
                    servicePlugin.Settings);
                instance.SettingsFile = settingsFile;
            }
            else
            {
                throw new PluginLoadException("IPlugin type does not implement MusicService.", instance.AssemblyDirectory);
            }


        }

        private void BeforeLoad()
        {
            if (Plugins != null)
            {
                throw new InvalidOperationException("Plugins can only be loaded once");
            }
            Plugins = new List<PluginInstance>();
            // Cache current AppDomain loaded assemblies
            loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        }

        public void LoadAll()
        {
            BeforeLoad();

            // Plugins are stored in format {PluginDir}/{PluginName}/AthamePlugin.*.dll
            var subDirs = Directory.GetDirectories(PluginDirectory);
            foreach (var dir in subDirs)
            {
                var name = Path.GetFileName(dir);
                try
                {
                    // Attempt to load a .pdb if one exists
                    var basePath = Path.Combine(dir, PluginDllPrefix + name);
                    var dllFilename = basePath + ".dll";
                    var pdbFilename = basePath + ".pdb";

                    var theAssembly = File.Exists(pdbFilename)
                        ? Assembly.Load(File.ReadAllBytes(dllFilename), File.ReadAllBytes(pdbFilename))
                        : Assembly.Load(File.ReadAllBytes(dllFilename));

                    // Set basic information about the assembly
                    var plugin = new PluginInstance
                    {
                        Assembly = theAssembly,
                        AssemblyDirectory = dir,
                        Name = name
                    };
                    Plugins.Add(plugin);

                    Activate(plugin);
                    AreAnyLoaded = true;
                }
                catch (Exception ex)
                {
                    Log.WriteException(Level.Error, Tag, ex, $"While loading plugin {name}");
                    var eventArgs = new PluginLoadExceptionEventArgs { PluginName = name, Exception = ex, Continue = true };
                    LoadException?.Invoke(this, eventArgs);
                    if (!eventArgs.Continue) return;
                }

            }
        }

        public void InitAll(AthameApplication application)
        {
            Log.Debug(Tag, "Init plugin settings");
            if (Plugins == null)
            {
                throw new InvalidOperationException("InitAll can only be called after LoadAll");
            }
            foreach (var plugin in Plugins)
            {
                // If it's a service plugin, add it to main service collection
                var service = plugin.Service;
                if (service == null) continue;

                // Restore the config
                plugin.SettingsFile.Load();
                plugin.Service.Settings = plugin.SettingsFile.Settings;

                // Call Init
                plugin.Plugin.Init(application, plugin.Context);

                AddService(plugin.Service);
            }
        }

        private static readonly Regex FilenameRegex = new Regex(@"AthamePlugin\.(?:.*)\.dll");

        private readonly HashSet<MusicService> services = new HashSet<MusicService>();
        private readonly Dictionary<Uri, MusicService> servicesByUris = new Dictionary<Uri, MusicService>();

        private void AddService(MusicService service)
        {
            services.Add(service);
            foreach (var uri in service.BaseUri)
            {
                servicesByUris.Add(uri, service);
            }
        }

        public MusicService GetService(string name)
        {
            return (from service in services
                    where service.Info.Name == name
                    select service).FirstOrDefault();
        }

        public MusicService GetServiceByBaseUri(Uri baseUri)
        {
            return (from s in servicesByUris
                    where s.Key.Scheme == baseUri.Scheme && s.Key.Host == baseUri.Host
                    select s.Value).FirstOrDefault();
        }

        public IEnumerable<MusicService> ServicesEnumerable()
        {
            return services.AsEnumerable();
        }

    }
}
