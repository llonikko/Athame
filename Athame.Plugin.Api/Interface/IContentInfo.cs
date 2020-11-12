namespace Athame.Plugin.Api.Interface
{
    public interface IContentInfo
    {
        string Name { get; }
        string Extension { get; }
        string GetContent();
    }
}
