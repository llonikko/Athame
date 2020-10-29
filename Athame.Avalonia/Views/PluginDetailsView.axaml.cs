using Athame.Avalonia.ViewModels;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive.Disposables;

namespace Athame.Avalonia.Views
{
    public class PluginDetailsView : ReactiveUserControl<PluginDetailsViewModel>
    {
        public TextBlock PluginNameTextBlock => this.FindControl<TextBlock>("PluginNameTextBlock");
        public TextBlock PluginDescriptionTextBlock => this.FindControl<TextBlock>("PluginDescriptionTextBlock");
        public TextBlock PluginAuthorTextBlock => this.FindControl<TextBlock>("PluginAuthorTextBlock");
        public TextBlock PluginVersionTextBlock => this.FindControl<TextBlock>("PluginVersionTextBlock");

        public PluginDetailsView()
        {
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.PluginName, v => v.PluginNameTextBlock.Text)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.PluginDescription, v => v.PluginDescriptionTextBlock.Text)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.PluginAuthor, v => v.PluginAuthorTextBlock.Text)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.PluginVersion, v => v.PluginVersionTextBlock.Text)
                    .DisposeWith(disposables);
            });

            InitializeComponent();
        }

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
