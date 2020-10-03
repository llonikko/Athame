using Athame.Plugin.Api.Service;
using System.IO;

namespace Athame.Core.Utilities
{
    public static class AthameWriter
    {
        public static void Write(string fileName, ITextInfo textInfo)
        {
            FileInfo file = new FileInfo($"{fileName}.{textInfo.Extension}");
            if (!file.Exists)
            {
                var writer = file.CreateText();
                writer.WriteLine(textInfo.Content);
                writer.Close();
            }
        }

        public static void Write(string fileName, MediaCover picture)
        {
            if (!File.Exists(fileName))
            {
                File.WriteAllBytes(fileName, picture.Data);
            }
        }
    }
}
