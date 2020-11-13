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

        public IList<ServiceRestoreStatus> ServicesRestoreStatus { get; }

        public ReactiveCommand<Unit, Unit> RestoreCommand { get; }

        public ServiceRestoreWindowViewModel(IEnumerable<IMediaService> services)
        {
            this.services = services;

            ServicesRestoreStatus = new List<ServiceRestoreStatus>();
            Init();

            RestoreCommand = ReactiveCommand.CreateFromTask(Restore);
        }

        private void Init()
        {
            foreach (var svc in services)
            {
                ServicesRestoreStatus.Add(new ServiceRestoreStatus
                {
                    IsAuthenticating = true,
                    Message = "Please wait...",
                    Name = svc.Name,
                    Account = svc.AsAuthenticatable().Account.FormattedName
                });
            }
        }

        private async Task Restore()
        {
            Log.Debug("Restore");
            var allRestored = await RestoreAll();
            // if (allRestored) 
            // {
            //     Close();
            // }
        }

        private async Task<bool> RestoreAll()
        {
            var auth = Locator.Current.GetService<AthameApp>().AuthenticationManager;

            var fails = new List<AuthenticationResult>();
            var restoreTasks = auth.Restore(services).ToList();

            Log.Debug("Starting restore, Restore task Count = {Count}", restoreTasks.Count);
            while (restoreTasks.Count > 0)
            {
                var finishedTask = await Task.WhenAny(restoreTasks);

                restoreTasks.Remove(finishedTask);

                var result = await finishedTask;

                var status = ServicesRestoreStatus.First(s => s.Name == result.ServiceName);
                status.IsAuthenticating = false;

                if (result.IsAuthenticated)
                {
                    Log.Debug("Restore complete for {Result}", result.ServiceName);
                    status.Message = "Signed in successfully";
                }
                else
                {
                    if (result.Exception != null)
                    {
                        Log.Error(result.Exception, "Restore failed for {Result}", result.ServiceName);
                    }
                    else
                    {
                        Log.Debug("Restore complete for {Result}", result.ServiceName);
                    }

                    fails.Add(result);
                    status.Message = "Error signing in";
                }
            }

            Log.Information("Finished restore");
            return fails.Count == 0;
        }
    }
}
