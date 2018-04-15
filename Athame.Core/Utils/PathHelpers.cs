using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace Athame.Core.Utils
{

    /// <summary>
    /// Provides supplementary methods for the <see cref="Path"/> class.
    /// </summary>
    public static class PathHelpers
    {
        private const int MaxPathLength = 260;

        /// <summary>
        /// The character invalid path characters are replaced with.
        /// </summary>
        public const string ReplacementChar = "-";

        /// <summary>
        /// Replaces invalid path characters in a filename.
        /// </summary>
        /// <param name="name">The filename to clean.</param>
        /// <returns>A valid filename.</returns>
        public static string CleanFilename(string name)
        {
            var invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            var invalidRegStr = String.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return Regex.Replace(name, invalidRegStr, ReplacementChar);
        }

        /// <summary>
        /// Splits a path by the null character and cleans each component.
        /// </summary>
        /// <param name="path">The path to clean.</param>
        /// <returns>A valid path.</returns>
        public static string CleanPath(string path)
        {
            var components = path.Split('\0');
            var cleanComponents = new string[components.Length];

            for (var i = 0; i < components.Length; i++)
            {
                cleanComponents[i] = CleanFilename(components[i]);
            }
            return String.Join(Path.DirectorySeparatorChar.ToString(), cleanComponents);
        }

        public static bool IsPathTooLong(string pathName)
        {
            if (pathName.Length < MaxPathLength)
            {
                return false;
            }
            // Check if the Windows 10 LongPathsEnabled registry key is set
            var longPathsEnabled = (int)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "LongPathsEnabled", 0);
            return longPathsEnabled != 0;
        }

        public static string[] SplitPathFormat(string path)
        {
            path = path.Replace('/', '\\');
            return path.Split('\\');

        }
    }
}
