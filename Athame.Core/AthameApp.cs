using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Athame.Core.Plugin;
using Athame.Core.Settings;
using Athame.Plugin.Api;
using Athame.Plugin.Api.Interface;
using Serilog;

namespace Athame.Core
{
    public class AthameApp
    {
        private static readonly string ApplicationDataFolderPath 
            = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        private static readonly Dictionary<Uri, IMediaService> services
            = new Dictionary<Uri, IMediaService>();

        public string AppDataFolder
            => Path.GetFullPath(Path.Combine(ApplicationDataFolderPath, "Athame.Avalonia"));
        public string AppLogsFolder
            => Path.Combine(AppDataFolder, "Logs");
        public string PluginsFolder
            => Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
        public string PluginDataFolder
            => Path.Combine(AppDataFolder, "Plugin Data");
        public string AppSettingsPath
            => Path.Combine(AppDataFolder, "Athame Settings.json");

        public AthameSettings AppSettings { get; set; }
        public AuthenticationManager AuthenticationManager { get; }
        public List<IPlugin> Plugins { get; }

        public AthameApp()
        {
            Directory.CreateDirectory(AppDataFolder);
            Directory.CreateDirectory(AppLogsFolder);
            //if (!Directory.Exists(PluginsFolder))
            //{
            //    Directory.CreateDirectory(PluginsFolder);
            //}
            Directory.CreateDirectory(PluginDataFolder);

            Plugins = new List<IPlugin>();
            AuthenticationManager = new AuthenticationManager();
        }

        public void InitApp()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine(AppLogsFolder, "log.txt"),
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true)
                .CreateLogger();

            AppSettings = JsonFileSettings.Load<AthameSettings>(AppSettingsPath);
        }

        public void LoadPlugins()
        {
            // Plugins are stored in format {PluginDirectory}/{PluginName}/Athame.Plugin.*.dll
            foreach (var dir in Directory.GetDirectories(PluginsFolder))
            {
                if (PluginLoader.Load(dir) is IPlugin p)
                {
                    Plugins.Add(p);
                    p.Init(PluginDataFolder);
                    AddService(p.Service);
                }
            }
        }

        public void UpdateSettings(AthameSettings settings)
        {
            AppSettings = settings;
            AppSettings.Save();
        }

        public static void AddService(IMediaService service)
        {
            foreach (var uri in service.BaseUri)
            {
                services.Add(uri, service);
            }
        }

        public static IMediaService GetService(Uri baseUri)
            => (from s in services where s.Key.Scheme == baseUri.Scheme && s.Key.Host == baseUri.Host select s.Value)
            .FirstOrDefault();

        public static IEnumerable<IMediaService> Services
            => (from s in services select s.Value)
            .Distinct();
    }
}
