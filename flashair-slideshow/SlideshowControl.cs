using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using flashair_slideshow.Properties;

namespace flashair_slideshow
{
    internal sealed class SlideshowControl
    {
        public EventHandler<ImageChosenEventArgs> ImageChosen;
        public Settings Settings { get; }

        private readonly HashSet<string> _extensions;
        private readonly Random _random;

        public SlideshowControl(Settings settings)
        {
            Settings = settings;
            _extensions = GetExtensionsHashset();
            _random = new Random();
        }


        public void Start(CancellationToken cancellationToken)
        {
            var directory = new DirectoryInfo(Settings.PictureFolder);

            List<FileInfo> files = GetFiles(directory);

            while (true)
            {
                FileInfo fileInfo = GetRandomFile(files);
                Image image = ReadImage(fileInfo);

                FireImageChosen(image);
                bool cancelled = cancellationToken.WaitHandle.WaitOne(TimeSpan.FromSeconds(Settings.MaximumDisplaySeconds));

                if (cancelled)
                {
                    return;
                }
            }
        }

        private static Image ReadImage(FileInfo fileInfo)
        {
            var image = Image.FromFile(fileInfo.FullName);

            foreach (var prop in image.PropertyItems)
            {
                if (prop.Id == 0x0112) //value of EXIF
                {
                    int orientationValue = prop.Value[0];
                    RotateFlipType rotateFlipType = GetOrientationToFlipType(orientationValue);
                    image.RotateFlip(rotateFlipType);
                    break;
                }
            }

            return image;
        }

        private static RotateFlipType GetOrientationToFlipType(int orientationValue)
        {
            RotateFlipType rotateFlipType;

            switch (orientationValue)
            {
                case 1:
                    rotateFlipType = RotateFlipType.RotateNoneFlipNone;
                    break;
                case 2:
                    rotateFlipType = RotateFlipType.RotateNoneFlipX;
                    break;
                case 3:
                    rotateFlipType = RotateFlipType.Rotate180FlipNone;
                    break;
                case 4:
                    rotateFlipType = RotateFlipType.Rotate180FlipX;
                    break;
                case 5:
                    rotateFlipType = RotateFlipType.Rotate90FlipX;
                    break;
                case 6:
                    rotateFlipType = RotateFlipType.Rotate90FlipNone;
                    break;
                case 7:
                    rotateFlipType = RotateFlipType.Rotate270FlipX;
                    break;
                case 8:
                    rotateFlipType = RotateFlipType.Rotate270FlipNone;
                    break;
                default:
                    rotateFlipType = RotateFlipType.RotateNoneFlipNone;
                    break;
            }

            return rotateFlipType;
        }

        private FileInfo GetRandomFile(IList<FileInfo> files)
        {
            if (files == null)
            {
                throw new ArgumentNullException(nameof(files));
            }
            if (files.Count == 0)
            {
                throw new ArgumentException(@"At least one file required", nameof(files));
            }

            return files[_random.Next(files.Count - 1)];
        }

        private List<FileInfo> GetFiles(DirectoryInfo directory)
        {
            FileInfo[] allFiles = directory.GetFiles("*", SearchOption.AllDirectories);

            List<FileInfo> files = allFiles.Where(x => _extensions.Contains(x.Extension, StringComparer.OrdinalIgnoreCase))
                .ToList();
            return files;
        }

        private HashSet<string> GetExtensionsHashset()
        {
            var hashSet = new HashSet<string>();

            foreach (string extension in Settings.FilenameExtensions)
            {
                hashSet.Add(extension);
            }

            return hashSet;
        }

        private void FireImageChosen(Image image)
        {
            ImageChosen?.Invoke(this, new ImageChosenEventArgs(image));
        }
    }
}