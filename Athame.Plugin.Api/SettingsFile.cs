namespace Athame.Plugin.Api
{
    public class SettingsFile : ISettingsFile
    {
        public string SettingsPath { get; set; }

        public virtual void Save()
        {
        }
    }
}
