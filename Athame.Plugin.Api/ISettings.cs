namespace Athame.Plugin.Api
{
    public interface ISettings
    {
        string SettingsPath { get; set; }
        void Load();
        void Save();
    }

    public interface ISettings<T> : ISettings
    {
        T Current { get; }
        void Update(T settings);
    }
}
