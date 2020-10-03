using System;
using Athame.Plugin.Api.Service;

namespace Athame.Core.Plugin
{
    public class AuthenticationResult
    {
        public bool IsSuccess { get; }
        public IMediaService Service { get; }
        public Exception Exception { get; }

        public AuthenticationResult(IMediaService service, bool isSuccess, Exception exception = null)
        {
            Service = service;
            IsSuccess = isSuccess;
            Exception = exception;
        }

        public override string ToString()
            => $"{nameof(Service)}: {Service}, {nameof(IsSuccess)}: {IsSuccess}";
    }
}
