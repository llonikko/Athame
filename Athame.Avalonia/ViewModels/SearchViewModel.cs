using Athame.Avalonia.Models;
using Athame.Core;
using Athame.Core.Download;
using Athame.Core.Search;
using Athame.Plugin.Api.Service;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Athame.Avalonia.ViewModels
{
    public class SearchViewModel : ViewModelBase
    {
        private readonly UrlResolver resolver;

        [Reactive]
        public string SearchText { get; set; }

        public string UrlValidationStatusText { [ObservableAsProperty]get; }
        public bool IsUrlValid { [ObservableAsProperty]get; }
        public bool IsValidating { [ObservableAsProperty]get; }
        public bool IsSearching { [ObservableAsProperty]get; }

        public UrlParseResult ParseResult { [ObservableAsProperty]get; }
        public MediaDownloadItem SearchResult { [ObservableAsProperty]get; }

        public ReactiveCommand<Unit, MediaDownloadItem> SearchMediaCommand { get; }

        public SearchViewModel()
        {
            resolver = new UrlResolver();

            var parseResult = this
                .WhenAnyValue(x => x.SearchText)
                .Select(url => resolver.Parse(url));
            parseResult
                .Select(result => result.GetMessage())
                .ToPropertyEx(this, x => x.UrlValidationStatusText);
            parseResult
                .ToPropertyEx(this, x => x.ParseResult);

            var parseStatus = parseResult.Select(x => x.ParseStatus);
            parseStatus
                .Select(x => x == UrlParseStatus.NullOrEmptyString)
                .Select(validating => !validating)
                .ToPropertyEx(this, x => x.IsValidating);
            parseStatus
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

        private async Task<MediaDownloadItem> Search()
        {
            var settings = Locator.Current.GetService<AthameApp>().AppSettings;
            var folderBrowser = Locator.Current.GetService<FolderBrowserDialog>();
            var preference = settings.GetPreference(ParseResult.MediaDescriptor.MediaType);

            var path = preference.AskLocation
                ? await folderBrowser.SelectFolder(preference.Location)
                : preference.Location;
            var format = preference.GetPlatformSaveFormat();

            var media = await resolver.ResolveMedia(ParseResult.MediaDescriptor);
            var context = new MediaDownloadContext
            {
                Root = path,
                MediaPathFormat = format
            };
            return new MediaDownloadItem(ParseResult.MediaDescriptor, context, media);
        }
    }
}
