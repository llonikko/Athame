namespace Athame.Plugin.Api.Service
{
    public interface IContentInfo
    {
        string Name { get; }
        string Extension { get; }
        string GetContent();
    }
}
