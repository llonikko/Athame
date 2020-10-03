using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Athame.Plugin.Api
{
    public class Settings<T> : ISettings<T> where T : class, new()
    {
        public string SettingsPath { get; set; }

        private static JsonSerializerOptions JsonOptions
            => new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };

        public Settings() 
        {
        }

        public Settings(string settingsPath)
        { 
            SettingsPath = settingsPath;
        }

        public T Current { get; protected set; }

        public void Load()
        {
            if (!File.Exists(SettingsPath))
            {
                Update(new T());
            }
            else
            {
                Deserialize();
            }
        }

        private void Deserialize()
        {
            try
            {
                Current = JsonSerializer.Deserialize<T>(File.ReadAllText(SettingsPath), JsonOptions);
            }
            catch (JsonException)
            {
                Update(new T());
            }
        }

        public void Save()
        {
            File.WriteAllText(SettingsPath, JsonSerializer.Serialize(Current, JsonOptions));
        }

        public void Update(T settings)
        {
            Current = settings;
            Save();
        }
    }
}
