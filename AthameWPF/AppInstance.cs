using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Athame.Core.Logging;
using Athame.Core.Plugin;
using Athame.PluginAPI;
using AthameWPF.Settings;

namespace AthameWPF
{
    public class AppInstance : AthameApplication
    {
        private const string Tag = nameof(AppInstance);

        private const string SettingsFilename = "Athame Settings.json";
        public static string LogDir;

        public PluginManager PluginManager { get; private set; }

        public AppInstance()
        {
            IsWindowed = true;
#if DEBUG
            UserDataPath = Path.Combine(Directory.GetCurrentDirectory(), "UserDataDebug");
#else
                UserDataPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Athame");
#endif
            
        }

        private SettingsFile<AthameSettings> settingsFile;
        public AthameSettings Settings => settingsFile.Settings;

        private void InitLogging()
        {
            LogDir = UserDataPathOf("Logs");
            Directory.CreateDirectory(LogDir);
            Log.AddLogger("file", new FileLogger(LogDir));
#if !DEBUG
            Log.Filter = Level.Warning;
            AppDomain.CurrentDomain.UnhandledException += AppDomainHandler;
#endif
            Log.Debug(Tag, "Logging installed on AppDomain");
        }

        private void AppDomainHandler(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            Log.WriteException(Level.Fatal, "AppDomain", unhandledExceptionEventArgs.ExceptionObject as Exception);
        }

        public void Begin()
        {
            Directory.CreateDirectory(UserDataPath);
            InitLogging();
            var settingsPath = UserDataPathOf(SettingsFilename);
            settingsFile = new SettingsFile<AthameSettings>(settingsPath);
            PluginManager = new PluginManager(Path.Combine(Directory.GetCurrentDirectory(), PluginManager.PluginDir), UserDataPath);
            settingsFile.Load();
            Log.Debug(Tag, "Necessary setup is done");
        }

        public void SaveSettings()
        {
            settingsFile.Save();
        }

        public void End()
        {
            Log.Debug(Tag, "Ending now...");
            settingsFile.Save();
        }
    }
}
