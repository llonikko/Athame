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
                Location = new StringBuilder(Location).ToString(),
                PathFormat = new StringBuilder(PathFormat).ToString(),
                AskLocation = AskLocation
            };

        public MediaPreference GetCopy()
            => Clone() as MediaPreference;
    }
}
