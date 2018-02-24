using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using flashair_slideshow.Properties;

namespace flashair_slideshow
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void _browseButton_Click(object sender, EventArgs e)
        {
            _folderBrowserDialog.SelectedPath = _pictureFolder.Text;
            var result = _folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                _pictureFolder.Text = _folderBrowserDialog.SelectedPath;
            }
        }

        private void _startButton_Click(object sender, EventArgs e)
        {
            SaveSettings();

            StartSlideshow();
        }

        private void StartSlideshow()
        {
            var form = new PictureForm(Settings.Default);
            form.ShowDialog();
        }

        private void LoadSettings()
        {
            var settings = Settings.Default;

            _pictureFolder.Text = settings.PictureFolder;
            _minimumDisplayTime.Value = settings.MinimumDisplaySeconds;
            _maximumDisplayTime.Value = settings.MaximumDisplaySeconds;
            _showFilenames.Checked = settings.ShowFileNames;
        }


        private void SaveSettings()
        {
            var settings = Settings.Default;

            settings.PictureFolder = _pictureFolder.Text;
            settings.MinimumDisplaySeconds = (int) _minimumDisplayTime.Value;
            settings.MaximumDisplaySeconds = (int) _maximumDisplayTime.Value;
            settings.ShowFileNames = _showFilenames.Checked;

            settings.Save();
        }
    }
}
