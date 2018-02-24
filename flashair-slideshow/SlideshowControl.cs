using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using flashair_slideshow.Properties;

namespace flashair_slideshow
{
    internal sealed class SlideshowControl
    {
        public Settings Settings { get; }
        public EventHandler<ImageChosenEventArgs> ImageChosen;

        public SlideshowControl(Settings settings)
        {
            Settings = settings;
        }

        public void Start(CancellationToken cancellationToken)
        {
            var directory = new DirectoryInfo(Settings.PictureFolder);
            var rand = new Random();

            FileInfo[] files = directory.GetFiles("*.jpg", SearchOption.AllDirectories);

            foreach (var fileInfo in files.OrderBy(x => rand.Next()))
            {
                FireImageChosen(Image.FromFile(fileInfo.FullName));
                bool cancelled = cancellationToken.WaitHandle.WaitOne(TimeSpan.FromSeconds(Settings.MaximumDisplaySeconds));

                if (cancelled)
                {
                    return;
                }
            }
        }

        private void FireImageChosen(Image image)
        {
            ImageChosen?.Invoke(this, new ImageChosenEventArgs(image));
        }
    }
}