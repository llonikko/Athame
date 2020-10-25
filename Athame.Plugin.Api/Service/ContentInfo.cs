using System.Text;

namespace Athame.Plugin.Api.Service
{
    public abstract class ContentInfo : IContentInfo
    {
        protected readonly StringBuilder content = new StringBuilder();

        public virtual string Name { get; set; }
        public virtual string Extension { get; }
        public string GetContent()
            => content.ToString();
    }
}
