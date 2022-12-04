namespace Prism2
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBoxImageToProcess = new System.Windows.Forms.PictureBox();
            this.groupBoxColor = new System.Windows.Forms.GroupBox();
            this.radioButtonColorAll = new System.Windows.Forms.RadioButton();
            this.radioButtonColorBlue = new System.Windows.Forms.RadioButton();
            this.radioButtonColorGreen = new System.Windows.Forms.RadioButton();
            this.radioButtonColorRed = new System.Windows.Forms.RadioButton();
            this.textBoxCurrentFile = new System.Windows.Forms.TextBox();
            this.groupBoxObject = new System.Windows.Forms.GroupBox();
            this.radioButtonObjectSquares = new System.Windows.Forms.RadioButton();
            this.radioButtonObjectCircles = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImageToProcess)).BeginInit();
            this.groupBoxColor.SuspendLayout();
            this.groupBoxObject.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenFile.Location = new System.Drawing.Point(680, 24);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(108, 23);
            this.buttonOpenFile.TabIndex = 0;
            this.buttonOpenFile.Text = "Open Image";
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pictureBoxImageToProcess);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(650, 531);
            this.panel1.TabIndex = 2;
            // 
            // pictureBoxImageToProcess
            // 
            this.pictureBoxImageToProcess.Location = new System.Drawing.Point(3, 0);
            this.pictureBoxImageToProcess.Name = "pictureBoxImageToProcess";
            this.pictureBoxImageToProcess.Size = new System.Drawing.Size(647, 543);
            this.pictureBoxImageToProcess.TabIndex = 3;
            this.pictureBoxImageToProcess.TabStop = false;
            // 
            // groupBoxColor
            // 
            this.groupBoxColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxColor.Controls.Add(this.radioButtonColorAll);
            this.groupBoxColor.Controls.Add(this.radioButtonColorBlue);
            this.groupBoxColor.Controls.Add(this.radioButtonColorGreen);
            this.groupBoxColor.Controls.Add(this.radioButtonColorRed);
            this.groupBoxColor.Location = new System.Drawing.Point(680, 55);
            this.groupBoxColor.Name = "groupBoxColor";
            this.groupBoxColor.Size = new System.Drawing.Size(108, 100);
            this.groupBoxColor.TabIndex = 3;
            this.groupBoxColor.TabStop = false;
            this.groupBoxColor.Text = "Color Detect";
            // 
            // radioButtonColorAll
            // 
            this.radioButtonColorAll.AutoSize = true;
            this.radioButtonColorAll.Location = new System.Drawing.Point(6, 76);
            this.radioButtonColorAll.Name = "radioButtonColorAll";
            this.radioButtonColorAll.Size = new System.Drawing.Size(36, 17);
            this.radioButtonColorAll.TabIndex = 3;
            this.radioButtonColorAll.TabStop = true;
            this.radioButtonColorAll.Text = "All";
            this.radioButtonColorAll.UseVisualStyleBackColor = true;
            this.radioButtonColorAll.CheckedChanged += new System.EventHandler(this.radioButtonColorAll_CheckedChanged);
            // 
            // radioButtonColorBlue
            // 
            this.radioButtonColorBlue.AutoSize = true;
            this.radioButtonColorBlue.Location = new System.Drawing.Point(6, 57);
            this.radioButtonColorBlue.Name = "radioButtonColorBlue";
            this.radioButtonColorBlue.Size = new System.Drawing.Size(46, 17);
            this.radioButtonColorBlue.TabIndex = 2;
            this.radioButtonColorBlue.TabStop = true;
            this.radioButtonColorBlue.Text = "Blue";
            this.radioButtonColorBlue.UseVisualStyleBackColor = true;
            this.radioButtonColorBlue.CheckedChanged += new System.EventHandler(this.radioButtonColorBlue_CheckedChanged);
            // 
            // radioButtonColorGreen
            // 
            this.radioButtonColorGreen.AutoSize = true;
            this.radioButtonColorGreen.Location = new System.Drawing.Point(6, 38);
            this.radioButtonColorGreen.Name = "radioButtonColorGreen";
            this.radioButtonColorGreen.Size = new System.Drawing.Size(54, 17);
            this.radioButtonColorGreen.TabIndex = 1;
            this.radioButtonColorGreen.TabStop = true;
            this.radioButtonColorGreen.Text = "Green";
            this.radioButtonColorGreen.UseVisualStyleBackColor = true;
            this.radioButtonColorGreen.CheckedChanged += new System.EventHandler(this.radioButtonColorGreen_CheckedChanged);
            // 
            // radioButtonColorRed
            // 
            this.radioButtonColorRed.AutoSize = true;
            this.radioButtonColorRed.Checked = true;
            this.radioButtonColorRed.Location = new System.Drawing.Point(6, 19);
            this.radioButtonColorRed.Name = "radioButtonColorRed";
            this.radioButtonColorRed.Size = new System.Drawing.Size(45, 17);
            this.radioButtonColorRed.TabIndex = 0;
            this.radioButtonColorRed.TabStop = true;
            this.radioButtonColorRed.Text = "Red";
            this.radioButtonColorRed.UseVisualStyleBackColor = true;
            this.radioButtonColorRed.CheckedChanged += new System.EventHandler(this.radioButtonColorRed_CheckedChanged);
            // 
            // textBoxCurrentFile
            // 
            this.textBoxCurrentFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCurrentFile.Location = new System.Drawing.Point(12, 548);
            this.textBoxCurrentFile.Name = "textBoxCurrentFile";
            this.textBoxCurrentFile.ReadOnly = true;
            this.textBoxCurrentFile.Size = new System.Drawing.Size(650, 20);
            this.textBoxCurrentFile.TabIndex = 4;
            // 
            // groupBoxObject
            // 
            this.groupBoxObject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxObject.Controls.Add(this.radioButtonObjectSquares);
            this.groupBoxObject.Controls.Add(this.radioButtonObjectCircles);
            this.groupBoxObject.Location = new System.Drawing.Point(680, 161);
            this.groupBoxObject.Name = "groupBoxObject";
            this.groupBoxObject.Size = new System.Drawing.Size(108, 62);
            this.groupBoxObject.TabIndex = 5;
            this.groupBoxObject.TabStop = false;
            this.groupBoxObject.Text = "Object Type";
            // 
            // radioButtonObjectSquares
            // 
            this.radioButtonObjectSquares.AutoSize = true;
            this.radioButtonObjectSquares.Location = new System.Drawing.Point(6, 38);
            this.radioButtonObjectSquares.Name = "radioButtonObjectSquares";
            this.radioButtonObjectSquares.Size = new System.Drawing.Size(64, 17);
            this.radioButtonObjectSquares.TabIndex = 1;
            this.radioButtonObjectSquares.Text = "Squares";
            this.radioButtonObjectSquares.UseVisualStyleBackColor = true;
            this.radioButtonObjectSquares.CheckedChanged += new System.EventHandler(this.radioButtonObjectSquares_CheckedChanged);
            // 
            // radioButtonObjectCircles
            // 
            this.radioButtonObjectCircles.AutoSize = true;
            this.radioButtonObjectCircles.Checked = true;
            this.radioButtonObjectCircles.Location = new System.Drawing.Point(6, 19);
            this.radioButtonObjectCircles.Name = "radioButtonObjectCircles";
            this.radioButtonObjectCircles.Size = new System.Drawing.Size(56, 17);
            this.radioButtonObjectCircles.TabIndex = 0;
            this.radioButtonObjectCircles.TabStop = true;
            this.radioButtonObjectCircles.Text = "Circles";
            this.radioButtonObjectCircles.UseVisualStyleBackColor = true;
            this.radioButtonObjectCircles.CheckedChanged += new System.EventHandler(this.radioButtonObjectCircles_CheckedChanged);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 570);
            this.Controls.Add(this.groupBoxObject);
            this.Controls.Add(this.textBoxCurrentFile);
            this.Controls.Add(this.groupBoxColor);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonOpenFile);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "Prism2: Detect Solid Circles and Squares By Color using EmguCV (Image<TColor, TDe" +
    "pth> Version)";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImageToProcess)).EndInit();
            this.groupBoxColor.ResumeLayout(false);
            this.groupBoxColor.PerformLayout();
            this.groupBoxObject.ResumeLayout(false);
            this.groupBoxObject.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBoxImageToProcess;
        private System.Windows.Forms.GroupBox groupBoxColor;
        private System.Windows.Forms.RadioButton radioButtonColorBlue;
        private System.Windows.Forms.RadioButton radioButtonColorGreen;
        private System.Windows.Forms.RadioButton radioButtonColorRed;
        private System.Windows.Forms.TextBox textBoxCurrentFile;
        private System.Windows.Forms.RadioButton radioButtonColorAll;
        private System.Windows.Forms.GroupBox groupBoxObject;
        private System.Windows.Forms.RadioButton radioButtonObjectSquares;
        private System.Windows.Forms.RadioButton radioButtonObjectCircles;
    }
}

