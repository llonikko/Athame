using System;
using System.Collections.Generic;
using System.IO;
using Athame.Core.Plugin;
using Athame.Core.Settings;
using Athame.Plugin.Api;
using Serilog;

namespace Athame.Core
{
    public class AthameApp
    {
        private static readonly string ApplicationDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

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

        public void LoadAndInitPlugins()
        {
            var pluginManager = new PluginManager();
            pluginManager.LoadPlugins(PluginsFolder);
            pluginManager.InitPlugins(PluginDataFolder);

            Plugins.AddRange(pluginManager.Plugins);
            MediaServiceManager.Add(Plugins);
        }
    }
}
