namespace Prism5
{
    partial class mainFrm
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
            CloseDownAndReleaseObjects();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainFrm));
            this.captureImageBox = new Emgu.CV.UI.ImageBox();
            this.buttonCapture = new System.Windows.Forms.Button();
            this.groupBoxAPI = new System.Windows.Forms.GroupBox();
            this.radioButtonAPI_DShow = new System.Windows.Forms.RadioButton();
            this.radioButtonAPI_WMF = new System.Windows.Forms.RadioButton();
            this.groupBoxScreenSize = new System.Windows.Forms.GroupBox();
            this.radioButtonSize_160x120 = new System.Windows.Forms.RadioButton();
            this.radioButtonSize_2034x1536 = new System.Windows.Forms.RadioButton();
            this.radioButtonSize_800x600 = new System.Windows.Forms.RadioButton();
            this.radioButtonSize_1920x1080 = new System.Windows.Forms.RadioButton();
            this.radioButtonSize_1280x720 = new System.Windows.Forms.RadioButton();
            this.radioButtonSize_640x480 = new System.Windows.Forms.RadioButton();
            this.groupBoxFramesPerSec = new System.Windows.Forms.GroupBox();
            this.radioButtonFPS_10 = new System.Windows.Forms.RadioButton();
            this.radioButtonFPS_30 = new System.Windows.Forms.RadioButton();
            this.radioButtonFPS_20 = new System.Windows.Forms.RadioButton();
            this.groupBoxOutput = new System.Windows.Forms.GroupBox();
            this.textBoxOutputFile = new System.Windows.Forms.TextBox();
            this.textBoxOutputDir = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonRecordToDisk = new System.Windows.Forms.Button();
            this.groupBoxCircleDetect = new System.Windows.Forms.GroupBox();
            this.radioButtonColorRed = new System.Windows.Forms.RadioButton();
            this.radioButtonColorBlue = new System.Windows.Forms.RadioButton();
            this.radioButtonColorAny = new System.Windows.Forms.RadioButton();
            this.radioButtonColorGreen = new System.Windows.Forms.RadioButton();
            this.textBoxNowRecording = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.captureImageBox)).BeginInit();
            this.groupBoxAPI.SuspendLayout();
            this.groupBoxScreenSize.SuspendLayout();
            this.groupBoxFramesPerSec.SuspendLayout();
            this.groupBoxOutput.SuspendLayout();
            this.groupBoxCircleDetect.SuspendLayout();
            this.SuspendLayout();
            // 
            // captureImageBox
            // 
            this.captureImageBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.captureImageBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.captureImageBox.Location = new System.Drawing.Point(3, 12);
            this.captureImageBox.Name = "captureImageBox";
            this.captureImageBox.Size = new System.Drawing.Size(640, 480);
            this.captureImageBox.TabIndex = 2;
            this.captureImageBox.TabStop = false;
            // 
            // buttonCapture
            // 
            this.buttonCapture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCapture.Location = new System.Drawing.Point(655, 374);
            this.buttonCapture.Name = "buttonCapture";
            this.buttonCapture.Size = new System.Drawing.Size(133, 23);
            this.buttonCapture.TabIndex = 3;
            this.buttonCapture.Text = "Start Capture";
            this.buttonCapture.UseVisualStyleBackColor = true;
            this.buttonCapture.Click += new System.EventHandler(this.buttonCapture_Click);
            // 
            // groupBoxAPI
            // 
            this.groupBoxAPI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxAPI.Controls.Add(this.radioButtonAPI_DShow);
            this.groupBoxAPI.Controls.Add(this.radioButtonAPI_WMF);
            this.groupBoxAPI.Location = new System.Drawing.Point(655, 12);
            this.groupBoxAPI.Name = "groupBoxAPI";
            this.groupBoxAPI.Size = new System.Drawing.Size(133, 56);
            this.groupBoxAPI.TabIndex = 4;
            this.groupBoxAPI.TabStop = false;
            this.groupBoxAPI.Text = "Backend API";
            // 
            // radioButtonAPI_DShow
            // 
            this.radioButtonAPI_DShow.AutoSize = true;
            this.radioButtonAPI_DShow.Location = new System.Drawing.Point(16, 33);
            this.radioButtonAPI_DShow.Name = "radioButtonAPI_DShow";
            this.radioButtonAPI_DShow.Size = new System.Drawing.Size(80, 17);
            this.radioButtonAPI_DShow.TabIndex = 2;
            this.radioButtonAPI_DShow.Text = "DirectShow";
            this.radioButtonAPI_DShow.UseVisualStyleBackColor = true;
            // 
            // radioButtonAPI_WMF
            // 
            this.radioButtonAPI_WMF.AutoSize = true;
            this.radioButtonAPI_WMF.Checked = true;
            this.radioButtonAPI_WMF.Location = new System.Drawing.Point(16, 17);
            this.radioButtonAPI_WMF.Name = "radioButtonAPI_WMF";
            this.radioButtonAPI_WMF.Size = new System.Drawing.Size(110, 17);
            this.radioButtonAPI_WMF.TabIndex = 1;
            this.radioButtonAPI_WMF.TabStop = true;
            this.radioButtonAPI_WMF.Text = "Media Foundation";
            this.radioButtonAPI_WMF.UseVisualStyleBackColor = true;
            // 
            // groupBoxScreenSize
            // 
            this.groupBoxScreenSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxScreenSize.Controls.Add(this.radioButtonSize_160x120);
            this.groupBoxScreenSize.Controls.Add(this.radioButtonSize_2034x1536);
            this.groupBoxScreenSize.Controls.Add(this.radioButtonSize_800x600);
            this.groupBoxScreenSize.Controls.Add(this.radioButtonSize_1920x1080);
            this.groupBoxScreenSize.Controls.Add(this.radioButtonSize_1280x720);
            this.groupBoxScreenSize.Controls.Add(this.radioButtonSize_640x480);
            this.groupBoxScreenSize.Location = new System.Drawing.Point(655, 72);
            this.groupBoxScreenSize.Name = "groupBoxScreenSize";
            this.groupBoxScreenSize.Size = new System.Drawing.Size(133, 125);
            this.groupBoxScreenSize.TabIndex = 5;
            this.groupBoxScreenSize.TabStop = false;
            this.groupBoxScreenSize.Text = "Screen Size";
            // 
            // radioButtonSize_160x120
            // 
            this.radioButtonSize_160x120.AutoSize = true;
            this.radioButtonSize_160x120.Location = new System.Drawing.Point(16, 19);
            this.radioButtonSize_160x120.Name = "radioButtonSize_160x120";
            this.radioButtonSize_160x120.Size = new System.Drawing.Size(66, 17);
            this.radioButtonSize_160x120.TabIndex = 6;
            this.radioButtonSize_160x120.Text = "160x120";
            this.radioButtonSize_160x120.UseVisualStyleBackColor = true;
            // 
            // radioButtonSize_2034x1536
            // 
            this.radioButtonSize_2034x1536.AutoSize = true;
            this.radioButtonSize_2034x1536.Location = new System.Drawing.Point(16, 104);
            this.radioButtonSize_2034x1536.Name = "radioButtonSize_2034x1536";
            this.radioButtonSize_2034x1536.Size = new System.Drawing.Size(78, 17);
            this.radioButtonSize_2034x1536.TabIndex = 5;
            this.radioButtonSize_2034x1536.Text = "2034x1536";
            this.radioButtonSize_2034x1536.UseVisualStyleBackColor = true;
            // 
            // radioButtonSize_800x600
            // 
            this.radioButtonSize_800x600.AutoSize = true;
            this.radioButtonSize_800x600.Location = new System.Drawing.Point(16, 53);
            this.radioButtonSize_800x600.Name = "radioButtonSize_800x600";
            this.radioButtonSize_800x600.Size = new System.Drawing.Size(66, 17);
            this.radioButtonSize_800x600.TabIndex = 4;
            this.radioButtonSize_800x600.Text = "800x600";
            this.radioButtonSize_800x600.UseVisualStyleBackColor = true;
            // 
            // radioButtonSize_1920x1080
            // 
            this.radioButtonSize_1920x1080.AutoSize = true;
            this.radioButtonSize_1920x1080.Location = new System.Drawing.Point(16, 87);
            this.radioButtonSize_1920x1080.Name = "radioButtonSize_1920x1080";
            this.radioButtonSize_1920x1080.Size = new System.Drawing.Size(78, 17);
            this.radioButtonSize_1920x1080.TabIndex = 3;
            this.radioButtonSize_1920x1080.Text = "1920x1080";
            this.radioButtonSize_1920x1080.UseVisualStyleBackColor = true;
            // 
            // radioButtonSize_1280x720
            // 
            this.radioButtonSize_1280x720.AutoSize = true;
            this.radioButtonSize_1280x720.Location = new System.Drawing.Point(16, 70);
            this.radioButtonSize_1280x720.Name = "radioButtonSize_1280x720";
            this.radioButtonSize_1280x720.Size = new System.Drawing.Size(72, 17);
            this.radioButtonSize_1280x720.TabIndex = 2;
            this.radioButtonSize_1280x720.Text = "1280x720";
            this.radioButtonSize_1280x720.UseVisualStyleBackColor = true;
            // 
            // radioButtonSize_640x480
            // 
            this.radioButtonSize_640x480.AutoSize = true;
            this.radioButtonSize_640x480.Checked = true;
            this.radioButtonSize_640x480.Location = new System.Drawing.Point(16, 36);
            this.radioButtonSize_640x480.Name = "radioButtonSize_640x480";
            this.radioButtonSize_640x480.Size = new System.Drawing.Size(66, 17);
            this.radioButtonSize_640x480.TabIndex = 1;
            this.radioButtonSize_640x480.TabStop = true;
            this.radioButtonSize_640x480.Text = "640x480";
            this.radioButtonSize_640x480.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.radioButtonSize_640x480.UseVisualStyleBackColor = true;
            // 
            // groupBoxFramesPerSec
            // 
            this.groupBoxFramesPerSec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFramesPerSec.Controls.Add(this.radioButtonFPS_10);
            this.groupBoxFramesPerSec.Controls.Add(this.radioButtonFPS_30);
            this.groupBoxFramesPerSec.Controls.Add(this.radioButtonFPS_20);
            this.groupBoxFramesPerSec.Location = new System.Drawing.Point(655, 199);
            this.groupBoxFramesPerSec.Name = "groupBoxFramesPerSec";
            this.groupBoxFramesPerSec.Size = new System.Drawing.Size(133, 73);
            this.groupBoxFramesPerSec.TabIndex = 6;
            this.groupBoxFramesPerSec.TabStop = false;
            this.groupBoxFramesPerSec.Text = "Frames Per Sec";
            // 
            // radioButtonFPS_10
            // 
            this.radioButtonFPS_10.AutoSize = true;
            this.radioButtonFPS_10.Location = new System.Drawing.Point(16, 17);
            this.radioButtonFPS_10.Name = "radioButtonFPS_10";
            this.radioButtonFPS_10.Size = new System.Drawing.Size(37, 17);
            this.radioButtonFPS_10.TabIndex = 9;
            this.radioButtonFPS_10.Text = "10";
            this.radioButtonFPS_10.UseVisualStyleBackColor = true;
            // 
            // radioButtonFPS_30
            // 
            this.radioButtonFPS_30.AutoSize = true;
            this.radioButtonFPS_30.Location = new System.Drawing.Point(16, 51);
            this.radioButtonFPS_30.Name = "radioButtonFPS_30";
            this.radioButtonFPS_30.Size = new System.Drawing.Size(37, 17);
            this.radioButtonFPS_30.TabIndex = 8;
            this.radioButtonFPS_30.Text = "30";
            this.radioButtonFPS_30.UseVisualStyleBackColor = true;
            // 
            // radioButtonFPS_20
            // 
            this.radioButtonFPS_20.AutoSize = true;
            this.radioButtonFPS_20.Checked = true;
            this.radioButtonFPS_20.Location = new System.Drawing.Point(16, 34);
            this.radioButtonFPS_20.Name = "radioButtonFPS_20";
            this.radioButtonFPS_20.Size = new System.Drawing.Size(37, 17);
            this.radioButtonFPS_20.TabIndex = 7;
            this.radioButtonFPS_20.TabStop = true;
            this.radioButtonFPS_20.Text = "20";
            this.radioButtonFPS_20.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.radioButtonFPS_20.UseVisualStyleBackColor = true;
            // 
            // groupBoxOutput
            // 
            this.groupBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxOutput.Controls.Add(this.textBoxOutputFile);
            this.groupBoxOutput.Controls.Add(this.textBoxOutputDir);
            this.groupBoxOutput.Controls.Add(this.label2);
            this.groupBoxOutput.Controls.Add(this.label1);
            this.groupBoxOutput.Controls.Add(this.buttonRecordToDisk);
            this.groupBoxOutput.Location = new System.Drawing.Point(655, 403);
            this.groupBoxOutput.Name = "groupBoxOutput";
            this.groupBoxOutput.Size = new System.Drawing.Size(133, 121);
            this.groupBoxOutput.TabIndex = 7;
            this.groupBoxOutput.TabStop = false;
            this.groupBoxOutput.Text = "Record to Disk";
            // 
            // textBoxOutputFile
            // 
            this.textBoxOutputFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutputFile.Location = new System.Drawing.Point(9, 94);
            this.textBoxOutputFile.Name = "textBoxOutputFile";
            this.textBoxOutputFile.ReadOnly = true;
            this.textBoxOutputFile.Size = new System.Drawing.Size(117, 20);
            this.textBoxOutputFile.TabIndex = 8;
            // 
            // textBoxOutputDir
            // 
            this.textBoxOutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutputDir.Location = new System.Drawing.Point(9, 55);
            this.textBoxOutputDir.Name = "textBoxOutputDir";
            this.textBoxOutputDir.ReadOnly = true;
            this.textBoxOutputDir.Size = new System.Drawing.Size(117, 20);
            this.textBoxOutputDir.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Output File";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Output Dir";
            // 
            // buttonRecordToDisk
            // 
            this.buttonRecordToDisk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRecordToDisk.Location = new System.Drawing.Point(6, 15);
            this.buttonRecordToDisk.Name = "buttonRecordToDisk";
            this.buttonRecordToDisk.Size = new System.Drawing.Size(120, 23);
            this.buttonRecordToDisk.TabIndex = 4;
            this.buttonRecordToDisk.Text = "Start Recording";
            this.buttonRecordToDisk.UseVisualStyleBackColor = true;
            this.buttonRecordToDisk.Click += new System.EventHandler(this.buttonRecordToDisk_Click);
            // 
            // groupBoxCircleDetect
            // 
            this.groupBoxCircleDetect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxCircleDetect.Controls.Add(this.radioButtonColorRed);
            this.groupBoxCircleDetect.Controls.Add(this.radioButtonColorBlue);
            this.groupBoxCircleDetect.Controls.Add(this.radioButtonColorAny);
            this.groupBoxCircleDetect.Controls.Add(this.radioButtonColorGreen);
            this.groupBoxCircleDetect.Location = new System.Drawing.Point(655, 278);
            this.groupBoxCircleDetect.Name = "groupBoxCircleDetect";
            this.groupBoxCircleDetect.Size = new System.Drawing.Size(133, 90);
            this.groupBoxCircleDetect.TabIndex = 8;
            this.groupBoxCircleDetect.TabStop = false;
            this.groupBoxCircleDetect.Text = "Circle Detect";
            // 
            // radioButtonColorRed
            // 
            this.radioButtonColorRed.AutoSize = true;
            this.radioButtonColorRed.Checked = true;
            this.radioButtonColorRed.Location = new System.Drawing.Point(16, 19);
            this.radioButtonColorRed.Name = "radioButtonColorRed";
            this.radioButtonColorRed.Size = new System.Drawing.Size(79, 17);
            this.radioButtonColorRed.TabIndex = 10;
            this.radioButtonColorRed.TabStop = true;
            this.radioButtonColorRed.Text = "Red Circles";
            this.radioButtonColorRed.UseVisualStyleBackColor = true;
            // 
            // radioButtonColorBlue
            // 
            this.radioButtonColorBlue.AutoSize = true;
            this.radioButtonColorBlue.Location = new System.Drawing.Point(16, 53);
            this.radioButtonColorBlue.Name = "radioButtonColorBlue";
            this.radioButtonColorBlue.Size = new System.Drawing.Size(80, 17);
            this.radioButtonColorBlue.TabIndex = 9;
            this.radioButtonColorBlue.Text = "Blue Circles";
            this.radioButtonColorBlue.UseVisualStyleBackColor = true;
            // 
            // radioButtonColorAny
            // 
            this.radioButtonColorAny.AutoSize = true;
            this.radioButtonColorAny.Location = new System.Drawing.Point(16, 70);
            this.radioButtonColorAny.Name = "radioButtonColorAny";
            this.radioButtonColorAny.Size = new System.Drawing.Size(77, 17);
            this.radioButtonColorAny.TabIndex = 8;
            this.radioButtonColorAny.Text = "Any Circles";
            this.radioButtonColorAny.UseVisualStyleBackColor = true;
            // 
            // radioButtonColorGreen
            // 
            this.radioButtonColorGreen.AutoSize = true;
            this.radioButtonColorGreen.Location = new System.Drawing.Point(16, 36);
            this.radioButtonColorGreen.Name = "radioButtonColorGreen";
            this.radioButtonColorGreen.Size = new System.Drawing.Size(88, 17);
            this.radioButtonColorGreen.TabIndex = 7;
            this.radioButtonColorGreen.Text = "Green Circles";
            this.radioButtonColorGreen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.radioButtonColorGreen.UseVisualStyleBackColor = true;
            // 
            // textBoxNowRecording
            // 
            this.textBoxNowRecording.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNowRecording.Location = new System.Drawing.Point(3, 498);
            this.textBoxNowRecording.Name = "textBoxNowRecording";
            this.textBoxNowRecording.ReadOnly = true;
            this.textBoxNowRecording.Size = new System.Drawing.Size(640, 20);
            this.textBoxNowRecording.TabIndex = 9;
            // 
            // mainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 527);
            this.Controls.Add(this.textBoxNowRecording);
            this.Controls.Add(this.groupBoxCircleDetect);
            this.Controls.Add(this.groupBoxOutput);
            this.Controls.Add(this.groupBoxFramesPerSec);
            this.Controls.Add(this.groupBoxScreenSize);
            this.Controls.Add(this.groupBoxAPI);
            this.Controls.Add(this.buttonCapture);
            this.Controls.Add(this.captureImageBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "mainFrm";
            this.Text = "Prism5: Camera Capture to Screen and Disk with Circle Recognition";
            ((System.ComponentModel.ISupportInitialize)(this.captureImageBox)).EndInit();
            this.groupBoxAPI.ResumeLayout(false);
            this.groupBoxAPI.PerformLayout();
            this.groupBoxScreenSize.ResumeLayout(false);
            this.groupBoxScreenSize.PerformLayout();
            this.groupBoxFramesPerSec.ResumeLayout(false);
            this.groupBoxFramesPerSec.PerformLayout();
            this.groupBoxOutput.ResumeLayout(false);
            this.groupBoxOutput.PerformLayout();
            this.groupBoxCircleDetect.ResumeLayout(false);
            this.groupBoxCircleDetect.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox captureImageBox;
        private System.Windows.Forms.Button buttonCapture;
        private System.Windows.Forms.GroupBox groupBoxAPI;
        private System.Windows.Forms.RadioButton radioButtonAPI_DShow;
        private System.Windows.Forms.RadioButton radioButtonAPI_WMF;
        private System.Windows.Forms.GroupBox groupBoxScreenSize;
        private System.Windows.Forms.RadioButton radioButtonSize_2034x1536;
        private System.Windows.Forms.RadioButton radioButtonSize_800x600;
        private System.Windows.Forms.RadioButton radioButtonSize_1920x1080;
        private System.Windows.Forms.RadioButton radioButtonSize_1280x720;
        private System.Windows.Forms.RadioButton radioButtonSize_640x480;
        private System.Windows.Forms.RadioButton radioButtonSize_160x120;
        private System.Windows.Forms.GroupBox groupBoxFramesPerSec;
        private System.Windows.Forms.RadioButton radioButtonFPS_10;
        private System.Windows.Forms.RadioButton radioButtonFPS_30;
        private System.Windows.Forms.RadioButton radioButtonFPS_20;
        private System.Windows.Forms.GroupBox groupBoxOutput;
        private System.Windows.Forms.Button buttonRecordToDisk;
        private System.Windows.Forms.TextBox textBoxOutputFile;
        private System.Windows.Forms.TextBox textBoxOutputDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxCircleDetect;
        private System.Windows.Forms.RadioButton radioButtonColorRed;
        private System.Windows.Forms.RadioButton radioButtonColorBlue;
        private System.Windows.Forms.RadioButton radioButtonColorAny;
        private System.Windows.Forms.RadioButton radioButtonColorGreen;
        private System.Windows.Forms.TextBox textBoxNowRecording;
    }

}