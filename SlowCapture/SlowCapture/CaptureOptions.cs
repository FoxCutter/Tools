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
    public partial class CaptureOptions : Form
    {
        public bool MatchTitle { get; set; }
        public string WindowName { get; set; }
        public string WindowTitle { get; set; }
        public IntPtr WindowHandle { get; set; }

        public bool TopmostOnly { get; set; }
        public CaptureMethod Method { get; set; }

        public CaptureOptions()
        {
            InitializeComponent();
        }

        struct WindowData
        {
            public string Name;
            public string Title;
            public IntPtr Handle;

            public override string ToString()
            {
                return string.Format("[{0}] - {1}", Name, Title);
            }

        }

        private bool EnumWindowsCallback(IntPtr hWnd, IntPtr lParam)
        {
            int style = ExternalAPI.GetWindowLong(hWnd, -16);
            int Exstyle = ExternalAPI.GetWindowLong(hWnd, -20);

            if (!ExternalAPI.IsWindowVisible(hWnd))
                return true;

            // Ignore Popup windows
            if ((style & 0x80000000) != 0)
                return true;

            uint ProcessID;
            StringBuilder TempString = new StringBuilder();

            ExternalAPI.GetWindowThreadProcessId(hWnd, out ProcessID);

            IntPtr ProcessHandle = ExternalAPI.OpenProcess(0x0400, false, ProcessID);

            TempString.EnsureCapacity(1024);
            ExternalAPI.GetModuleFileNameEx(ProcessHandle, IntPtr.Zero, TempString, TempString.Capacity);

            if (TempString.ToString() == System.Reflection.Assembly.GetExecutingAssembly().Location)
                return true;

            WindowData Entry;
            Entry.Handle = hWnd;
            Entry.Name = System.IO.Path.GetFileNameWithoutExtension(TempString.ToString());

            ExternalAPI.CloseHandle(ProcessHandle);

            int size = ExternalAPI.GetWindowTextLength(hWnd);
            if (size != 0)
            {
                TempString.EnsureCapacity(size + 1);
                ExternalAPI.GetWindowText(hWnd, TempString, TempString.Capacity);

                Entry.Title = TempString.ToString();
            }
            else
            {
                Entry.Title = "";
            }

            WindowDropdown.Items.Add(Entry);

            return true;
        }

        private void CaptureOptions_Load(object sender, EventArgs e)
        {
            WindowDropdown.Items.Clear();
            WindowHandle = IntPtr.Zero;

            ExternalAPI.EnumWindows(new ExternalAPI.EnumWindowsProc(EnumWindowsCallback), IntPtr.Zero);

            foreach(WindowData Data in WindowDropdown.Items)
            {
                if(Data.Name.Equals(WindowName, StringComparison.InvariantCultureIgnoreCase))
                {
                    if(MatchTitle)
                    {
                        if (Data.Title != WindowTitle)
                            continue;
                    }

                    WindowDropdown.SelectedItem = Data;
                    break;
                }
            }

            MatchTitleCheck.Checked = MatchTitle;
            TopmostOnlyCheck.Checked = TopmostOnly;

            MethodDropdown.SelectedIndex = (int)Method;
        }

        private void MatchTitleCheck_CheckedChanged(object sender, EventArgs e)
        {
            MatchTitle = MatchTitleCheck.Checked;
        }

        private void WindowDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            WindowData Data = (WindowData)WindowDropdown.SelectedItem;

            WindowName = Data.Name;
            WindowTitle = Data.Title;
            WindowHandle = Data.Handle;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();

        }

        private void MethodDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            Method = (CaptureMethod)MethodDropdown.SelectedIndex;
        }

        private void TopmostOnlyCheck_CheckedChanged(object sender, EventArgs e)
        {
            TopmostOnly = TopmostOnlyCheck.Checked;
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (TopmostOnly && ExternalAPI.GetForegroundWindow() != WindowHandle)
                return;

            if (CapturePreview.Image != null)
                CapturePreview.Image.Dispose();

            CapturePreview.Image = ExternalAPI.CaptureWindow(WindowHandle, Method);
        }
    }
}
