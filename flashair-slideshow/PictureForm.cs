using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using flashair_slideshow.Properties;

namespace flashair_slideshow
{
    internal partial class PictureForm : Form
    {
        private Font _font;
        private Brush _brush;
        public Settings Settings { get; }
        private Task Task { get; set; }
        private string _fileName;

        private CancellationTokenSource CancellationTokenSource { get; set; }

        public PictureForm(Settings settings)
        {
            Settings = settings;
            InitializeComponent();
        }

        private void PictureForm_Load(object sender, EventArgs e)
        {
            _font = new Font(Font.FontFamily, 75);
            _brush = Brushes.OrangeRed;

            Cursor.Hide();

            StartSlideshow();
        }

        private void StartSlideshow()
        {
            var control = new SlideshowControl(Settings.Default);
            control.ImageChosen += (o, ie) =>
            {
                _pictureBox.Image = ie.Image;
                _fileName = Path.GetFileNameWithoutExtension(ie.FileName);
            };
            control.UnhandledExceptionThrown += SlideshowControlCrashed;

            CancellationTokenSource = new CancellationTokenSource();
            Task = Task.Factory.StartNew(() => control.Start(CancellationTokenSource.Token), CancellationTokenSource.Token);
        }

        private TimeSpan _lastRestartDelay = TimeSpan.FromSeconds(1);
        private readonly TimeSpan _maxRestartDelay = TimeSpan.FromMinutes(5);

        private void SlideshowControlCrashed(object sender, UnhandledExceptionEventArgs e)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry($"flashair-slideshow: Unhandled exception thrown from slideshow control: {e.ExceptionObject}",
                    EventLogEntryType.Error);
            }

            if (!e.IsTerminating)
            {
                return;
            }

            if (_lastRestartDelay < _maxRestartDelay)
            {
                _lastRestartDelay = _lastRestartDelay + _lastRestartDelay;

                if (_lastRestartDelay > _maxRestartDelay)
                {
                    _lastRestartDelay = _maxRestartDelay;
                }
            }

            Thread.Sleep(_lastRestartDelay);

            StartSlideshow();
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

            Cursor.Show();
        }

        private void _pictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (!Settings.ShowFileNames || String.IsNullOrWhiteSpace(_fileName))
            {
                return;
            }

            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            string text = _fileName;

            SizeF textSize = e.Graphics.MeasureString(text, _font);
            PointF locationToDraw = new PointF
            {
                X = _pictureBox.Width / 2 - textSize.Width / 2,
                Y = _pictureBox.Height - (int) (textSize.Height * 1.5)
            };

            e.Graphics.DrawString(text, _font, _brush, locationToDraw);
        }
    }
}
