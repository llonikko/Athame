using Athame.Avalonia.Resources;
using Athame.Avalonia.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using AvaloniaProgressRing;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Athame.Avalonia.Views
{
    public class SearchView : ReactiveUserControl<SearchViewModel>
    {
        public Button SearchButton 
            => this.FindControl<Button>("SearchButton");
        
        public TextBox SearchTextBox 
            => this.FindControl<TextBox>("SearchTextBox");
        
        public Image UrlValidationStatusImage 
            => this.FindControl<Image>("UrlValidationStatusImage");
        
        public TextBlock UrlValidationStatusTextBlock 
            => this.FindControl<TextBlock>("UrlValidationStatusTextBlock");
        
        public StackPanel UrlValidationStatusPanel 
            => this.FindControl<StackPanel>("UrlValidationStatusPanel");
        
        public ProgressRing SearchProgressRing 
            => this.FindControl<ProgressRing>("SearchProgressRing");

        public SearchView()
        {
            this.WhenActivated(disposables =>
            {
                this.BindCommand(ViewModel, vm => vm.SearchMediaCommand, v => v.SearchButton)
                    .DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.SearchText, v => v.SearchTextBox.Text)
                    .DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.IsValidating, v => v.UrlValidationStatusPanel.IsVisible)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.UrlValidationStatusText, v => v.UrlValidationStatusTextBlock.Text)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.IsUrlValid, v => v.UrlValidationStatusTextBlock.Foreground, valid => valid ? Brushes.Green : Brushes.Red)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.IsUrlValid, v => v.UrlValidationStatusImage.Source, valid => valid ? Images.Success : Images.Error)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.IsSearching, v => v.SearchProgressRing.IsActive)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.IsSearching, v => v.SearchButton.IsVisible, isSearching => !isSearching)
                    .DisposeWith(disposables);

                Observable
                    .FromEventPattern(SearchTextBox, nameof(SearchTextBox.Tapped))
                    .Subscribe(e =>
                    {
                        if (e.Sender is TextBox t && !string.IsNullOrEmpty(t.Text))
                        {
                            t.SelectionStart = 0;
                            t.SelectionEnd = t.Text.Length;
                        }
                    })
                    .DisposeWith(disposables);
            });

            InitializeComponent();
        }

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
