using Athame.Core.Interface;

namespace Athame.Core.Extensions
{
    public static class IUrlParseResultExtensions
    {
        public static IMediaUrlParseResult AsMediaUrlParseResult(this IUrlParseResult result)
            => result as IMediaUrlParseResult;
    }
}
