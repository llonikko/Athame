using Athame.Plugin.Api.Interface;
using Athame.Plugin.Api.Service;
using System.IO;

namespace Athame.Core.Utilities
{
    public static class AthameWriter
    {
        public static void Write(string fileName, IContentInfo info)
        {
            FileInfo file = new FileInfo($"{fileName}.{info.Extension}");
            if (!file.Exists)
            {
                var writer = file.CreateText();
                writer.WriteLine(info.GetContent());
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
