using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace ConsoleLib
{
    public class ScreenBuffer : IDisposable
    {
        /* 
        TODO: From System.Console
        
        public static void Write    (bool value);
        public static void WriteLine(bool value);
        public static void Write    (decimal value);
        public static void WriteLine(decimal value);
        public static void Write    (double value);
        public static void WriteLine(double value);
        public static void Write    (float value);
        public static void WriteLine(float value);
        public static void Write    (int value);
        public static void WriteLine(int value);
        public static void Write    (long value);
        public static void WriteLine(long value);
        public static void Write    (object value);
        public static void WriteLine(object value);
        public static void Write    (uint value);
        public static void WriteLine(uint value);
        public static void Write    (ulong value);
        public static void WriteLine(ulong value);

        public static void Write    (string format, params object[] arg);
        public static void WriteLine(string format, params object[] arg);
        public static void Write    (string format, object arg0);
        public static void WriteLine(string format, object arg0);
        public static void Write    (string format, object arg0, object arg1);
        public static void WriteLine(string format, object arg0, object arg1);
        public static void Write    (string format, object arg0, object arg1, object arg2);
        public static void WriteLine(string format, object arg0, object arg1, object arg2);
        public static void Write    (string format, object arg0, object arg1, object arg2, object arg3);
        public static void WriteLine(string format, object arg0, object arg1, object arg2, object arg3);
         
        public static void Write    (char[] buffer, int index, int count);
        public static void WriteLine(char[] buffer, int index, int count);
         */

        Microsoft.Win32.SafeHandles.SafeFileHandle BufferHandle;
        bool FreeHandle;
        CharacterAttribute OriginalAttributes;
        System.IO.TextWriter OutputStream = null;

        public ScreenBuffer()
        {
            FreeHandle = true;
            
            BufferHandle = WinAPI.CreateConsoleScreenBuffer(WinAPI.DesiredAccess.GENERIC_READ | WinAPI.DesiredAccess.GENERIC_WRITE, WinAPI.ShareMode.FILE_SHARE_READ | WinAPI.ShareMode.FILE_SHARE_WRITE,
                                                            IntPtr.Zero, WinAPI.ConsoleFlags.TextModeBuffer, IntPtr.Zero);

            if (BufferHandle.IsInvalid)
            {
                throw new ConsoleExException("ConsoleEx: Unable to create a screen buffer.");
            }

            OriginalAttributes = Attribute;
        }

        public ScreenBuffer(ScreenBuffer CloneFrom, bool CopyContents = false) : this()
        {
            Mode = CloneFrom.Mode;            
            WinAPI.ConsoleScreenBufferInfoEx TempBufferInfo = CloneFrom.ScreenBufferInfo;
            
            // When setting the screen buffer info the window right/bottom is total cell count, not last
            TempBufferInfo.Window.Right++;
            TempBufferInfo.Window.Bottom++;
            
            ScreenBufferInfo = TempBufferInfo;

            CursorSize = CloneFrom.CursorSize;
            CursorVisible = CloneFrom.CursorVisible;

            CursorLeft = CloneFrom.CursorLeft;
            CursorTop = CloneFrom.CursorTop;

            if (CopyContents)
            {
                // ReadConsoleOutput has a soft limited of 64K of data, but can go lower depending on heap usage.
                // Because of that we will limit ourselves to 32k per a copy, which works out to 8192 CharInfos.
                // So we will calculate the best size of buffer to fit into that.
                short BufferBlockWidth = TempBufferInfo.BufferSize.X;
                short BufferBlockHeight = (short)(0x2000 / TempBufferInfo.BufferSize.X);

                CharInfo[,] Buffer = new CharInfo[BufferBlockHeight, BufferBlockWidth];
                WinAPI.Coord BufferSize = new WinAPI.Coord(BufferBlockWidth, BufferBlockHeight);                
                WinAPI.Coord BufferPos = new WinAPI.Coord(0, 0);
                WinAPI.SmallRect Region = new WinAPI.SmallRect(0, 0, BufferSize.X - 1, BufferBlockHeight - 1);

                while (Region.Top <= TempBufferInfo.BufferSize.Y)
                {
                    if (!WinAPI.ReadConsoleOutput(CloneFrom.BufferHandle, Buffer, BufferSize, BufferPos, ref Region))
                    {
                        throw new ConsoleExException("ConsoleEx: Unable to read source screen buffer.");
                    }

                    if (!WinAPI.WriteConsoleOutput(BufferHandle, Buffer, BufferSize, BufferPos, ref Region))
                    {
                        throw new ConsoleExException("ConsoleEx: Unable to write to screen buffer.");
                    }

                    Region.Top += BufferBlockHeight;
                    Region.Bottom += BufferBlockHeight;
                }
            }

            OriginalAttributes = Attribute;
        }


        public ScreenBuffer(Microsoft.Win32.SafeHandles.SafeFileHandle Handle)
        {
            // If we're being attached to an existing handle, sanity check that it's something we can work with.
            if (WinAPI.GetFileType(Handle) != WinAPI.FileTypes.Character)
            {
                throw new Exception("Invalid handle type for Output Buffer.");
            }

            // Flag that we're attached to the handle, so we don't accidently free it.            
            FreeHandle = false;
            BufferHandle = Handle;

            OriginalAttributes = Attribute;
        }

        static internal ScreenBuffer OpenCurrentScreenBuffer()
        {
            Microsoft.Win32.SafeHandles.SafeFileHandle Handle = ConsoleEx.GetConsoleOutputHandle();

            if (Handle.IsInvalid)
                return null;
            
            ScreenBuffer Ret = new ScreenBuffer(Handle);
            
            // The buffer actually owns this handle, so make sure we free it.
            Ret.FreeHandle = true;

            return Ret;
        }

        #region Dispose/Finaize

        ~ScreenBuffer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool Disposing)
        {
            if (FreeHandle && !BufferHandle.IsClosed)
                BufferHandle.Close();
        }
        
        #endregion

        #region Properties

        internal Microsoft.Win32.SafeHandles.SafeFileHandle Handle
        {
            get { return BufferHandle; }
        }

        // A stream for this screen buffer
        public System.IO.TextWriter Stream
        {
            get
            {
                if (OutputStream == null)
                {
                    System.IO.StreamWriter Stream = new System.IO.StreamWriter(new WinAPI.ConsoleStream(BufferHandle, System.IO.FileAccess.Write), ConsoleEx.OutputEncoding);
                    Stream.AutoFlush = true;
                    
                    OutputStream = Stream;
                }

                return OutputStream;
            }
        }

        public ConsoleExOutputMode Mode
        {
            get
            {
                ConsoleExOutputMode Ret;
                if (!WinAPI.GetConsoleOutputMode(BufferHandle, out Ret))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to get input mode.");
                }
                return Ret;
            }
            set
            {
                if (!WinAPI.SetConsoleOutputMode(BufferHandle, value))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set input mode.");
                }
            }
        }

        public int LargestWindowHeight
        {
            get
            {
                WinAPI.Coord Size = WinAPI.GetLargestConsoleWindowSize(BufferHandle);

                return Size.Y;
            }
        }

        public int LargestWindowWidth
        {
            get
            {
                WinAPI.Coord Size = WinAPI.GetLargestConsoleWindowSize(BufferHandle);

                return Size.X;
            }
        }

        public ConsoleColor BackgroundColor
        {
            get
            {
                return Attribute.Background;
            }
            set
            {
                CharacterAttribute v = Attribute;
                v.Background = value;
                Attribute = v;
            }
        }

        public ConsoleColor ForegroundColor
        {
            get
            {
                return Attribute.Foreground;
            }
            set
            {
                CharacterAttribute v = Attribute;
                v.Foreground = value;
                Attribute = v;
            }
        }

        public int CursorLeft
        {
            get
            {
                return ScreenBufferInfo.CursorPosition.X;
            }
            set
            {
                WinAPI.Coord Pos = ScreenBufferInfo.CursorPosition;
                Pos.X = (short)value;

                if (!WinAPI.SetConsoleCursorPosition(BufferHandle, Pos))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set cursor position.");
                }
            }
        }

        public int CursorTop
        {
            get
            {
                return ScreenBufferInfo.CursorPosition.Y;
            }
            set
            {
                WinAPI.Coord Pos = ScreenBufferInfo.CursorPosition;
                Pos.Y = (short)value;

                if (!WinAPI.SetConsoleCursorPosition(BufferHandle, Pos))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set cursor position.");
                }
            }
        }

        public int CursorSize
        {
            get
            {
                WinAPI.ConsoleCursorInfo Info;
                if (!WinAPI.GetConsoleCursorInfo(BufferHandle, out Info))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to get cursor information.");
                }

                return (int)Info.Size;
            }
            set
            {
                WinAPI.ConsoleCursorInfo Info;
                if (!WinAPI.GetConsoleCursorInfo(BufferHandle, out Info))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to get cursor information.");
                }

                Info.Size = (uint)value;
                if (!WinAPI.SetConsoleCursorInfo(BufferHandle, ref Info))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set cursor information.");
                }
            }
        }

        public bool CursorVisible
        {
            get
            {
                WinAPI.ConsoleCursorInfo Info;
                if (!WinAPI.GetConsoleCursorInfo(BufferHandle, out Info))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to get cursor information.");
                }

                return Info.Visable;
            }
            set
            {
                WinAPI.ConsoleCursorInfo Info;
                if (!WinAPI.GetConsoleCursorInfo(BufferHandle, out Info))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to get cursor information.");
                }

                Info.Visable = value;
                if (!WinAPI.SetConsoleCursorInfo(BufferHandle, ref Info))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set cursor information.");
                }
            }
        }
        
        
        public int WindowHeight
        {
            get
            {
                return ScreenBufferInfo.Window.Height;
            }
            set
            {
                WinAPI.SmallRect Size = ScreenBufferInfo.Window;

                Size.Height = value;
                if (!WinAPI.SetConsoleWindowInfo(BufferHandle, true, ref Size))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set Console Window Info.");
                }
            }
        }

        public int WindowWidth
        {
            get
            {
                return ScreenBufferInfo.Window.Width;
            }
            set
            {
                WinAPI.SmallRect Size = ScreenBufferInfo.Window;

                Size.Width = value;
                if (!WinAPI.SetConsoleWindowInfo(BufferHandle, true, ref Size))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set Console Window Info.");
                }
            }
        }
        
        public int WindowLeft
        {
            get
            {
                return ScreenBufferInfo.Window.Left;
            }
            set
            {
                WinAPI.SmallRect Size = ScreenBufferInfo.Window;
                int OldWidth = Size.Width;

                Size.Left = (short)value;
                Size.Width = OldWidth;
                if (!WinAPI.SetConsoleWindowInfo(BufferHandle, true, ref Size))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set Console Window Info.");
                }
            }
        }

        public int WindowTop
        {
            get
            {
                return ScreenBufferInfo.Window.Top;
            }
            set
            {
                WinAPI.SmallRect Size = ScreenBufferInfo.Window;
                int OldHeigh = Size.Height;

                Size.Top = (short)value;
                Size.Height = OldHeigh;
                if (!WinAPI.SetConsoleWindowInfo(BufferHandle, true, ref Size))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set Console Window Info.");
                }
            }
        }

        public int BufferHeight
        {
            get
            {
                return ScreenBufferInfo.BufferSize.Y;
            }
            set
            {
                WinAPI.Coord Size = ScreenBufferInfo.BufferSize;
                Size.Y = (short)value;

                if (!WinAPI.SetConsoleScreenBufferSize(BufferHandle, Size))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set Buffer Size.");
                }
            }
        }

        public int BufferWidth
        {
            get
            {
                return ScreenBufferInfo.BufferSize.X;
            }
            set
            {
                WinAPI.Coord Size = ScreenBufferInfo.BufferSize;
                Size.X = (short)value;

                if (!WinAPI.SetConsoleScreenBufferSize(BufferHandle, Size))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set Buffer Size.");
                }
            }
        }
        
        internal WinAPI.ConsoleScreenBufferInfoEx ScreenBufferInfo
        {
            get
            {
                WinAPI.ConsoleScreenBufferInfoEx Info = default(WinAPI.ConsoleScreenBufferInfoEx);
                Info.SetSize();

                if (!WinAPI.GetConsoleScreenBufferInfoEx(BufferHandle, ref Info))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to get screen buffer info.");
                }

                return Info;
            }

            set
            {
                if (!WinAPI.SetConsoleScreenBufferInfoEx(BufferHandle, ref value))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set screen buffer info.");
                }
            }
        }

        internal CharacterAttribute Attribute
        {
            get
            {
                CharacterAttribute ret = ScreenBufferInfo.Attributes;

                return ret;
            }
            
            set
            {
                if (!WinAPI.SetConsoleTextAttribute(BufferHandle, value))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set attributes.");
                }
            }
        }

        public bool ProcessedOutput
        {
            get
            {
                return (Mode & ConsoleExOutputMode.ProcessedOutput) == ConsoleExOutputMode.ProcessedOutput;
            }

            set
            {
                if (value)
                    Mode |= ConsoleExOutputMode.ProcessedOutput;
                else
                    Mode &= ~ConsoleExOutputMode.ProcessedOutput;
            }
        }
        
        public bool WrapAtEOL
        {
            get
            {
                return (Mode & ConsoleExOutputMode.WrapAtEOL) == ConsoleExOutputMode.WrapAtEOL;
            }

            set
            {
                if (value)
                    Mode |= ConsoleExOutputMode.WrapAtEOL;
                else
                    Mode &= ~ConsoleExOutputMode.WrapAtEOL;
            }
        }

        public bool VirtualTerminal
        {
            get
            {
                return (Mode & ConsoleExOutputMode.VirutalTerminalProcessing) == ConsoleExOutputMode.VirutalTerminalProcessing;
            }

            set
            {
                if (value)
                    Mode |= ConsoleExOutputMode.VirutalTerminalProcessing;
                else
                    Mode &= ~ConsoleExOutputMode.VirutalTerminalProcessing;
            }
        }

        public bool NewLineAutoReturn
        {
            get
            {
                return (Mode & ConsoleExOutputMode.DisableNewLineAutoReturn) != ConsoleExOutputMode.DisableNewLineAutoReturn;
            }

            set
            {
                if (value)
                    Mode &= ~ConsoleExOutputMode.DisableNewLineAutoReturn;
                else
                    Mode |= ConsoleExOutputMode.DisableNewLineAutoReturn;
            }
        }

        public bool LVBGridWoldwide
        {
            get
            {
                return (Mode & ConsoleExOutputMode.LVBGridWoldwide) == ConsoleExOutputMode.LVBGridWoldwide;
            }

            set
            {
                if (value)
                    Mode |= ConsoleExOutputMode.LVBGridWoldwide;
                else
                    Mode &= ~ConsoleExOutputMode.LVBGridWoldwide;
            }
        }
        
        #endregion

        #region Output Functions

        public void WriteLine()
        {
            Write("\r\n");
        }

        public void Write(char Data)
        {
            uint Len;
            if (!WinAPI.WriteConsole(BufferHandle, Data.ToString(), 1, out Len, IntPtr.Zero))
            {
                throw new ConsoleExException("ConsoleEx: Unable to write to screen buffer.");
            }
        }

        public void WriteLine(char Data)
        {
            Write(Data);
            WriteLine();
        }

        public void Write(char[] Data)
        {
            uint Len;
            if (!WinAPI.WriteConsole(BufferHandle, Data, (uint)Data.Length, out Len, IntPtr.Zero))
            {
                throw new ConsoleExException("ConsoleEx: Unable to write to screen buffer.");
            }
        }

        public void WriteLine(char[] Data)
        {
            Write(Data);
            WriteLine();
        }

        public void Write(string Data)
        {
            uint Len;
            if (!WinAPI.WriteConsole(BufferHandle, Data, (uint)Data.Length, out Len, IntPtr.Zero))
            {
                throw new ConsoleExException("ConsoleEx: Unable to write to screen buffer.");
            }
        }

        public void WriteLine(string Data)
        {
            Write(Data);
            WriteLine();
        }

        //---------------------------------------------------
        
        public int WritePos(char Buffer, int PosLeft, int PosTop)
        {
            char[] SmallBuffer = new char[1];
            return WritePos(SmallBuffer, PosLeft, PosTop);
        }

        public int WritePos(CharacterAttribute Buffer, int PosLeft, int PosTop)
        {
            CharacterAttribute[] SmallBuffer = new CharacterAttribute[1];
            return WritePos(SmallBuffer, PosLeft, PosTop);
        }

        public int WritePos(char[] Buffer, int PosLeft, int PosTop)
        {
            WinAPI.Coord BufferSize = new WinAPI.Coord(Buffer.GetLength(1), Buffer.GetLength(0));
            WinAPI.Coord BufferPos = new WinAPI.Coord(PosLeft, PosTop);

            uint WriteCount;
            if (!WinAPI.WriteConsoleOutputCharacter(BufferHandle, Buffer, (uint)Buffer.Length, BufferPos, out WriteCount))
            {
                throw new ConsoleExException("ConsoleEx: Unable to write to screen buffer.");
            }

            return (int)WriteCount;
        }

        public int WritePos(CharacterAttribute[] Buffer, int PosLeft, int PosTop)
        {
            WinAPI.Coord BufferSize = new WinAPI.Coord(Buffer.GetLength(1), Buffer.GetLength(0));
            WinAPI.Coord BufferPos = new WinAPI.Coord(PosLeft, PosTop);

            uint WriteCount;
            if (!WinAPI.WriteConsoleOutputAttribute(BufferHandle, Buffer, (uint)Buffer.Length, BufferPos, out WriteCount))
            {
                throw new ConsoleExException("ConsoleEx: Unable to write to screen buffer.");
            }

            return (int)WriteCount;
        }


        public int WriteBlock(CharInfo[,] Buffer, int PosLeft, int PosTop)
        {
            return WriteBlock(Buffer, 0, 0, PosLeft, PosTop, PosLeft + Buffer.GetLength(1) - 1, PosTop + Buffer.GetLength(0) - 1);
        }

        public int WriteBlock(CharInfo[,] Buffer, int BufferOffsetX, int BufferOffsetY, int PosLeft, int PosTop, int PosRight, int PosBottom)
        {
            WinAPI.Coord BufferSize = new WinAPI.Coord(Buffer.GetLength(1), Buffer.GetLength(0));
            WinAPI.Coord BufferPos = new WinAPI.Coord(BufferOffsetX, BufferOffsetY);

            WinAPI.SmallRect Region = new WinAPI.SmallRect(PosLeft, PosTop, PosRight, PosBottom);

            if (!WinAPI.WriteConsoleOutput(BufferHandle, Buffer, BufferSize, BufferPos, ref Region))
            {
                throw new ConsoleExException("ConsoleEx: Unable to read from screen buffer.");
            }

            return Region.Height * Region.Width;
        }
        
        #endregion

        #region Scrolling

        // Scrolls the whole buffer up a given number of lines, filling with blanks in the current colors
        public void Scroll(int LineCount = 1)
        {
            MoveBufferArea(0, LineCount, BufferWidth, BufferHeight - LineCount, 0, 0, ' ', Attribute.Foreground, Attribute.Background);
        }

        public void MoveBufferArea(int DataLeft, int DataTop, int DataWidth, int DataHeight, int DestX, int DestY, char FillChar = ' ')
        {
            MoveBufferArea(DataLeft, DataTop, DataWidth, DataHeight, DestX, DestY, FillChar, Attribute.Foreground, Attribute.Background);
        }

        public void MoveBufferArea(int DataLeft, int DataTop, int DataWidth, int DataHeight, int DestX, int DestY, char FillChar, ConsoleColor FillForground, ConsoleColor FillBackground)
        {
            WinAPI.SmallRect Source = new WinAPI.SmallRect(DataLeft, DataTop, DataLeft + DataWidth - 1, DataTop + DataHeight - 1);
            WinAPI.Coord Destination = new WinAPI.Coord(DestX, DestY);

            CharInfo Fill = new CharInfo(FillChar, new CharacterAttribute(FillForground, FillBackground));

            if (!WinAPI.ScrollConsoleScreenBuffer(BufferHandle, ref Source, IntPtr.Zero, Destination, ref Fill))
            {
                throw new ConsoleExException("ConsoleEx: Unable to move data in screen buffer.");
            }
        }

        public void MoveBufferArea(System.Drawing.Rectangle DataPos, System.Drawing.Rectangle ClippingPos, System.Drawing.Point DestinationLoc, char FillChar, ConsoleColor FillForground, ConsoleColor FillBackground)
        {
            WinAPI.SmallRect Source = new WinAPI.SmallRect(DataPos.Left, DataPos.Top, DataPos.Right, DataPos.Bottom);
            WinAPI.SmallRect Clipping = new WinAPI.SmallRect(ClippingPos.Left, ClippingPos.Top, ClippingPos.Right, ClippingPos.Bottom);
            WinAPI.Coord Destination = new WinAPI.Coord(DestinationLoc.X, DestinationLoc.Y);

            CharInfo Fill = new CharInfo(FillChar, new CharacterAttribute(FillForground, FillBackground));

            if (!WinAPI.ScrollConsoleScreenBufferWithClipping(BufferHandle, ref Source, ref Clipping, Destination, ref Fill))
            {
                throw new ConsoleExException("ConsoleEx: Unable to move data in screen buffer.");
            }
        }
        
        #endregion

        #region Reading Functions

        public int ReadPos(char Buffer, int PosLeft, int PosTop)
        {
            char[] SmallBuffer = new char[1];
            return ReadPos(SmallBuffer, PosLeft, PosTop);
        }

        public int ReadPos(CharacterAttribute Buffer, int PosLeft, int PosTop)
        {
            CharacterAttribute[] SmallBuffer = new CharacterAttribute[1];
            return ReadPos(SmallBuffer, PosLeft, PosTop);
        }

        public int ReadPos(char[] Buffer, int PosLeft, int PosTop)
        {
            WinAPI.Coord BufferSize = new WinAPI.Coord(Buffer.GetLength(1), Buffer.GetLength(0));
            WinAPI.Coord BufferPos = new WinAPI.Coord(PosLeft, PosTop);

            uint ReadCount;
            if (!WinAPI.ReadConsoleOutputCharacter(BufferHandle, Buffer, (uint)Buffer.Length, BufferPos, out ReadCount))
            {
                throw new ConsoleExException("ConsoleEx: Unable to read from screen buffer.");
            }

            return (int)ReadCount;
        }

        public int ReadPos(CharacterAttribute[] Buffer, int PosLeft, int PosTop)
        {
            WinAPI.Coord BufferSize = new WinAPI.Coord(Buffer.GetLength(1), Buffer.GetLength(0));
            WinAPI.Coord BufferPos = new WinAPI.Coord(PosLeft, PosTop);

            uint ReadCount;
            if (!WinAPI.ReadConsoleOutputAttribute(BufferHandle, Buffer, (uint)Buffer.Length, BufferPos, out ReadCount))
            {
                throw new ConsoleExException("ConsoleEx: Unable to read from screen buffer.");
            }

            return (int)ReadCount;
        }

        public int ReadPos(CharInfo[,] Buffer, int PosLeft, int PosTop)
        {
            return ReadPos(Buffer, 0, 0, PosLeft, PosTop, PosLeft + Buffer.GetLength(1) - 1, PosTop + Buffer.GetLength(0) - 1);
        }

        public int ReadPos(CharInfo[,] Buffer, int BufferOffsetX, int BufferOffsetY, int PosLeft, int PosTop, int PosRight, int PosBottom)
        {
            WinAPI.Coord BufferSize = new WinAPI.Coord(Buffer.GetLength(1), Buffer.GetLength(0));
            WinAPI.Coord BufferPos = new WinAPI.Coord(BufferOffsetX, BufferOffsetY);

            WinAPI.SmallRect Region = new WinAPI.SmallRect(PosLeft, PosTop, PosRight, PosBottom);

            if (!WinAPI.ReadConsoleOutput(BufferHandle, Buffer, BufferSize, BufferPos, ref Region))
            {
                throw new ConsoleExException("ConsoleEx: Unable to read from screen buffer.");
            }

            return Region.Height * Region.Width;
        }

        #endregion

        #region Utitily Functions

        public void Clear()
        {
            WinAPI.Coord Pos = new WinAPI.Coord(0, 0);

            uint FillTotal = (uint)(BufferHeight * BufferWidth);
            uint FillOutput = 0;
            WinAPI.FillConsoleOutputCharacter(BufferHandle, ' ', FillTotal, Pos, out FillOutput);
            WinAPI.FillConsoleOutputAttribute(BufferHandle, Attribute.Value, FillTotal, Pos, out FillOutput);
        }

        public void SetBufferSize(int width, int height)
        {
            WinAPI.Coord Size = new WinAPI.Coord(width, height);

            if (!WinAPI.SetConsoleScreenBufferSize(BufferHandle, Size))
            {
                throw new ConsoleExException("ConsoleEx: Unable to set Buffer Size.");
            }
        }

        public void SetCursorPosition(int left, int top)
        {
            WinAPI.Coord Pos = new WinAPI.Coord(left, top);

            if (!WinAPI.SetConsoleCursorPosition(BufferHandle, Pos))
            {
                throw new ConsoleExException("ConsoleEx: Unable to set cursor position.");
            }
        }

        public void SetWindowPosition(int left, int top)
        {
            WinAPI.SmallRect NewWindow = ScreenBufferInfo.Window;

            int OldWidth = NewWindow.Width;
            int OldHeight = NewWindow.Height;

            NewWindow.Left = (short)left;
            NewWindow.Top = (short)top;

            NewWindow.Width = OldWidth;
            NewWindow.Height = OldHeight;

            if (!WinAPI.SetConsoleWindowInfo(BufferHandle, true, ref NewWindow))
            {
                throw new ConsoleExException("ConsoleEx: Unable to set Console Window Info.");
            }
        }

        public void SetWindowSize(int width, int height)
        {
            WinAPI.SmallRect NewWindow = ScreenBufferInfo.Window;

            NewWindow.Height = height;
            NewWindow.Width = width;

            if (!WinAPI.SetConsoleWindowInfo(BufferHandle, true, ref NewWindow))
            {
                throw new ConsoleExException("ConsoleEx: Unable to set Console Window Info.");
            }

        }

        public void ResetColor()
        {
            Attribute = OriginalAttributes;
        }

        #endregion

    }
}
