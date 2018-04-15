using System;

namespace Athame.Core.Plugin
{
    public class PluginIncompatibleException : Exception
    {
        public PluginIncompatibleException(string message) : base(message) { }
    }
}
