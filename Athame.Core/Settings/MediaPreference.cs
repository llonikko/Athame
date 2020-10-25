using System;
using System.IO;
using System.Text;

namespace Athame.Core.Settings
{
    public class MediaPreference : ICloneable
    {
        public string Location { get; set; }
        public string PathFormat { get; set; }
        public bool AskLocation { get; set; }

        public string GetPlatformSaveFormat()
            => Path.DirectorySeparatorChar == '/' ? PathFormat : PathFormat.Replace('/', Path.DirectorySeparatorChar);

        public object Clone()
            => new MediaPreference
            {
                Location = Location,
                PathFormat = PathFormat,
                AskLocation = AskLocation
            };
    }
}
