using System;
using System.IO;
using Athame.Core.Logging;
using Newtonsoft.Json;

namespace Athame.Core.Settings
{

    public class SettingsFile
    {
        public const string Tag = "SettingsFile";

        private readonly string settingsPath;
        private readonly Type settingsType;
        private readonly object defaultSettings;
        private readonly JsonSerializerSettings SerializerSettings;

        public object Settings { get; set; }

        public SettingsFile(string settingsPath, Type settingsType, object defaultSettings)
        {
            Log.Debug(Tag, "Init settings file");
            this.settingsPath = settingsPath;
            this.settingsType = settingsType;
            this.defaultSettings = defaultSettings;
            SerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        public bool Exists()
        {
            return File.Exists(settingsPath);
        }

        public void Load()
        {
            Log.Debug(Tag, "Load config");
            if (!File.Exists(settingsPath))
            {
                Log.Info(Tag, $"Create new config in {settingsPath}");
                Settings = defaultSettings;
                Save();
            }
            else
            {
                try
                {
                    // Assign settings path to deserialised settings instance
                    Settings = JsonConvert.DeserializeObject(File.ReadAllText(settingsPath), settingsType,
                        SerializerSettings);
                }
                catch (JsonSerializationException ex)
                {
                    Log.WriteException(Level.Warning, Tag, ex, "Config could not be parsed, creating new");
#if DEBUG
                    throw;
#endif
                    Settings = defaultSettings;
                    Save();
                }
            }
        }

        public void Save()
        {
            Log.Debug(Tag, $"Saving config to {settingsPath}");
            File.WriteAllText(settingsPath, JsonConvert.SerializeObject(Settings, SerializerSettings));
        }
    }

    public class SettingsFile<T> : SettingsFile where T : new()
    {
        public SettingsFile(string settingsPath) : base(settingsPath, typeof(T), new T()) { }

        public new T Settings
        {
            get { return (T)base.Settings; }
            set { base.Settings = value; }
        }

    }
}
