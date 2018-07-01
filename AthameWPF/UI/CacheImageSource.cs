using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Athame.Core.Logging;
using Athame.PluginAPI.Service;

namespace AthameWPF.UI
{
    public class CacheImageSource : BitmapSource
    {
        private ServiceMediaId mediaId;
        private bool isWaiting;
        private event EventHandler WaitCompleted;

        private async void GetCachedBitmapAsync()
        {
//            isWaiting = true;
//            WaitCompleted?.Invoke(this, EventArgs.Empty);
//
//            var s = mediaId.ToString();
//            if (PictureCache.HasPicture(s))
//            {
//                var entry = PictureCache.GetPicture(s);
//
//                if (Thumbnail && entry.OriginalPicture.IsThumbnailAvailable)
//                {
//                    await entry.GetThumbnailAsync();
//                    Image = entry.XamlThumbnailBitmap;
//                }
//                else
//                {
//                    await entry.GetFullSizeAsync();
//                    Image = entry.XamlFullSizeBitmap;
//                }
//
//            }
//            else
//            {
//                Image = new BitmapImage(ResourceUriHelper.BuildUri("DefaultArtwork.png"));
//                Log.Warning(nameof(CacheImageSource), $"No picture cache entry for {s}");
//            }
//
//            isWaiting = false;
//            WaitCompleted?.Invoke(this, EventArgs.Empty);
        }


        public ServiceMediaId MediaId
        {
            get { return mediaId; }

            set
            {
                mediaId = value;
                GetCachedBitmapAsync();
            }

        }

        public bool Thumbnail { get; set; }

        public int ThumbnailSize { get; set; } = 300;

        public BitmapImage Image
        {
            get;
            private set;
        }

        public CacheImageSource()
        {
            Image = new BitmapImage();
        }

        public override PixelFormat Format => Image.Format;

        public override int PixelWidth => Image.PixelWidth;

        public override int PixelHeight => Image.PixelHeight;

        public override double DpiX => Image.DpiX;

        public override double DpiY => Image.DpiY;

        public override BitmapPalette Palette => Image.Palette;

        public override void CopyPixels(Array pixels, int stride, int offset)
        {
            Image.CopyPixels(pixels, stride, offset);
        }

        public override void CopyPixels(Int32Rect sourceRect, Array pixels, int stride, int offset)
        {
            Image.CopyPixels(sourceRect, pixels, stride, offset);
        }

        public override void CopyPixels(Int32Rect sourceRect, IntPtr buffer, int bufferSize, int stride)
        {
            Image.CopyPixels(sourceRect, buffer, bufferSize, stride);
        }

        public override bool IsDownloading => isWaiting;

        public override event EventHandler DownloadCompleted
        {
            add { WaitCompleted += value; }
            remove { WaitCompleted -= value; }
        }

        public override event EventHandler<DownloadProgressEventArgs> DownloadProgress
        {
            add { Image.DownloadProgress += value; }
            remove { Image.DownloadProgress -= value; }
        }

        public override event EventHandler<ExceptionEventArgs> DownloadFailed
        {
            add { Image.DownloadFailed += value; }
            remove { Image.DownloadFailed -= value; }
        }

        public override event EventHandler<ExceptionEventArgs> DecodeFailed
        {
            add { Image.DecodeFailed += value; }
            remove { Image.DecodeFailed -= value; }
        }

        protected override Freezable CreateInstanceCore()
        {
            return Image;
        }
    }
}
