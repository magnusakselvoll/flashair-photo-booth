using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using flashair_slideshow.Properties;

namespace flashair_slideshow
{
    internal sealed class SlideshowControl
    {
        public Settings Settings { get; }
        public Form Form { get; }

        public SlideshowControl(Settings settings, Form form)
        {
            Settings = settings;
            Form = form;
        }

        public void Start()
        {
            var directory = new DirectoryInfo(Settings.PictureFolder);
            var rand = new Random();

            FileInfo[] files = directory.GetFiles("*.jpg", SearchOption.AllDirectories);

            var pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            Form.Controls.Add(pictureBox);
            Form.Shown += (o, e) => Form.Activate();
            Form.Show();

            foreach (var fileInfo in files.OrderBy(x => rand.Next()))
            {
                pictureBox.Image = Image.FromFile(fileInfo.FullName);    
                Form.BringToFront();
                Thread.Sleep(TimeSpan.FromSeconds(Settings.MaximumDisplaySeconds));
            }

            Form.Close();
        }
    }
}