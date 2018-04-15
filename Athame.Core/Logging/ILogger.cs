namespace Athame.Core.Logging
{
    public enum Level
    {
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }

    public interface ILogger
    {
        void Write(Level level, string moduleTag, string message);
    }
}
