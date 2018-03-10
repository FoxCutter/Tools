using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLib
{
    public class ScreenBuffer : IDisposable
    {
        IntPtr BufferHandle = WinAPI.INVALID_HANDLE_VALUE;
        bool FreeHandle;

        public ScreenBuffer()
        {
            FreeHandle = true;
            
            BufferHandle = WinAPI.CreateConsoleScreenBuffer(WinAPI.DesiredAccess.GENERIC_READ | WinAPI.DesiredAccess.GENERIC_WRITE, WinAPI.ShareMode.FILE_SHARE_READ | WinAPI.ShareMode.FILE_SHARE_WRITE,
                                                            IntPtr.Zero, WinAPI.ConsoleFlags.TextModeBuffer, IntPtr.Zero);

            if (BufferHandle == WinAPI.INVALID_HANDLE_VALUE)
            {
                throw new ConsoleExException("ConsoleEx: Unable to create a screen buffer.");
            }
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
                CharInfo[,]Buffer = new CharInfo[BufferHeight, BufferWidth];
                WinAPI.Coord BufferSize = TempBufferInfo.BufferSize;
                WinAPI.Coord BufferPos = new WinAPI.Coord(0, 0);
                WinAPI.SmallRect Region = new WinAPI.SmallRect(0, 0, BufferWidth, BufferHeight);

                if (!WinAPI.ReadConsoleOutput(CloneFrom.BufferHandle, Buffer, BufferSize, BufferPos, ref Region))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to read source screen buffer.");
                }

                if (!WinAPI.WriteConsoleOutput(BufferHandle, Buffer, BufferSize, BufferPos, ref Region))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to write to screen buffer.");
                }
            }
        }

        
        public ScreenBuffer(IntPtr Handle)
        {
            // If we're being attached to an existing handle, sanity check that it's something we can work with.
            if (WinAPI.GetFileType(Handle) != WinAPI.FileTypes.Character)
            {
                throw new Exception("Invalid handle type for Output Buffer.");
            }

            // Flag that we're attached to the handle, so we don't accidently free it.            
            FreeHandle = false;
            BufferHandle = Handle;
        }

        static internal ScreenBuffer OpenCurrentScreenBuffer()
        {
            IntPtr Handle = ConsoleEx.GetConsoleOutputHandle();

            if (Handle == WinAPI.INVALID_HANDLE_VALUE)
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
            if (FreeHandle && BufferHandle != WinAPI.INVALID_HANDLE_VALUE)
                WinAPI.CloseHandle(BufferHandle);

            BufferHandle = WinAPI.INVALID_HANDLE_VALUE;

        }
        
        #endregion

        #region Properties

        public IntPtr Handle
        {
            get { return BufferHandle; }
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
                    throw new ConsoleExException("ConsoleEx: Unable to get input mode.");
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
        
        WinAPI.ConsoleScreenBufferInfoEx ScreenBufferInfo
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

        CharacterAttribute Attribute
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
        
        #endregion

        #region Output Functions

        public void Write(char Data)
        {
            uint Len;
            if (!WinAPI.WriteConsole(BufferHandle, Data.ToString(), 1, out Len, IntPtr.Zero))
            {
                throw new ConsoleExException("ConsoleEx: Unable to write to screen buffer.");
            }
        }

        public void Write(char[] Data)
        {
            uint Len;
            if (!WinAPI.WriteConsole(BufferHandle, Data, (uint)Data.Length, out Len, IntPtr.Zero))
            {
                throw new ConsoleExException("ConsoleEx: Unable to write to screen buffer.");
            }
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
            Write("\r\n");
        }

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

        public int WriteBlock(CharInfo[,] Buffer, int PosLeft, int PosTop)
        {
            return WriteBlock(Buffer, 0, 0, PosLeft, PosTop, PosLeft + Buffer.GetLength(1) - 1, PosTop + Buffer.GetLength(0) - 1);
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

        // Scrolls the whole buffer up a given number of lines, filling with blanks in the default colors
        public void Scroll(int LineCount = 1)
        {
            MoveBlock(0, LineCount, BufferWidth, BufferHeight, 0, 0, ' ', Attribute.Foreground, Attribute.Background);
        }

        public void MoveBlock(int DataLeft, int DataTop, int DataRight, int DataBottom, int DestX, int DestY, char FillChar = ' ')
        {
            MoveBlock(DataLeft, DataTop, DataRight, DataBottom, DestX, DestY, FillChar, Attribute.Foreground, Attribute.Background);
        }

        public void MoveBlock(int DataLeft, int DataTop, int DataRight, int DataBottom, int DestX, int DestY, char FillChar, ConsoleColor FillForground, ConsoleColor FillBackground)
        {
            WinAPI.SmallRect Source = new WinAPI.SmallRect(DataLeft, DataTop, DataRight, DataBottom);
            WinAPI.Coord Destination = new WinAPI.Coord(DestX, DestY);

            CharInfo Fill = new CharInfo(FillChar, new CharacterAttribute(FillForground, FillBackground));

            if (!WinAPI.ScrollConsoleScreenBuffer(BufferHandle, ref Source, IntPtr.Zero, Destination, ref Fill))
            {
                throw new ConsoleExException("ConsoleEx: Unable to move data in screen buffer.");
            }
        }

        public void MoveBlock(System.Drawing.Rectangle DataPos, System.Drawing.Rectangle ClippingPos, System.Drawing.Point DestinationLoc, char FillChar, ConsoleColor FillForground, ConsoleColor FillBackground)
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

        public int Read(char Buffer, int PosLeft, int PosTop)
        {
            char[] SmallBuffer = new char[1];
            return Read(SmallBuffer, PosLeft, PosTop);
        }
        
        public int Read(CharacterAttribute Buffer, int PosLeft, int PosTop)
        {
            CharacterAttribute[] SmallBuffer = new CharacterAttribute[1];
            return Read(SmallBuffer, PosLeft, PosTop);
        }

        public int ReadBlock(CharInfo[,] Buffer, int PosLeft, int PosTop)
        {
            return ReadBlock(Buffer, 0, 0, PosLeft, PosTop, PosLeft + Buffer.GetLength(1) - 1, PosTop + Buffer.GetLength(0) - 1);
        }
        
        public int Read(char[] Buffer, int PosLeft, int PosTop)
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

        public int Read(CharacterAttribute[] Buffer, int PosLeft, int PosTop)
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
               
        public int ReadBlock(CharInfo[,] Buffer, int BufferOffsetX, int BufferOffsetY, int PosLeft, int PosTop, int PosRight, int PosBottom)
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
    }
}
