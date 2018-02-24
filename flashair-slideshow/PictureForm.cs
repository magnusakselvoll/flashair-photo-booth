using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using flashair_slideshow.Properties;

namespace flashair_slideshow
{
    internal partial class PictureForm : Form
    {
        public Settings Settings { get; }
        private Task Task { get; set; }

        private CancellationTokenSource CancellationTokenSource { get; set; }

        public PictureForm(Settings settings)
        {
            Settings = settings;
            InitializeComponent();
        }

        private void PictureForm_Load(object sender, EventArgs e)
        {
            Cursor.Hide();

            var control = new SlideshowControl(Settings.Default);
            control.ImageChosen += (o, ie) => _pictureBox.Image = ie.Image;

            CancellationTokenSource = new CancellationTokenSource();
            Task = Task.Factory.StartNew(() => control.Start(CancellationTokenSource.Token), CancellationTokenSource.Token);
        }

        private void PictureForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void PictureForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Task != null && Task.Status == TaskStatus.Running)
            {
                CancellationTokenSource.Cancel();

                Task.Wait(TimeSpan.FromSeconds(1));
            }

            Task = null;
            CancellationTokenSource = null;
        }
    }
}
