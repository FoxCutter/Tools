namespace SlowCapture
{
    partial class SettingOptions
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
            System.Windows.Forms.GroupBox groupBox3;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.GroupBox groupBox2;
            System.Windows.Forms.Label label11;
            System.Windows.Forms.Label label10;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            this.CroppingRightControl = new System.Windows.Forms.NumericUpDown();
            this.CroppingBottomControl = new System.Windows.Forms.NumericUpDown();
            this.CroppingTopControl = new System.Windows.Forms.NumericUpDown();
            this.CroppingLeftControl = new System.Windows.Forms.NumericUpDown();
            this.WindowHeightLabel = new System.Windows.Forms.Label();
            this.WindowWidthLabel = new System.Windows.Forms.Label();
            this.ResizeCaptureCheck = new System.Windows.Forms.CheckBox();
            this.AbortButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.ResizeWidthTextbox = new System.Windows.Forms.TextBox();
            this.ResizeHeightTextbox = new System.Windows.Forms.TextBox();
            groupBox3 = new System.Windows.Forms.GroupBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            groupBox2 = new System.Windows.Forms.GroupBox();
            label11 = new System.Windows.Forms.Label();
            label10 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CroppingRightControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CroppingBottomControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CroppingTopControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CroppingLeftControl)).BeginInit();
            groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(this.CroppingRightControl);
            groupBox3.Controls.Add(this.CroppingBottomControl);
            groupBox3.Controls.Add(this.CroppingTopControl);
            groupBox3.Controls.Add(this.CroppingLeftControl);
            groupBox3.Controls.Add(label1);
            groupBox3.Controls.Add(label2);
            groupBox3.Controls.Add(label3);
            groupBox3.Controls.Add(label4);
            groupBox3.Location = new System.Drawing.Point(12, 12);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(318, 108);
            groupBox3.TabIndex = 0;
            groupBox3.TabStop = false;
            groupBox3.Text = "Cropping";
            // 
            // CroppingRightControl
            // 
            this.CroppingRightControl.Location = new System.Drawing.Point(197, 48);
            this.CroppingRightControl.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.CroppingRightControl.Name = "CroppingRightControl";
            this.CroppingRightControl.Size = new System.Drawing.Size(67, 20);
            this.CroppingRightControl.TabIndex = 5;
            this.CroppingRightControl.ValueChanged += new System.EventHandler(this.CroppingRightControl_ValueChanged);
            // 
            // CroppingBottomControl
            // 
            this.CroppingBottomControl.Location = new System.Drawing.Point(124, 73);
            this.CroppingBottomControl.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.CroppingBottomControl.Name = "CroppingBottomControl";
            this.CroppingBottomControl.Size = new System.Drawing.Size(67, 20);
            this.CroppingBottomControl.TabIndex = 7;
            this.CroppingBottomControl.ValueChanged += new System.EventHandler(this.CroppingBottomControl_ValueChanged);
            // 
            // CroppingTopControl
            // 
            this.CroppingTopControl.Location = new System.Drawing.Point(124, 20);
            this.CroppingTopControl.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.CroppingTopControl.Name = "CroppingTopControl";
            this.CroppingTopControl.Size = new System.Drawing.Size(67, 20);
            this.CroppingTopControl.TabIndex = 1;
            this.CroppingTopControl.ValueChanged += new System.EventHandler(this.CroppingTopControl_ValueChanged);
            // 
            // CroppingLeftControl
            // 
            this.CroppingLeftControl.Location = new System.Drawing.Point(81, 46);
            this.CroppingLeftControl.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.CroppingLeftControl.Name = "CroppingLeftControl";
            this.CroppingLeftControl.Size = new System.Drawing.Size(67, 20);
            this.CroppingLeftControl.TabIndex = 3;
            this.CroppingLeftControl.ValueChanged += new System.EventHandler(this.CroppingLeftControl_ValueChanged);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(92, 22);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(26, 13);
            label1.TabIndex = 0;
            label1.Text = "Top";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(50, 48);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(25, 13);
            label2.TabIndex = 2;
            label2.Text = "Left";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(159, 48);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(32, 13);
            label3.TabIndex = 4;
            label3.Text = "Right";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(78, 75);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(40, 13);
            label4.TabIndex = 6;
            label4.Text = "Bottom";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(this.ResizeHeightTextbox);
            groupBox2.Controls.Add(this.ResizeWidthTextbox);
            groupBox2.Controls.Add(this.WindowHeightLabel);
            groupBox2.Controls.Add(this.WindowWidthLabel);
            groupBox2.Controls.Add(label11);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(this.ResizeCaptureCheck);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(label6);
            groupBox2.Location = new System.Drawing.Point(12, 135);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(318, 101);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Sizing";
            // 
            // WindowHeightLabel
            // 
            this.WindowHeightLabel.AutoSize = true;
            this.WindowHeightLabel.Location = new System.Drawing.Point(264, 71);
            this.WindowHeightLabel.Name = "WindowHeightLabel";
            this.WindowHeightLabel.Size = new System.Drawing.Size(25, 13);
            this.WindowHeightLabel.TabIndex = 7;
            this.WindowHeightLabel.Text = "720";
            // 
            // WindowWidthLabel
            // 
            this.WindowWidthLabel.AutoSize = true;
            this.WindowWidthLabel.Location = new System.Drawing.Point(264, 45);
            this.WindowWidthLabel.Name = "WindowWidthLabel";
            this.WindowWidthLabel.Size = new System.Drawing.Size(31, 13);
            this.WindowWidthLabel.TabIndex = 6;
            this.WindowWidthLabel.Text = "1280";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(181, 71);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(83, 13);
            label11.TabIndex = 4;
            label11.Text = "Window Height:";
            label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(184, 45);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(80, 13);
            label10.TabIndex = 5;
            label10.Text = "Window Width:";
            label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ResizeCaptureCheck
            // 
            this.ResizeCaptureCheck.AutoSize = true;
            this.ResizeCaptureCheck.Location = new System.Drawing.Point(13, 19);
            this.ResizeCaptureCheck.Name = "ResizeCaptureCheck";
            this.ResizeCaptureCheck.Size = new System.Drawing.Size(98, 17);
            this.ResizeCaptureCheck.TabIndex = 0;
            this.ResizeCaptureCheck.Text = "Resize Capture";
            this.ResizeCaptureCheck.UseVisualStyleBackColor = true;
            this.ResizeCaptureCheck.CheckedChanged += new System.EventHandler(this.ResizeCaptureCheck_CheckedChanged);
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(24, 45);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(35, 13);
            label5.TabIndex = 0;
            label5.Text = "Width";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(24, 71);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(38, 13);
            label6.TabIndex = 2;
            label6.Text = "Height";
            // 
            // AbortButton
            // 
            this.AbortButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.AbortButton.Location = new System.Drawing.Point(253, 245);
            this.AbortButton.Name = "AbortButton";
            this.AbortButton.Size = new System.Drawing.Size(77, 23);
            this.AbortButton.TabIndex = 3;
            this.AbortButton.Text = "Cancel";
            this.AbortButton.UseVisualStyleBackColor = true;
            this.AbortButton.Click += new System.EventHandler(this.AbortButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(163, 245);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(77, 23);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // ResizeWidthTextbox
            // 
            this.ResizeWidthTextbox.Location = new System.Drawing.Point(65, 42);
            this.ResizeWidthTextbox.MaxLength = 4;
            this.ResizeWidthTextbox.Name = "ResizeWidthTextbox";
            this.ResizeWidthTextbox.Size = new System.Drawing.Size(53, 20);
            this.ResizeWidthTextbox.TabIndex = 1;
            this.ResizeWidthTextbox.Text = "0";
            this.ResizeWidthTextbox.TextChanged += new System.EventHandler(this.ResizeWidthTextbox_TextChanged);
            this.ResizeWidthTextbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ResizeWidthTextbox_KeyPress);
            // 
            // ResizeHeightTextbox
            // 
            this.ResizeHeightTextbox.Location = new System.Drawing.Point(65, 68);
            this.ResizeHeightTextbox.MaxLength = 4;
            this.ResizeHeightTextbox.Name = "ResizeHeightTextbox";
            this.ResizeHeightTextbox.Size = new System.Drawing.Size(53, 20);
            this.ResizeHeightTextbox.TabIndex = 3;
            this.ResizeHeightTextbox.Text = "0";
            this.ResizeHeightTextbox.TextChanged += new System.EventHandler(this.ResizeHeightTextbox_TextChanged);
            this.ResizeHeightTextbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ResizeHeightTextbox_KeyPress);
            // 
            // SettingOptions
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.AbortButton;
            this.ClientSize = new System.Drawing.Size(346, 279);
            this.Controls.Add(this.AbortButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(groupBox3);
            this.Controls.Add(groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingOptions";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Setting Options";
            this.Load += new System.EventHandler(this.SettingOptions_Load);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CroppingRightControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CroppingBottomControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CroppingTopControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CroppingLeftControl)).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown CroppingRightControl;
        private System.Windows.Forms.NumericUpDown CroppingBottomControl;
        private System.Windows.Forms.NumericUpDown CroppingTopControl;
        private System.Windows.Forms.NumericUpDown CroppingLeftControl;
        private System.Windows.Forms.Label WindowHeightLabel;
        private System.Windows.Forms.Label WindowWidthLabel;
        private System.Windows.Forms.CheckBox ResizeCaptureCheck;
        private System.Windows.Forms.Button AbortButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.TextBox ResizeHeightTextbox;
        private System.Windows.Forms.TextBox ResizeWidthTextbox;
    }
}