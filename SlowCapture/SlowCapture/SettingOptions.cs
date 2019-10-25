using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlowCapture
{
    public partial class SettingOptions : Form
    {
        public ExternalAPI.Rect _Cropping;

        public bool ResizeOutput { get; set; }
        public int ResizeOutputHeight { get; set; }
        public int ResizeOutputWidth { get; set; }

        public int CroppingTop { get; set; }
        public int CroppingBottom { get; set; }
        public int CroppingLeft { get; set; }
        public int CroppingRight { get; set; }

        public int WindowHeight
        {
            set
            {
                WindowHeightLabel.Text = value.ToString();
            }
        }

        public int WindowWidth
        {
            set
            {
                WindowWidthLabel.Text = value.ToString();
            }
        }

        public SettingOptions()
        {
            ResizeOutput = false;
            ResizeOutputHeight = 0;
            ResizeOutputWidth = 0;

            CroppingTop = 0;
            CroppingBottom = 0;
            CroppingLeft = 0;
            CroppingRight = 0;

            InitializeComponent();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void AbortButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void SettingOptions_Load(object sender, EventArgs e)
        {
            CroppingTopControl.Value = CroppingTop;
            CroppingLeftControl.Value = CroppingLeft;
            CroppingBottomControl.Value = CroppingBottom;
            CroppingRightControl.Value = CroppingRight;

            ResizeCaptureCheck.Checked = ResizeOutput;
            ResizeWidthTextbox.Enabled = ResizeOutput;
            ResizeHeightTextbox.Enabled = ResizeOutput;

            ResizeWidthTextbox.Text = ResizeOutputWidth.ToString();
            ResizeHeightTextbox.Text = ResizeOutputHeight.ToString();
        }

        private void ResizeCaptureCheck_CheckedChanged(object sender, EventArgs e)
        {
            ResizeOutput = ResizeCaptureCheck.Checked;
            ResizeWidthTextbox.Enabled = ResizeOutput;
            ResizeHeightTextbox.Enabled = ResizeOutput;
        }

        private void CroppingTopControl_ValueChanged(object sender, EventArgs e)
        {
            CroppingTop = (int)CroppingTopControl.Value;
        }

        private void CroppingLeftControl_ValueChanged(object sender, EventArgs e)
        {
            CroppingLeft = (int)CroppingLeftControl.Value;
        }

        private void CroppingRightControl_ValueChanged(object sender, EventArgs e)
        {
            CroppingRight = (int)CroppingRightControl.Value;
        }

        private void CroppingBottomControl_ValueChanged(object sender, EventArgs e)
        {
            CroppingBottom = (int)CroppingBottomControl.Value;
        }

        private void ResizeWidthTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar));
        }

        private void ResizeWidthTextbox_TextChanged(object sender, EventArgs e)
        {
            int Temp;
            if (int.TryParse(ResizeWidthTextbox.Text, out Temp))
            {
                ResizeOutputWidth = Temp;
            }
        }

        private void ResizeHeightTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar));
        }

        private void ResizeHeightTextbox_TextChanged(object sender, EventArgs e)
        {
            int Temp;
            if (int.TryParse(ResizeHeightTextbox.Text, out Temp))
            {
                ResizeOutputHeight = Temp;
            }
        }
    }
}
