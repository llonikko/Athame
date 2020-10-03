using System.Collections.Generic;
using System.Linq;

namespace Athame.Plugin.Api.Downloader
{
    public static class MediaFileTypes
    {
        /// <summary>
        /// Represents the MP3 filetype, with the extension .mp3 and the MIME type "audio/mpeg".
        /// </summary>
        public static FileType MP3
            => new FileType
            {
                Extension = "mp3",
                MimeType = "audio/mpeg"
            };

        /// <summary>
        /// Represents the AAC filetype, with the extension .aac and the MIME type "audio/aac".
        /// </summary>
        public static FileType AAC
            => new FileType
            {
                Extension = "aac",
                MimeType = "audio/aac"
            };

        /// <summary>
        /// Represents the MPEG 4 audio filetype, with the extension .m4a and the MIME type "audio/mp4".
        /// </summary>
        public static FileType M4A
            => new FileType
            {
                Extension = "m4a",
                MimeType = "audio/mp4"
            };

        /// <summary>
        /// Represents the Ogg Vorbis filetype, with the extension .ogg and the MIME type "audio/ogg".
        /// </summary>
        public static FileType OGG
            => new FileType
            {
                Extension = "ogg",
                MimeType = "audio/ogg"
            };

        /// <summary>
        /// Represents the FLAC filetype, with the extension .flac and the MIME type "audio/x-flac".
        /// </summary>
        public static FileType FLAC
            => new FileType
            {
                Extension = "flac",
                MimeType = "audio/x-flac"
            };

        public static FileType JPEG
            => new FileType
            {
                Extension = "jpg",
                MimeType = "image/jpeg"
            };

        public static FileType PNG
            => new FileType
            {
                Extension = "png",
                MimeType = "image/png"
            };

        public static FileType WEBP
            => new FileType
            {
                Extension = "webp",
                MimeType = "image/webp"
            };

        /// <summary>
        /// Represents an unknown filetype.
        /// </summary>
        public static FileType Unknown 
            => new FileType();

        public static FileType ByMimeType(string mimeType)
            => (from ft in AllTypes where ft.MimeType == mimeType select ft)
            .FirstOrDefault() ?? Unknown;

        public static FileType ByExtension(string extension)
            => (from ft in AllTypes where ft.Extension == extension select ft)
            .FirstOrDefault() ?? Unknown;

        /// <summary>
        /// Adds a file type to the registry. Do not add types with different extensions.
        /// </summary>
        /// <param name="type"></param>
        public static void AddType(FileType type) 
            => AllTypes.Append(type);

        public static IEnumerable<FileType> AllTypes
            => new HashSet<FileType>
            {
                MP3, AAC, M4A, OGG, FLAC,
                JPEG, PNG, WEBP
            };
    }
}
