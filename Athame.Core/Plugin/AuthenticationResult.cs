using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athame.PluginAPI.Service;

namespace Athame.Core.Plugin
{
    public class AuthenticationResult
    {
        public MusicService Service { get; internal set; }

        public bool Result { get; internal set; }

        public Exception Exception { get; internal set; }

        internal AuthenticationResult(MusicService service, bool result, Exception exception)
        {
            Service = service;
            Result = result;
            Exception = exception;
        }

        public override string ToString()
        {
            return $"{nameof(Service)}: {Service}, {nameof(Result)}: {Result}";
        }
    }
}
