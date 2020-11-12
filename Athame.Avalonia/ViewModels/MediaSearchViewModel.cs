using Athame.Core.Download;
using Athame.Core.Interface;
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
        private IUrlParseResult ParseResult { [ObservableAsProperty]get; }

        [Reactive]
        public string SearchText { get; set; }

        public string UrlValidationMessage { [ObservableAsProperty]get; }
        public bool IsUrlValid { [ObservableAsProperty]get; }
        public bool IsValidating { [ObservableAsProperty]get; }
        public bool IsSearching { [ObservableAsProperty]get; }

        public MediaItem SearchResult { [ObservableAsProperty]get; }

        public ReactiveCommand<Unit, MediaItem> SearchMediaCommand { get; }

        public MediaSearchViewModel()
        {
            this
                .WhenAnyValue(x => x.SearchText)
                .Select(url => UrlResolver.ResolveUrl(url))
                .ToProperty(this, x => x.ParseResult);

            this.WhenAnyValue(x => x.ParseResult)
                .Select(x => x.Message)
                .ToPropertyEx(this, x => x.UrlValidationMessage);

            var status = this
                .WhenAnyValue(x => x.ParseResult)
                .Select(x => x.Status);
            status
                .Select(x => x == UrlParseStatus.NullOrEmptyString)
                .Select(validating => !validating)
                .ToPropertyEx(this, x => x.IsValidating);
            status
                .Select(x => x == UrlParseStatus.Success)
                .ToPropertyEx(this, x => x.IsUrlValid);

            var canSearch = this.WhenAnyValue(x => x.IsUrlValid);
            SearchMediaCommand = ReactiveCommand.CreateFromTask(SearchMedia, canSearch);
            SearchMediaCommand.IsExecuting
                .ToPropertyEx(this, x => x.IsSearching);
            SearchMediaCommand.ThrownExceptions
                .Subscribe(error =>
                {   /* Handle exceptions. */
                    if (error is ResourceNotFoundException)
                    {
                        // var msgBox = MessageBox.Avalonia.MessageBoxManager
                        //     .GetMessageBoxStandardWindow("Not Found", ex.Message);
                        // msgBox.Show();
                        Log.Debug(error.Message, "Not Found");
                    }
                    else
                    {
                        Log.Debug(error.Message, "While attempting to resolve media");
                    }
                });

            this.WhenAnyObservable(x => x.SearchMediaCommand)
                .ToPropertyEx(this, x => x.SearchResult);
        }

        private async Task<MediaItem> SearchMedia()
        {
            var resolver = MediaResolver.Create(ParseResult);
            return await resolver.ResolveMedia();
        }
    }
}
