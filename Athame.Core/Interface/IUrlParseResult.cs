using Athame.Core.Search;
using Athame.Plugin.Api.Service;

namespace Athame.Core.Interface
{
    /// <summary>
    /// Represents a parse result parsed with <see cref="UrlResolver.Parse"/>.
    /// </summary>
    public interface IUrlParseResult
    {
        UrlParseStatus Status { get; }
        MediaDescriptor Result { get; }
        string GetMessage();
    }
}
