namespace Athame.Plugin.Api.Service
{
    /// <summary>
    /// Represents a custom metadata value. Custom metadata can be used to denote service-specific attributes,
    /// such as if a track is explicit or an album or playlist is available in a higher quality.
    /// </summary>
    public class Metadata
    {
        /// <summary>
        /// The metadata name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The metadata's value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Whether to display the metadata to the user or not.
        /// </summary>
        public bool CanDisplay { get; set; }

        /// <summary>
        /// If <see cref="CanDisplay"/> is true, will display as a boolean value depending on whether <see cref="Value"/>
        /// is the string "True" or "False", rather than the actual value. 
        /// </summary>
        public bool IsFlag { get; set; }
    }
}
