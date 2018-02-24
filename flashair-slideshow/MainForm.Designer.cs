namespace flashair_slideshow
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this._pictureFolder = new System.Windows.Forms.TextBox();
            this._folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this._browseButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._startButton = new System.Windows.Forms.Button();
            this._minimumDisplayTime = new System.Windows.Forms.NumericUpDown();
            this._maximumDisplayTime = new System.Windows.Forms.NumericUpDown();
            this._showFilenames = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._minimumDisplayTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._maximumDisplayTime)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Picture folder";
            // 
            // _pictureFolder
            // 
            this._pictureFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._pictureFolder.Location = new System.Drawing.Point(140, 12);
            this._pictureFolder.Name = "_pictureFolder";
            this._pictureFolder.Size = new System.Drawing.Size(186, 20);
            this._pictureFolder.TabIndex = 1;
            // 
            // _browseButton
            // 
            this._browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._browseButton.Location = new System.Drawing.Point(332, 10);
            this._browseButton.Name = "_browseButton";
            this._browseButton.Size = new System.Drawing.Size(75, 23);
            this._browseButton.TabIndex = 2;
            this._browseButton.Text = "Browse";
            this._browseButton.UseVisualStyleBackColor = true;
            this._browseButton.Click += new System.EventHandler(this._browseButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Minimum display time (s)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Maximum display time (s)";
            // 
            // _startButton
            // 
            this._startButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._startButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._startButton.Location = new System.Drawing.Point(140, 122);
            this._startButton.Name = "_startButton";
            this._startButton.Size = new System.Drawing.Size(186, 49);
            this._startButton.TabIndex = 9;
            this._startButton.Text = "Start slideshow";
            this._startButton.UseVisualStyleBackColor = true;
            this._startButton.Click += new System.EventHandler(this._startButton_Click);
            // 
            // _minimumDisplayTime
            // 
            this._minimumDisplayTime.Location = new System.Drawing.Point(140, 38);
            this._minimumDisplayTime.Name = "_minimumDisplayTime";
            this._minimumDisplayTime.Size = new System.Drawing.Size(46, 20);
            this._minimumDisplayTime.TabIndex = 4;
            // 
            // _maximumDisplayTime
            // 
            this._maximumDisplayTime.Location = new System.Drawing.Point(140, 64);
            this._maximumDisplayTime.Name = "_maximumDisplayTime";
            this._maximumDisplayTime.Size = new System.Drawing.Size(46, 20);
            this._maximumDisplayTime.TabIndex = 6;
            // 
            // _showFilenames
            // 
            this._showFilenames.AutoSize = true;
            this._showFilenames.Location = new System.Drawing.Point(140, 91);
            this._showFilenames.Name = "_showFilenames";
            this._showFilenames.Size = new System.Drawing.Size(15, 14);
            this._showFilenames.TabIndex = 8;
            this._showFilenames.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Show filenames";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(140, 178);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(274, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Tip: Press the ESC key or ALT+F4 to stop the slideshow.";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 204);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this._showFilenames);
            this.Controls.Add(this._maximumDisplayTime);
            this.Controls.Add(this._minimumDisplayTime);
            this.Controls.Add(this._startButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._browseButton);
            this.Controls.Add(this._pictureFolder);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(435, 243);
            this.Name = "MainForm";
            this.Text = "flashair-slideshow";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this._minimumDisplayTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._maximumDisplayTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _pictureFolder;
        private System.Windows.Forms.FolderBrowserDialog _folderBrowserDialog;
        private System.Windows.Forms.Button _browseButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button _startButton;
        private System.Windows.Forms.NumericUpDown _minimumDisplayTime;
        private System.Windows.Forms.NumericUpDown _maximumDisplayTime;
        private System.Windows.Forms.CheckBox _showFilenames;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}

