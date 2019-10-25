﻿namespace SlowCapture
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.PreviewScreen = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewScreen)).BeginInit();
            this.SuspendLayout();
            // 
            // PreviewScreen
            // 
            this.PreviewScreen.BackColor = System.Drawing.SystemColors.ControlDark;
            this.PreviewScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PreviewScreen.Location = new System.Drawing.Point(0, 0);
            this.PreviewScreen.Name = "PreviewScreen";
            this.PreviewScreen.Size = new System.Drawing.Size(456, 291);
            this.PreviewScreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PreviewScreen.TabIndex = 1;
            this.PreviewScreen.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(456, 291);
            this.Controls.Add(this.PreviewScreen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Slow Capture";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PreviewScreen)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox PreviewScreen;
    }
}

