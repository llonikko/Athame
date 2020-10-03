namespace Athame.Plugin.Api.Service
{
    /// <summary>
    /// Represents a music artist.
    /// </summary>
    public class Artist
    {
        /// <summary>
        /// The artist's ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The artist's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A picture of the artist, if one exists.
        /// </summary>
        public MediaCover Picture { get; set; }

        public override string ToString() => Name;
    }
}