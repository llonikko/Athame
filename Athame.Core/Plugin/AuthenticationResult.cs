using System;

namespace Athame.Core.Plugin
{
    public class AuthenticationResult
    {
        public string ServiceName { get; set; }
        public bool IsAuthenticated { get; set; }
        public Exception Exception { get; set; }
    }
}
