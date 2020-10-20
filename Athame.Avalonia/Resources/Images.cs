using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;

namespace Athame.Avalonia.Resources
{
    public static class Images
    {
        public static Bitmap AppLogo
            => new Bitmap(AvaloniaLocator.Current
                .GetService<IAssetLoader>()
                .Open(new Uri($"avares://Athame.Avalonia/Assets/athame-logo.ico")));

        public static Bitmap Success
            => new Bitmap(AvaloniaLocator.Current
                .GetService<IAssetLoader>()
                .Open(new Uri($"avares://Athame.Avalonia/Assets/tick.png")));

        public static Bitmap Info
            => new Bitmap(AvaloniaLocator.Current
                .GetService<IAssetLoader>()
                .Open(new Uri($"avares://Athame.Avalonia/Assets/information-white.png")));

        public static Bitmap Error
            => new Bitmap(AvaloniaLocator.Current
                .GetService<IAssetLoader>()
                .Open(new Uri($"avares://Athame.Avalonia/Assets/exclamation-red.png")));
    }
}
