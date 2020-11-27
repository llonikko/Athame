using Athame.Plugin.Api;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Athame.Core.Plugin
{
    public static class PluginLoader
    {
        public static readonly string PluginNamePrefix
            = "Athame.Plugin";

        public static readonly AssemblyName PluginApi
            = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
               let name = assembly.GetName()
               where name.Name == "Athame.Plugin.API"
               select name)
            .FirstOrDefault();

        //public event EventHandler<PluginLoadExceptionEventArgs> PluginLoadException;

        public static IPlugin Load(string pluginDirectory)
        {
            var name = Path.GetFileName(pluginDirectory);
            var path = Path.Combine(pluginDirectory, $"{PluginNamePrefix}.{name}.dll");
            var assemblyName = new AssemblyName(Path.GetFileNameWithoutExtension(path));

            if (IsLoaded(assemblyName))
            {
                Log.Warning("Attempted to load {AssemblyName} again!", assemblyName.Name);
                return null;
            }

            Log.Information("Loading plugin: {PluginName} - {Path}", name, path);
            try
            {
                var loadContext = new PluginLoadContext(path);
                return LoadPluginFromAssembly(loadContext.LoadFromAssemblyName(assemblyName));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "While loading plugin {Plugin}", assemblyName.Name);
                //PluginLoadException?.Invoke(this, new PluginLoadExceptionEventArgs
                //{
                //    PluginName = pluginName,
                //    Exception = ex
                //});
            }
            return null;
        }

        public static IPlugin LoadPluginFromAssembly(Assembly assembly)
        {
            AssemblyName apiName = assembly
                    .GetReferencedAssemblies()
                    .FirstOrDefault(a => a.Name == PluginApi.Name);
            if (apiName == default(AssemblyName))
            {
                throw new PluginLoadException(
                    "Plugin does not reference Athame.Plugin.API.", assembly.Location);
            }
            if (apiName.Version.Major != PluginApi.Version.Major)
            {
                throw new PluginIncompatibleException(
                    $"Wrong major version of Athame.Plugin.API referenced: expected {PluginApi}, found {apiName}");
            }

            // Only filter for types which can be instantiated and implement IPlugin somehow.
            var type = assembly
                .GetExportedTypes()
                .FirstOrDefault(t => typeof(IPlugin)
                .IsAssignableFrom(t));
            if (type == default)
            {
                throw new PluginLoadException(
                    "No exported types found implementing IPlugin.", assembly.Location);
            }

            // Activate base plugin
            var plugin = Activator.CreateInstance(type) as IPlugin;
            if (plugin.Version != PluginApi.Version.Major)
            {
                throw new PluginIncompatibleException(
                    $"Plugin declares incompatible API version: expected {PluginApi.Version.Major}, found {plugin.Version}.");
            }
            return plugin;
        }

        private static bool IsLoaded(AssemblyName assemblyName)
            => AppDomain.CurrentDomain.GetAssemblies().Any(a => a.GetName() == assemblyName);
    }
}
