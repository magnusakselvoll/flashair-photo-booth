using System;
using System.Drawing;

namespace flashair_slideshow
{
    internal sealed class ImageChosenEventArgs : EventArgs
    {
        public Image Image { get; }

        public ImageChosenEventArgs(Image image)
        {
            Image = image;
        }
    }
}