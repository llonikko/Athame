using Athame.Plugin.Api.Interface;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Athame.Avalonia.Models
{
    public class ServiceRestoreStatus : ReactiveObject
    {
        [Reactive]
        public bool IsAuthenticating { get; set; }
        [Reactive]
        public string Message { get; set; }
        [Reactive]
        public string Name { get; set; }
        [Reactive]
        public string Account { get; set; }

        public static ServiceRestoreStatus Create(IMediaService service)
            => new ServiceRestoreStatus
            {
                IsAuthenticating = true,
                Message = "Please wait...",
                Name = service.Name,
                Account = service.Account.DisplayName
            };
    }
}
