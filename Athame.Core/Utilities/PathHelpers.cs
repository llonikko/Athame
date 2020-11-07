using Athame.Plugin.Api.Service;
using CenterCLR;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Athame.Core.Utilities
{
    /// <summary>
    /// Provides supplementary methods for the <see cref="Path"/> class.
    /// </summary>
    public static class PathHelpers
    {
        /// <summary>
        /// The character invalid path characters are replaced with.
        /// </summary>
        public const string ReplacementChar = "-";
        public const char NullSeparatorChar = '\0';

        /// <summary>
        /// Replaces invalid path characters in a filename.
        /// </summary>
        /// <param name="name">The filename to clean.</param>
        /// <returns>A valid filename.</returns>
        public static string CleanFilename(string name)
        {
            var invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            var invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);
            return Regex.Replace(name, invalidRegStr, ReplacementChar);
        }

        /// <summary>
        /// Splits a path by the null character and cleans each component.
        /// </summary>
        /// <param name="path">The path to clean.</param>
        /// <returns>A valid path.</returns>
        public static string CleanPath(string path)
        {
            var components = path.Split(NullSeparatorChar);
            var cleanComponents = new string[components.Length];
            for (var i = 0; i < components.Length; i++)
            {
                cleanComponents[i] = CleanFilename(components[i]);
            }
            return Join(cleanComponents);
        }

        public static string[] Split(string path)
            => path.Replace('/', Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar);

        public static string Join(string[] path)
            => string.Join(Path.DirectorySeparatorChar, path);

        public static void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static string CreateDefaultFileName(IMedia media)
            => CleanFilename($"{media.Artist} - {media.Title}");

        public static string FormatFilePath(object obj, string pathFormat)
        {
            // Hacky method to clean the file path
            var formatStrComponents = Split(pathFormat);
            var newFormat = string.Join(NullSeparatorChar.ToString(), formatStrComponents);
            var vars = Dictify.ObjectToDictionary(obj);
            // vars["ServiceName"] = title;
            var finalPath = CleanPath(Named.Format(newFormat, vars));
            return finalPath;
        }

        public static bool TryFormat(string format, IMediaCollection media, out string result)
        {
            try
            {
                result = $"{FormatFilePath(media.Tracks.First(), format)}.ext";
                return true;
            }
            catch (FormatException)
            {
                result = "Improperly formed format string - did you forget a { or }?";
            }
            catch (NullReferenceException)
            {
                result = "Undefined variable referenced - click the \"Help with path formats\" button to learn more.";
            }
            catch (Exception ex)
            {
                result = $"Unknown error ({ex.GetType().Name})";
            }
            return false;
        }
    }
}
