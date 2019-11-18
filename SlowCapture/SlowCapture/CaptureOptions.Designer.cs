namespace SlowCapture
{
    partial class CaptureOptions
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            this.WindowDropdown = new System.Windows.Forms.ComboBox();
            this.MatchTitleCheck = new System.Windows.Forms.CheckBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.AbortButton = new System.Windows.Forms.Button();
            this.CapturePreview = new System.Windows.Forms.PictureBox();
            this.TopmostOnlyCheck = new System.Windows.Forms.CheckBox();
            this.MethodDropdown = new System.Windows.Forms.ComboBox();
            this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.CapturePreview)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(9, 228);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(46, 13);
            label1.TabIndex = 0;
            label1.Text = "Window";
            // 
            // WindowDropdown
            // 
            this.WindowDropdown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WindowDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WindowDropdown.FormattingEnabled = true;
            this.WindowDropdown.Location = new System.Drawing.Point(59, 225);
            this.WindowDropdown.Name = "WindowDropdown";
            this.WindowDropdown.Size = new System.Drawing.Size(429, 21);
            this.WindowDropdown.TabIndex = 1;
            this.WindowDropdown.SelectedIndexChanged += new System.EventHandler(this.WindowDropdown_SelectedIndexChanged);
            // 
            // MatchTitleCheck
            // 
            this.MatchTitleCheck.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MatchTitleCheck.AutoSize = true;
            this.MatchTitleCheck.Location = new System.Drawing.Point(59, 279);
            this.MatchTitleCheck.Name = "MatchTitleCheck";
            this.MatchTitleCheck.Size = new System.Drawing.Size(79, 17);
            this.MatchTitleCheck.TabIndex = 4;
            this.MatchTitleCheck.Text = "Match Title";
            this.MatchTitleCheck.UseVisualStyleBackColor = true;
            this.MatchTitleCheck.CheckedChanged += new System.EventHandler(this.MatchTitleCheck_CheckedChanged);
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKButton.Location = new System.Drawing.Point(315, 314);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(77, 23);
            this.OKButton.TabIndex = 6;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // AbortButton
            // 
            this.AbortButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AbortButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.AbortButton.Location = new System.Drawing.Point(405, 314);
            this.AbortButton.Name = "AbortButton";
            this.AbortButton.Size = new System.Drawing.Size(77, 23);
            this.AbortButton.TabIndex = 7;
            this.AbortButton.Text = "Cancel";
            this.AbortButton.UseVisualStyleBackColor = true;
            this.AbortButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // CapturePreview
            // 
            this.CapturePreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CapturePreview.BackColor = System.Drawing.SystemColors.ControlDark;
            this.CapturePreview.Location = new System.Drawing.Point(12, 12);
            this.CapturePreview.Name = "CapturePreview";
            this.CapturePreview.Size = new System.Drawing.Size(479, 207);
            this.CapturePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.CapturePreview.TabIndex = 4;
            this.CapturePreview.TabStop = false;
            // 
            // TopmostOnlyCheck
            // 
            this.TopmostOnlyCheck.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TopmostOnlyCheck.AutoSize = true;
            this.TopmostOnlyCheck.Location = new System.Drawing.Point(59, 302);
            this.TopmostOnlyCheck.Name = "TopmostOnlyCheck";
            this.TopmostOnlyCheck.Size = new System.Drawing.Size(91, 17);
            this.TopmostOnlyCheck.TabIndex = 5;
            this.TopmostOnlyCheck.Text = "Topmost Only";
            this.TopmostOnlyCheck.UseVisualStyleBackColor = true;
            this.TopmostOnlyCheck.CheckedChanged += new System.EventHandler(this.TopmostOnlyCheck_CheckedChanged);
            // 
            // label2
            // 
            label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 254);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(43, 13);
            label2.TabIndex = 2;
            label2.Text = "Method";
            // 
            // MethodDropdown
            // 
            this.MethodDropdown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MethodDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MethodDropdown.FormattingEnabled = true;
            this.MethodDropdown.Items.AddRange(new object[] {
            "Direct Capture",
            "Desktop Capture"});
            this.MethodDropdown.Location = new System.Drawing.Point(59, 251);
            this.MethodDropdown.Name = "MethodDropdown";
            this.MethodDropdown.Size = new System.Drawing.Size(429, 21);
            this.MethodDropdown.TabIndex = 3;
            this.MethodDropdown.SelectedIndexChanged += new System.EventHandler(this.MethodDropdown_SelectedIndexChanged);
            // 
            // UpdateTimer
            // 
            this.UpdateTimer.Enabled = true;
            this.UpdateTimer.Interval = 250;
            this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // CaptureOptions
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.AbortButton;
            this.ClientSize = new System.Drawing.Size(500, 344);
            this.Controls.Add(this.MethodDropdown);
            this.Controls.Add(label2);
            this.Controls.Add(this.TopmostOnlyCheck);
            this.Controls.Add(this.CapturePreview);
            this.Controls.Add(this.AbortButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(label1);
            this.Controls.Add(this.WindowDropdown);
            this.Controls.Add(this.MatchTitleCheck);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(512, 360);
            this.Name = "CaptureOptions";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Capture Options";
            this.Load += new System.EventHandler(this.CaptureOptions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CapturePreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox WindowDropdown;
        private System.Windows.Forms.CheckBox MatchTitleCheck;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button AbortButton;
        private System.Windows.Forms.PictureBox CapturePreview;
        private System.Windows.Forms.CheckBox TopmostOnlyCheck;
        private System.Windows.Forms.ComboBox MethodDropdown;
        private System.Windows.Forms.Timer UpdateTimer;
    }
}