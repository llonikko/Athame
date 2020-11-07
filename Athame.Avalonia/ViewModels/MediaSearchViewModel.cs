using Athame.Core.Download;
using Athame.Core.Search;
using Athame.Plugin.Api.Service;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Athame.Avalonia.ViewModels
{
    public class MediaSearchViewModel : ViewModelBase
    {
        private MediaDescriptor Descriptor { [ObservableAsProperty]get; }

        [Reactive]
        public string SearchText { get; set; }

        public string UrlValidationStatusText { [ObservableAsProperty]get; }
        public bool IsUrlValid { [ObservableAsProperty]get; }
        public bool IsValidating { [ObservableAsProperty]get; }
        public bool IsSearching { [ObservableAsProperty]get; }

        public Core.Download.MediaItem SearchResult { [ObservableAsProperty]get; }

        public ReactiveCommand<Unit, MediaItem> SearchMediaCommand { get; }

        public MediaSearchViewModel()
        {
            var result = this
                .WhenAnyValue(x => x.SearchText)
                .Select(url => UrlResolver.Parse(url));
            result
                .Select(result => result.GetMessage())
                .ToPropertyEx(this, x => x.UrlValidationStatusText);
            result
                .Select(x => x.Result)
                .ToPropertyEx(this, x => x.Descriptor);

            var status = result.Select(x => x.Status);
            status
                .Select(x => x == UrlParseStatus.NullOrEmptyString)
                .Select(validating => !validating)
                .ToPropertyEx(this, x => x.IsValidating);
            status
                .Select(x => x == UrlParseStatus.Success)
                .ToPropertyEx(this, x => x.IsUrlValid);

            var canSearch = this.WhenAnyValue(x => x.IsUrlValid);
            SearchMediaCommand = ReactiveCommand.CreateFromTask(Search, canSearch);
            SearchMediaCommand.IsExecuting
                .ToPropertyEx(this, x => x.IsSearching);
            SearchMediaCommand.ThrownExceptions
                .Subscribe(error =>
                {   /* Handle exceptions. */
                    if (error is ResourceNotFoundException ex)
                    {
                        // var msgBox = MessageBox.Avalonia.MessageBoxManager
                        //     .GetMessageBoxStandardWindow("Not Found", ex.Message);
                        // msgBox.Show();
                        Log.Debug(ex.Message, "Not Found");
                    }
                    else
                    {
                        Log.Debug(error.Message, "While attempting to resolve media");
                    }
                });

            this.WhenAnyObservable(x => x.SearchMediaCommand)
                .ToPropertyEx(this, x => x.SearchResult);
        }

        private async Task<Core.Download.MediaItem> Search()
        {
            var resolver = new MediaResolver(Descriptor);
            return await resolver.ResolveMedia();
        }
    }
}
