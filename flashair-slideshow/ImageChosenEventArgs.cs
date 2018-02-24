using System;
using System.Drawing;
using System.IO;

namespace flashair_slideshow
{
    internal sealed class ImageChosenEventArgs : EventArgs
    {
        public Image Image { get; }
        public string FileName { get; }

        public ImageChosenEventArgs(Image image, string fileName)
        {
            Image = image;
            FileName = fileName;
        }
    }
}