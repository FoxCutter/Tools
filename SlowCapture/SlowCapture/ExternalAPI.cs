﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;


namespace SlowCapture
{
    public class ExternalAPI
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int x;
            public int y;
        }

        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        [DllImport("user32.dll")]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowLong(IntPtr hWnd, int Index);

        [DllImport("user32.dll")]
        public static extern IntPtr GetClientRect(IntPtr hWnd, ref Rect rect);

        [DllImport("user32.dll")]
        public static extern IntPtr ClientToScreen(IntPtr hWnd, ref Point point);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);


        [DllImport("user32.dll")]
        public static extern IntPtr CreateMenu();

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);

        [DllImport("user32.dll")]
        public static extern bool SetMenu(IntPtr hWnd, IntPtr hMenu);

        [DllImport("user32.dll")]
        public static extern bool AppendMenu(IntPtr hMenu, uint uFlags, uint uIDNewItem, string NewItem);

        [DllImport("Psapi.dll", CharSet = CharSet.Unicode)]
        public static extern int GetProcessImageFileName(IntPtr hProcess, StringBuilder lpImageFileName, int nSize);

        [DllImport("Psapi.dll", CharSet = CharSet.Unicode)]
        public static extern int GetModuleFileNameEx(IntPtr Process, IntPtr hModule, StringBuilder lpFilename, int nSize);

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        [DllImport("Kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        public static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        public static Bitmap CaptureWindow(IntPtr hWnd, CaptureMethod Method)
        {
            if (hWnd == IntPtr.Zero)
                return null;

            int style = ExternalAPI.GetWindowLong(hWnd, -16);
            //int Exstyle = ExternalAPI.GetWindowLong(hWnd, -20);

            // Ignore Popup windows 
            if ((style & 0x80000000) != 0)
                return null;

            var winrect = new ExternalAPI.Rect();
            ExternalAPI.GetWindowRect(hWnd, ref winrect);

            var clientrec = new ExternalAPI.Rect();
            ExternalAPI.GetClientRect(hWnd, ref clientrec);

            int width = clientrec.right - clientrec.left;
            int height = clientrec.bottom - clientrec.top;

            if (width <= 0 || height <= 0)
                return null;

            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                if (Method == CaptureMethod.PaintWindow)
                {
                    IntPtr Handle = graphics.GetHdc();

                    ExternalAPI.PrintWindow(hWnd, Handle, 1);

                    graphics.ReleaseHdc(Handle);
                }
                else if (Method == CaptureMethod.DesktopCapture)
                {
                    var tempPoint = new Point();
                    tempPoint.x = clientrec.left;
                    tempPoint.y = clientrec.top;
                    ClientToScreen(hWnd, ref tempPoint);

                    graphics.CopyFromScreen(tempPoint.x, tempPoint.y, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);
                }
            }

            return bmp;

        }
    }
}
