using System;

namespace Athame.Plugin.Api.Downloader
{
    public class FileType
    {
        /// <summary>
        /// The extension of the file. This should be used only for writing to disk; for URLs, you should check
        /// the response's Content-Type header then match it to the <see cref="MimeType"/> property.
        /// Note also the extension must not begin with a period.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// The file type's MIME type.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Appends the extension to the specified path or URL. If this FileType is unknown, then returns the
        /// passed string as-is.
        /// </summary>
        /// <param name="pathOrUrl">A path or URL to append to.</param>
        /// <returns>The appended string.</returns>
        public string AppendExtension(string pathOrUrl)
            => ReferenceEquals(this, MediaFileTypes.Unknown) ? pathOrUrl : $"{pathOrUrl}.{Extension}";

        protected bool Equals(FileType other)
            => string.Equals(Extension, other.Extension, StringComparison.OrdinalIgnoreCase) 
            && string.Equals(MimeType, other.MimeType, StringComparison.OrdinalIgnoreCase);

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FileType)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (StringComparer.OrdinalIgnoreCase.GetHashCode(Extension) * 397) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(MimeType);
            }
        }
    }
}
