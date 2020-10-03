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
    }
}
