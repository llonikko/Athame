using ReactiveUI;
using System;

namespace Athame.Avalonia.Extensions
{
    public static class RoutingStateExtensions
    {
        public static IObservable<IRoutableViewModel> Navigate<T>(this RoutingState router)
            where T : class, IRoutableViewModel, new()
            => router.Navigate.Execute(new T());
    }
}
