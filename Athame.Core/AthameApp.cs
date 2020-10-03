using System;
using System.Collections.Generic;
using System.IO;
using Athame.Core.Plugin;
using Athame.Core.Search;
using Athame.Core.Settings;
using Athame.Plugin.Api;
using Athame.Plugin.Api.Service;
using Serilog;

namespace Athame.Core
{
    public class AthameApp
    {
        private readonly PluginManager pluginManager;

        private static readonly string ApplicationDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        
        public bool IsWindowed { get; }

        public string AthameAppDataPath
            => Path.GetFullPath(Path.Combine(ApplicationDataFolderPath, "Athame.Avalonia"));

        public string AthameAppLogsPath
            => Path.Combine(AthameAppDataPath, "Logs");

        public string AthameAppSettingsPath
            => Path.Combine(AthameAppDataPath, "Athame Settings.json");

        public string AthamePluginsPath
            => Path.Combine(Directory.GetCurrentDirectory(), "Plugins");

        public string AthamePluginsSettingsPath
            => Path.Combine(AthameAppDataPath, "Plugin Data");

        public ISettings<AthameSettings> AppSettings { get; }

        public AuthenticationManager AuthenticationManager { get; }

        public UrlResolver UrlResolver { get; }

        public List<IPlugin> Plugins 
            => pluginManager.Plugins;

        public IEnumerable<IMediaService> PluginServices 
            => pluginManager.PluginServices;

        public AthameApp()
        {
            Directory.CreateDirectory(AthameAppDataPath);
            Directory.CreateDirectory(AthameAppLogsPath);
            //Directory.CreateDirectory(PluginsPath);
            Directory.CreateDirectory(AthamePluginsSettingsPath);

            IsWindowed = true;

            pluginManager = new PluginManager();

            AuthenticationManager = new AuthenticationManager();
            UrlResolver = new UrlResolver(pluginManager, AuthenticationManager);

            AppSettings = new Settings<AthameSettings>(AthameAppSettingsPath);
        }

        public void InitApp()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine(AthameAppLogsPath, "log.txt"),
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true)
                .CreateLogger();

            AppSettings.Load();
        }

        public void LoadAndInitPlugins()
        {
            pluginManager.LoadAll(AthamePluginsPath);
            pluginManager.InitAll(AthamePluginsSettingsPath);
        }
    }
}
