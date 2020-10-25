using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Athame.Plugin.Api
{
    public static class JsonFileSettings
    {
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

        public static TSettings Load<TSettings>(string path) where TSettings : class, ISettingsFile, new()
        {
            TSettings settings;
            if (!File.Exists(path))
            {
                settings = new TSettings
                {
                    SettingsPath = path
                };
            }
            else
            {
                settings = Deserialize<TSettings>(path);
            }

            return settings;
        }

        private static TSettings Deserialize<TSettings>(string path) where TSettings : class, ISettingsFile, new()
        {
            TSettings settings;
            try
            {
                settings = JsonSerializer.Deserialize<TSettings>(File.ReadAllText(path), JsonOptions);
                settings.SettingsPath = path;
            }
            catch (JsonException)
            {
                settings = new TSettings
                {
                    SettingsPath = path
                };
                Save(settings);
            }

            return settings;
        }

        public static void Save<TSettings>(TSettings settings) where TSettings : class, ISettingsFile
            => File.WriteAllText(settings.SettingsPath, JsonSerializer.Serialize(settings, JsonOptions));
    }
}
