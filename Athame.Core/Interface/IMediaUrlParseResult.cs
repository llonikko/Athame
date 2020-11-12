using Athame.Plugin.Api.Service;

namespace Athame.Core.Interface
{
    public interface IMediaUrlParseResult : IUrlParseResult
    {
        MediaDescriptor MediaDescriptor { get; }
    }
}
