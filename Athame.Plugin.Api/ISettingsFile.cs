namespace Athame.Plugin.Api
{
    public interface ISettingsFile
    {
        string SettingsPath { get; set; }
        void Save();
    }
}
