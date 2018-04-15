using System.Diagnostics;

namespace Athame.Core.Logging
{
    public class DebugLogger : ILogger
    {
        public void Write(Level level, string moduleTag, string message)
        {
            Debug.WriteLine($"[{level}] {message}", moduleTag);
        }
    }
}
