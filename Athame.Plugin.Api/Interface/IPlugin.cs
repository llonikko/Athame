using Athame.Plugin.Api.Service;

namespace Athame.Plugin.Api
{
    public interface IPlugin
    {
        /// <summary>
        /// The plugin's name. Required.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The plugin's description. Optional.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The plugin's author. Optional.
        /// </summary>
        string Author { get; }

        /// <summary>
        /// The plugin's version. Required.
        /// </summary>
        int Version { get; }

        /// <summary>
        /// Gets the media service provider
        /// </summary>
        /// <return>The media service provider</return>
        IMediaService Service { get; }

        /// <summary>
        /// An object that holds persistent settings. Settings are deserialized from storage when the service is first initialized and 
        /// serialized when the user clicks "Save" on the settings form and when the application closes. Implementations should provide a
        /// "default" settings instance when there are no persisted settings available.
        /// </summary>
        ISettingsFile Settings { get; }

        /// <summary>
        /// Returns a settings control to display in the settings form. Do not cache this in your implementation, as it is always disposed
        /// when the settings form closes.
        /// </summary>
        /// <returns>A settings control to display.</returns>
        ISettingsControl SettingsControl { get; }

        /// <summary>
        /// Called when the plugin is initialized.
        /// </summary>
        /// <param name="application">Info and methods for interacting with the host application</param>
        void Init(string settingsPath);
    }
}
