using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athame.PluginAPI
{
    public class AthameApplication
    {
        private string userDataPath;

        /// <summary>
        /// True if the host application is presenting a window as the main method of interaction.
        /// </summary>
        public bool IsWindowed { get; set; }
        /// <summary>
        /// The full path to the user data directory
        /// </summary>
        public string UserDataPath {
            get
            {
                return userDataPath;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Value cannot be null", nameof(value));
                }
                userDataPath = Path.GetFullPath(value);
            }
        }

        public string UserDataPathOf(string relPath)
        {
            return Path.Combine(UserDataPath, relPath);
        }

    }
}
