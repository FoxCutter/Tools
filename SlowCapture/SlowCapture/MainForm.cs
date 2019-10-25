﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

// <div>Icons made by <a href="https://www.flaticon.com/authors/freepik" title="Freepik">Freepik</a> from <a href="https://www.flaticon.com/"             title="Flaticon">www.flaticon.com</a></div>

namespace SlowCapture
{
    public partial class MainForm : Form
    {
        CaptureOptions CaptureOptionsWindow = new CaptureOptions();
        SettingOptions SettingOptionsWindow = new SettingOptions();

        private string WindowName = "";
        private string WindowTitle = "";
        private bool MatchWindowTitle = false;

        private bool ResizeOutput = false;
        public int ResizeOutputHeight = 720;
        public int ResizeOutputWidth = 1024;

        public int CroppingTop = 0;
        public int CroppingBottom = 0;
        public int CroppingLeft = 0;
        public int CroppingRight = 0;

        public MainForm()
        {
            WindowName = "Winword";

            CroppingLeft = 0;
            CroppingRight = 0;
            CroppingBottom = 0;
            CroppingTop = 64;

            ResizeOutput = true;
            ResizeOutputHeight = 660;
            ResizeOutputWidth = 720;

            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            if(m.Msg == 0x111) // WM_COMMAND
            {
                int ID = m.WParam.ToInt32();
                if((ID & 0xFFFF) == 0x3000)
                {
                    CaptureOptionsWindow.WindowName = WindowName;
                    CaptureOptionsWindow.WindowTitle = WindowTitle;
                    CaptureOptionsWindow.MatchTitle = MatchWindowTitle;

                    if(CaptureOptionsWindow.ShowDialog(this) == DialogResult.OK)
                    {
                        WindowName = CaptureOptionsWindow.WindowName;
                        WindowTitle = CaptureOptionsWindow.WindowTitle;
                        MatchWindowTitle = CaptureOptionsWindow.MatchTitle;
                    }
                }
                else if ((ID & 0xFFFF) == 0x3001)
                {
                    SettingOptionsWindow.ResizeOutput = ResizeOutput;
                    SettingOptionsWindow.ResizeOutputHeight = ResizeOutputHeight;
                    SettingOptionsWindow.ResizeOutputWidth = ResizeOutputWidth;

                    SettingOptionsWindow.CroppingTop = CroppingTop;
                    SettingOptionsWindow.CroppingBottom = CroppingBottom;
                    SettingOptionsWindow.CroppingLeft = CroppingLeft;
                    SettingOptionsWindow.CroppingRight = CroppingRight;

                    if(SettingOptionsWindow.ShowDialog(this) == DialogResult.OK)
                    {
                        ResizeOutput = SettingOptionsWindow.ResizeOutput;
                        ResizeOutputHeight = SettingOptionsWindow.ResizeOutputHeight;
                        ResizeOutputWidth = SettingOptionsWindow.ResizeOutputWidth;

                        CroppingTop = SettingOptionsWindow.CroppingTop;
                        CroppingBottom = SettingOptionsWindow.CroppingBottom;
                        CroppingLeft = SettingOptionsWindow.CroppingLeft;
                        CroppingRight = SettingOptionsWindow.CroppingRight;
                    }
                }
            }

            base.WndProc(ref m);
        }

        System.Threading.Thread Updater;
        delegate void UpdateCaptureDelegate(Bitmap Image);
        UpdateCaptureDelegate UpdateCaptureCallback;

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Create some classic menus so they arn't in the capture.
            IntPtr MenuHandle = ExternalAPI.CreateMenu();
            ExternalAPI.AppendMenu(MenuHandle, 0, 0x3000, "Capture...");
            ExternalAPI.AppendMenu(MenuHandle, 0, 0x3001, "Settings...");
            ExternalAPI.SetMenu(this.Handle, MenuHandle);

            UpdateCaptureCallback += UpdateCapture;

            Updater = new System.Threading.Thread(new System.Threading.ThreadStart(UpdateThread));
            Updater.Start();
        }

        void UpdateCapture(Bitmap Image)
        {
            if (Image == null)
                return;

            int height = Image.Height;
            int width = Image.Width;

            if (SettingOptionsWindow.Visible)
            {
                height -= (SettingOptionsWindow.CroppingBottom + SettingOptionsWindow.CroppingTop);
                width -= (SettingOptionsWindow.CroppingLeft + SettingOptionsWindow.CroppingRight);
            }
            else
            {
                height -= (CroppingBottom + CroppingTop);
                width -= (CroppingLeft + CroppingRight);
            }

            if (width > 0 && height > 0)
            {
                if (PreviewScreen.Image != null)
                    PreviewScreen.Image.Dispose();

                try
                {
                    if (SettingOptionsWindow.Visible)
                    {
                        PreviewScreen.Image = Image.Clone(new Rectangle(SettingOptionsWindow.CroppingLeft, SettingOptionsWindow.CroppingTop, width, height), Image.PixelFormat);
                    }
                    else
                    {
                        PreviewScreen.Image = Image.Clone(new Rectangle(CroppingLeft, CroppingTop, width, height), Image.PixelFormat);
                    }
                }
                catch
                {
                    return;
                }

                if (SettingOptionsWindow.Visible)
                {
                    if (SettingOptionsWindow.ResizeOutput)
                    {
                        this.SetClientSizeCore(SettingOptionsWindow.ResizeOutputWidth, SettingOptionsWindow.ResizeOutputHeight);
                    }
                    else
                    {
                        this.SetClientSizeCore(width, height);
                    }
                }
                else
                {
                    if (ResizeOutput)
                    {
                        this.SetClientSizeCore(ResizeOutputWidth, ResizeOutputHeight);
                    }
                    else
                    {
                        this.SetClientSizeCore(width, height);
                    }
                }

                SettingOptionsWindow.WindowHeight = height;
                SettingOptionsWindow.WindowWidth = width;
            }

            Image.Dispose();
        }



        class EnumCallback
        {
            public string Name;
            public string Title;
            public bool MatchTitle;
            public IntPtr Handle;

            public bool EnumWindowsCallback(IntPtr hWnd, IntPtr lParam)
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
                ExternalAPI.CloseHandle(ProcessHandle);

                string ExeName = System.IO.Path.GetFileNameWithoutExtension(TempString.ToString());

                if (Name.Equals(ExeName, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (MatchTitle)
                    {
                        int size = ExternalAPI.GetWindowTextLength(hWnd);
                        if (size != 0)
                        {
                            TempString.EnsureCapacity(size + 1);
                            ExternalAPI.GetWindowText(hWnd, TempString, TempString.Capacity);

                            if(TempString.ToString() == Title)
                            {
                                Handle = hWnd;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        Handle = hWnd;
                        return false;
                    }
                }

                return true;
            }
        }

        EnumCallback FindProcessCallback = new EnumCallback();


        private IntPtr FindProcess (string ProcessName, string WindowTitle, bool MatchTitle)
        {
            FindProcessCallback.Name = ProcessName;
            FindProcessCallback.Title = WindowTitle;
            FindProcessCallback.MatchTitle = MatchTitle;

            FindProcessCallback.Handle = IntPtr.Zero;

            ExternalAPI.EnumWindows(new ExternalAPI.EnumWindowsProc(FindProcessCallback.EnumWindowsCallback), IntPtr.Zero);

            return FindProcessCallback.Handle;
        }

        public void UpdateThread()
        {
            while(true)
            {
                System.Threading.Thread.Sleep(0);

                IntPtr CaptureProcess = FindProcess(WindowName, WindowTitle, MatchWindowTitle);

                if (CaptureProcess == IntPtr.Zero)
                    continue;

                Bitmap Image = ExternalAPI.CaptureWindow(CaptureProcess);

                if(Image != null)
                    this.Invoke(UpdateCaptureCallback, Image);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Updater.Abort();
        }
    }
}