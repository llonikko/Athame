using Athame.Avalonia.ViewModels;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive.Disposables;

namespace Athame.Avalonia.Views
{
    public class ProgressStatusView : ReactiveUserControl<ProgressStatusViewModel>
    {
        public TextBlock MediaDownloadStatusTextBlock 
            => this.FindControl<TextBlock>("MediaDownloadStatusTextBlock");
        public TextBlock TrackDownloadStatusTextBlock 
            => this.FindControl<TextBlock>("TrackDownloadStatusTextBlock");
        public TextBlock TrackDownloadProgressTextBlock 
            => this.FindControl<TextBlock>("TrackDownloadProgressTextBlock");
        public TextBlock TrackDownloadTitleTextBlock 
            => this.FindControl<TextBlock>("TrackDownloadTitleTextBlock");
        public ProgressBar TrackDownloadProgressBar 
            => this.FindControl<ProgressBar>("TrackDownloadProgressBar");

        public ProgressStatusView()
        {
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.MediaDownloadStatus, v => v.MediaDownloadStatusTextBlock.Text)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.TrackDownloadStatus, v => v.TrackDownloadStatusTextBlock.Text)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.TrackDownloadProgressPercentage, v => v.TrackDownloadProgressTextBlock.Text, progress => $"{progress}%")
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.TrackDownloadProgressPercentage, v => v.TrackDownloadProgressBar.Value)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.TrackDownloadTitle, v => v.TrackDownloadTitleTextBlock.Text)
                    .DisposeWith(disposables);
            });

            InitializeComponent();
        }

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
