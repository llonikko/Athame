using Athame.Avalonia.Extensions;
using Athame.Avalonia.Models;
using Athame.Avalonia.Resources;
using Athame.Core;
using Athame.Core.Download;
using Athame.Core.Search;
using Athame.Plugin.Api.Service;
using Avalonia.Media.Imaging;
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
        private readonly UrlResolver urlResolver;

        [Reactive]
        public string SearchText { get; set; }

        public Bitmap UrlValidationStatusImage { [ObservableAsProperty]get; }
        public string UrlValidationStatusText { [ObservableAsProperty]get; }
        public bool IsUrlValid { [ObservableAsProperty]get; }
        public bool IsValidating { [ObservableAsProperty]get; }
        public bool IsSearching { [ObservableAsProperty]get; }
        public MediaDownloadService SearchResult { [ObservableAsProperty]get; }

        public ReactiveCommand<Unit, MediaDownloadService> SearchMediaCommand { get; }

        public SearchViewModel()
        {
            urlResolver = Locator.Current.GetService<AthameApp>().UrlResolver;

            var parseResult = this
                .WhenAnyValue(x => x.SearchText)
                .Select(url => Parse(url));

            parseResult
                .Select(result => GetMessage(result))
                .ToPropertyEx(this, x => x.UrlValidationStatusText);

            parseResult
                .Select(x => x == UrlParseResult.NullOrEmptyString)
                .Select(validating => !validating)
                .ToPropertyEx(this, x => x.IsValidating);

            parseResult
                .Select(x => x == UrlParseResult.Success)
                .ToPropertyEx(this, x => x.IsUrlValid);

            var isValid = this.WhenAnyValue(x => x.IsUrlValid);

            isValid
                .Select(valid => valid ? Images.Success : Images.Error)
                .ToPropertyEx(this, x => x.UrlValidationStatusImage);

            var canSearch = isValid;
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

        private UrlParseResult Parse(string url) 
            => urlResolver.Parse(url);

        private string GetMessage(UrlParseResult result) 
            => urlResolver.GetMessage(result);

        private async Task<MediaDownloadService> Search()
        {
            var settings = Locator.Current.GetService<AthameApp>().AppSettings.Current;
            var folderBrowser = Locator.Current.GetService<FolderBrowserDialog>();
            var preference = settings.GetPreference(urlResolver.MediaUri.MediaType);

            var path = preference.AskLocation
                ? await folderBrowser.SelectFolder(preference.Location)
                : preference.Location;
            var format = preference.GetPlatformSaveFormat();
            var service = urlResolver.Service;
            var media = await urlResolver.ResolveAsync();

            var context = new MediaDownloadContext
            {
                DownloadFolderPath = path,
                MediaPathFormat = format
            };
            return new MediaDownloadService(service, context, media);
        }
    }
}
