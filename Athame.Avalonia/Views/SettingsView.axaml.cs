using Athame.Avalonia.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive.Disposables;

namespace Athame.Avalonia.Views
{
    public class SettingsView : ReactiveUserControl<SettingsViewModel>
    {
        public Button CancelButton
            => this.FindControl<Button>("CancelButton");
        public Button SaveButton
            => this.FindControl<Button>("SaveButton");
        public Button SelectAlbumLocationButton
            => this.FindControl<Button>("SelectAlbumLocationButton");
        public Button SelectPlaylistLocationButton
            => this.FindControl<Button>("SelectPlaylistLocationButton");
        public Button AlbumPathFormatHelpButton
            => this.FindControl<Button>("AlbumPathFormatHelpButton");
        public Button PlaylistPathFormatHelpButton
            => this.FindControl<Button>("PlaylistPathFormatHelpButton");

        public TextBlock AlbumLocationTextBlock
            => this.FindControl<TextBlock>("AlbumLocationTextBlock");
        public TextBlock PlaylistLocationTextBlock
            => this.FindControl<TextBlock>("PlaylistLocationTextBlock");
        public TextBlock SampleAlbumPathTextBlock
            => this.FindControl<TextBlock>("SampleAlbumPathTextBlock");
        public TextBlock SamplePlaylistPathTextBlock
            => this.FindControl<TextBlock>("SamplePlaylistPathTextBlock");
        public TextBox AlbumPathFormatTextBox
            => this.FindControl<TextBox>("AlbumPathFormatTextBox");
        public TextBox PlaylistPathFormatTextBox
            => this.FindControl<TextBox>("PlaylistPathFormatTextBox");

        public CheckBox SameAsAlbumCheckBox
            => this.FindControl<CheckBox>("SameAsAlbumCheckBox");
        public CheckBox DontSavePlaylistArtworkCheckBox
            => this.FindControl<CheckBox>("DontSavePlaylistArtworkCheckBox");
        public CheckBox AskBeforeExitCheckBox
            => this.FindControl<CheckBox>("AskBeforeExitCheckBox");
        public CheckBox WriteWatermarkCheckBox
            => this.FindControl<CheckBox>("WriteWatermarkCheckBox");

        public ComboBox PlaylistFileTypeComboBox
            => this.FindControl<ComboBox>("PlaylistFileTypeComboBox");

        public ListBox PluginServicesListBox
            => this.FindControl<ListBox>("PluginServicesListBox");

        public PluginSettingsView PluginSettingsView
            => this.FindControl<PluginSettingsView>("PluginSettingsView");

        public SettingsView()
        {
            this.WhenActivated(disposables =>
            {
                #region Toggle Bindings
                this.OneWayBind(ViewModel, vm => vm.IsAlbumPathValid, v => v.SampleAlbumPathTextBlock.Foreground, valid => valid ? Brushes.Green : Brushes.Red)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.IsPlaylistPathValid, v => v.SamplePlaylistPathTextBlock.Foreground, valid => valid ? Brushes.Green : Brushes.Red)
                    .DisposeWith(disposables);
                #endregion

                #region ComboBox Bindings
                this.Bind(ViewModel, vm => vm.PlaylistFileType, v => v.PlaylistFileTypeComboBox.SelectedIndex)
                    .DisposeWith(disposables);
                #endregion

                #region CheckBox Bindings
                this.Bind(ViewModel, vm => vm.DontSavePlaylistArtwork, v => v.DontSavePlaylistArtworkCheckBox.IsChecked)
                    .DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.AskBeforeExit, v => v.AskBeforeExitCheckBox.IsChecked)
                    .DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.WriteWatermarkTag, v => v.WriteWatermarkCheckBox.IsChecked)
                    .DisposeWith(disposables);
                #endregion

                #region TextBox Bindings
                this.Bind(ViewModel, vm => vm.AlbumPathFormat, v => v.AlbumPathFormatTextBox.Text)
                    .DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.PlaylistPathFormat, v => v.PlaylistPathFormatTextBox.Text)
                    .DisposeWith(disposables);
                #endregion

                #region TextBlock Bindings
                this.OneWayBind(ViewModel, vm => vm.AlbumLocation, v => v.AlbumLocationTextBlock.Text)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.PlaylistLocation, v => v.PlaylistLocationTextBlock.Text)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.SampleAlbumPath, v => v.SampleAlbumPathTextBlock.Text)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.SamplePlaylistPath, v => v.SamplePlaylistPathTextBlock.Text)
                    .DisposeWith(disposables);
                #endregion

                #region Command Bindings
                this.BindCommand(ViewModel, vm => vm.SaveCommand, v => v.SaveButton)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.CancelCommand, v => v.CancelButton)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.SelectAlbumLocationCommand, v => v.SelectAlbumLocationButton)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.SelectPlaylistLocationCommand, v => v.SelectPlaylistLocationButton)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.ViewPathFormatHelpCommand, v => v.AlbumPathFormatHelpButton)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.ViewPathFormatHelpCommand, v => v.PlaylistPathFormatHelpButton)
                    .DisposeWith(disposables);
                #endregion

                this.Bind(ViewModel, vm => vm.SelectedPlugin, v => v.PluginServicesListBox.SelectedIndex)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.PluginSettingsView, v => v.PluginSettingsView.DataContext)
                    .DisposeWith(disposables);
            });

            InitializeComponent();
        }

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
