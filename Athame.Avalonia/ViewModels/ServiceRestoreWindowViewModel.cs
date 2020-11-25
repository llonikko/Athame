using Athame.Avalonia.Models;
using Athame.Core;
using Athame.Core.Plugin;
using Athame.Plugin.Api.Interface;
using ReactiveUI;
using Serilog;
using Splat;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace Athame.Avalonia.ViewModels
{
    public class ServiceRestoreWindowViewModel : ViewModelBase
    {
        private readonly IEnumerable<IMediaService> services;

        public static readonly ServiceRestoreWindowViewModel Null;

        public IList<ServiceRestoreStatus> StatusList { get; }

        public ReactiveCommand<Unit, Unit> RestoreCommand { get; }

        public ServiceRestoreWindowViewModel(IEnumerable<IMediaService> services)
        {
            this.services = services;

            StatusList = new List<ServiceRestoreStatus>();
            foreach (var svc in services)
            {
                StatusList.Add(ServiceRestoreStatus.Create(svc));
            }

            RestoreCommand = ReactiveCommand.CreateFromTask(Restore);
        }

        private async Task Restore()
        {
            Log.Debug("Restore");

            var tasks = Locator.Current.GetService<AthameApp>()
                .AuthenticationManager
                .Restore(services)
                .ToList();

            Log.Debug("Starting restore, Restore task Count = {Count}", tasks.Count);

            var allRestored = await RestoreAll(tasks);

            Log.Information("Finished restore");
            // if (allRestored) 
            // {
            //     Close();
            // }
        }

        private async Task<bool> RestoreAll(IList<Task<AuthenticationResult>> restoreTasks)
        {
            var fails = new List<AuthenticationResult>();
            while (restoreTasks.Count > 0)
            {
                var finishedTask = await Task.WhenAny(restoreTasks);
                AuthenticationResult result = await finishedTask;

                SetStatus(result);
                LogResult(result);

                if (!result.IsAuthenticated)
                {
                    fails.Add(result);
                }

                restoreTasks.Remove(finishedTask);
            }

            return fails.Count == 0;
        }

        private void SetStatus(AuthenticationResult result)
        {
            var status = StatusList.First(s => s.Name == result.ServiceName);
            status.IsAuthenticating = false;
            status.Message = result.IsAuthenticated ? "Sign-in successful." : "Sign-in failed.";
        }

        private void LogResult(AuthenticationResult result)
        {
            if (result.IsAuthenticated)
            {
                Log.Information("Restore complete for {Result}", result.ServiceName);
            }
            else
            {
                if (result.Exception != null)
                {
                    Log.Error(result.Exception, "Restore failed for {Result}", result.ServiceName);
                }
                else
                {
                    Log.Information("Restore complete for {Result}", result.ServiceName);
                }
            }
        }
    }
}
