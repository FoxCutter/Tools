using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleLib
{
    public class WinAPI
    {
        #region Console Stream

        internal class ConsoleStream : System.IO.Stream
        {
            bool Readable;
            bool Writeable;
            Microsoft.Win32.SafeHandles.SafeFileHandle BufferHandle;

            public ConsoleStream(Microsoft.Win32.SafeHandles.SafeFileHandle Handle, System.IO.FileAccess Access)
            {
                BufferHandle = Handle;
                Readable = (Access & System.IO.FileAccess.Read) == System.IO.FileAccess.Read;
                Writeable = (Access & System.IO.FileAccess.Write) == System.IO.FileAccess.Write;
            }

            public override bool CanRead
            {
                get { return Readable; }
            }

            public override bool CanSeek
            {
                get { return false; }
            }

            public override bool CanWrite
            {
                get { return Writeable; }
            }

            public override void Flush()
            {
                if (Readable)
                {
                    if (!WinAPI.FlushConsoleInputBuffer(BufferHandle))
                    {
                        throw new ConsoleExException("ConsoleEx: Unable to flush input buffer.");
                    }
                }
            }

            public override long Length
            {
                get { throw new NotImplementedException(); }
            }

            public override long Position
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                if (!Readable)
                    return 0;
                
                byte[] Data = new byte[count];
                uint DataRead;

                //if (!WinAPI.ReadConsole(BufferHandle, Data, (uint)count / sizeof(char), out DataRead, IntPtr.Zero))
                if (!WinAPI.ReadFile(BufferHandle, Data, (uint)count, out DataRead, IntPtr.Zero))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to read from console buffer.");
                }

                Data.CopyTo(buffer, offset);

                //return (int)DataRead * sizeof(char);
                return (int)DataRead;
            }

            public override long Seek(long offset, System.IO.SeekOrigin origin)
            {
                throw new NotImplementedException();
            }

            public override void SetLength(long value)
            {
                throw new NotImplementedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                if (!Writeable)
                    return;

                byte[] Data = new byte[count];
                Array.Copy(buffer, offset, Data, 0, count);

                uint DataWritten;

                //if (!WinAPI.WriteConsole(BufferHandle, Data, (uint)count / 2, out DataWritten, IntPtr.Zero))
                if (!WinAPI.WriteFile(BufferHandle, Data, (uint)count, out DataWritten, IntPtr.Zero))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to write to console.");
                }
            }
        }

        #endregion

        #region FileAPIs

        public enum CreationDispositionType : uint
        {
		    CREATE_NEW          = 1,
		    CREATE_ALWAYS       = 2,
		    OPEN_EXISTING       = 3,
		    OPEN_ALWAYS         = 4,
		    TRUNCATE_EXISTING   = 5,
        }

        public enum FileTypes : uint
        {
            Unknown     = 0x0000,
            Disk        = 0x0001,
            Character   = 0x0002,
            Pipe        = 0x0003,
            Remote      = 0x8000,
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Microsoft.Win32.SafeHandles.SafeFileHandle CreateFile(string Filename, DesiredAccess dwDesiredAccess, ShareMode dwShareMode, IntPtr lpSecurityAttributes, CreationDispositionType CreationDisposition, uint FlagsAndAttributes, IntPtr TemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadFile(Microsoft.Win32.SafeHandles.SafeFileHandle hHandle, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] Buffer, uint NumberOfBytesToRead, out uint NumberOfBytesRead, IntPtr Reserved);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteFile(Microsoft.Win32.SafeHandles.SafeFileHandle hHandle, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] Buffer, uint NumberOfBytesToWrite, out uint NumberOfBytesWritten, IntPtr Reserved);

        [DllImport("kernel32.dll", SetLastError = true)]
        static public extern FileTypes GetFileType(Microsoft.Win32.SafeHandles.SafeFileHandle hObject);

        [DllImport("user32.dll")]
        static public extern short GetKeyState(int virtualKeyCode);

        [DllImport("kernel32.dll")]
        static public extern bool Beep(uint Freq, uint Duration);

        [Flags]
        public enum DesiredAccess : uint
        {
            GENERIC_READ        = 0x80000000,
            GENERIC_WRITE       = 0x40000000,
        }

        [Flags]
        public enum ShareMode : uint
        {
            FILE_SHARE_READ     = 0x00000001,
            FILE_SHARE_WRITE    = 0x00000002,
        }

        #endregion

        #region General Console Funtions

        [DllImport("Kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        // Size is a Byte Count
        public static extern uint GetConsoleTitle(StringBuilder ConsoleTitle, uint Size);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        // Size is a Character Count
        public static extern uint GetConsoleOriginalTitle(StringBuilder ConsoleTitle, uint Size);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool SetConsoleTitle(string ConsoleTitle);


        [DllImport("Kernel32.dll")]
        public static extern uint GetConsoleCP();

        [DllImport("Kernel32.dll")]
        public static extern uint GetConsoleOutputCP();

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleCP(uint CodePageID);

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleOutputCP(uint CodePageID);
        
        
        
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool AllocConsole();

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool FreeConsole();

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool AttachConsole(uint ProcessID);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern uint GetConsoleProcessList([Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] uint[] ProcessList, uint ProcessCount);


        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool AddConsoleAlias(string Source, string Target, string ExeName);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        // TargetBufferLength is a Byte Count
        public static extern uint GetConsoleAlias(string Source, StringBuilder TargetBuffer, uint TargetBufferLength, string ExeName);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        // AliasBufferLength is a Byte Count
        public static extern uint GetConsoleAliases([Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] char[] AliasBuffer, uint AliasBufferLength, string ExeName);

        [DllImport("Kernel32.dll")]
        // Return value is a Character count
        static public extern uint GetConsoleAliasesLength(string ExeName);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        // ExeNameBufferLength is a Byte Count
        public static extern uint GetConsoleAliasExes([Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] char[] ExeNameBuffer, uint ExeNameBufferLength);

        [DllImport("Kernel32.dll")]
        // Return value is a Character count
        static public extern uint GetConsoleAliasExesLength();


        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool GetConsoleHistoryInfo(ref ConsoleHistoryInfo ConsoleHistoryInfo);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool SetConsoleHistoryInfo(ref ConsoleHistoryInfo ConsoleHistoryInfo);


        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool GetConsoleSelectionInfo(out ConsoleSelectionInfo ConsoleSelectionInfo);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool SetConsoleCtrlHandler(CtrlHandlerRoutine HandlerRoutine, bool Add);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool GenerateConsoleCtrlEvent(ConsoleExCtrlEventType dwCtrlEvent, uint ProcessGroupId);


        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern IntPtr GetStdHandle(StdHandleType StdHandle);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool SetStdHandle(StdHandleType StdHandle, Microsoft.Win32.SafeHandles.SafeFileHandle Handle);

        
        [DllImport("kernel32.dll", SetLastError = true)]
        static public extern bool GetConsoleDisplayMode(out ConsoleExDisplayModeFlags ModeFlags);

        
        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool GetNumberOfConsoleMouseButtons([Out]out uint NumberOfMouseButtons);

        #endregion

        #region Screen Buffer Functions

        [DllImport("Kernel32.dll", SetLastError = true, EntryPoint = "GetConsoleMode")]
        static public extern bool GetConsoleOutputMode(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleHandle, out ConsoleExOutputMode Mode);

        [DllImport("Kernel32.dll", SetLastError = true, EntryPoint = "SetConsoleMode")]
        static public extern bool SetConsoleOutputMode(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleHandle, ConsoleExOutputMode Mode);

        
        [DllImport("kernel32.dll", SetLastError = true)]
        static public extern bool SetConsoleDisplayMode(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, ConsoleExDisplayModeFlags Flags, out Coord NewScreenBufferDimensions);

        
        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern Coord GetLargestConsoleWindowSize(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput);


        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern Microsoft.Win32.SafeHandles.SafeFileHandle CreateConsoleScreenBuffer(DesiredAccess DesiredAccess, ShareMode ShareMode, IntPtr SecurityAttributes, ConsoleFlags Flags, IntPtr ScreenBufferData);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool SetConsoleActiveScreenBuffer(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput);

        
        
        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool FillConsoleOutputAttribute(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, CharacterAttributeEnum Attribute, uint Length, Coord WriteCoord, out uint NumberOfAttrsWritten);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool FillConsoleOutputCharacter(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, char Character, uint Length, Coord WriteCoord, out uint NumberOfCharsWritten);


        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool GetConsoleCursorInfo(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, out ConsoleCursorInfo ConsoleCursorInfo);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool SetConsoleCursorInfo(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, ref ConsoleCursorInfo ConsoleCursorInfo);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool SetConsoleCursorPosition(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, Coord CurrentPosition);


        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool GetConsoleScreenBufferInfo(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, out ConsoleScreenBufferInfo ConsoleScreenBufferInfo);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool GetConsoleScreenBufferInfoEx(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, ref ConsoleScreenBufferInfoEx ConsoleScreenBufferInfoEx);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool SetConsoleScreenBufferInfoEx(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, ref ConsoleScreenBufferInfoEx ConsoleScreenBufferInfoEx);


        
        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool SetConsoleScreenBufferSize(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, Coord Size);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool SetConsoleTextAttribute(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, CharacterAttribute Attributes);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool SetConsoleWindowInfo(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, bool Absolute, ref SmallRect ConsoleWindow);


        
        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern Coord GetConsoleFontSize(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, uint Font);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool GetCurrentConsoleFont(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, bool MaximumWindow, out ConsoleFontInfo ConsoleCurrentFont);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool GetCurrentConsoleFontEx(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, bool MaximumWindow, ref ConsoleFontInfoEx ConsoleCurrentFontEx);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool SetCurrentConsoleFontEx(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, bool MaximumWindow, ref ConsoleFontInfoEx ConsoleCurrentFontEx);



        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool ReadConsoleOutput(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] CharInfo[,] Buffer, Coord BufferSize, Coord BufferCoord, ref SmallRect ReadRegion);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool ReadConsoleOutputAttribute(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] CharacterAttribute[] Attribute, uint Length, Coord ReadCoord, out uint NumberOfAttrsRead);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool ReadConsoleOutputCharacter(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] char[] Character, uint Length, Coord ReadCoord, out uint NumberOfCharsRead);


        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool WriteConsole(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, string Buffer, uint NumberOfCharsToWrite, out uint NumberOfCharsWritten, IntPtr Reserved);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool WriteConsole(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] char[] Buffer, uint NumberOfCharsToWrite, out uint NumberOfCharsWritten, IntPtr Reserved);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool WriteConsole(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, ref char Buffer, uint NumberOfCharsToWrite, out uint NumberOfCharsWritten, IntPtr Reserved);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool WriteConsoleOutput(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] CharInfo[,] Buffer, Coord BufferSize, Coord BufferCoord, ref SmallRect WriteRegion);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool WriteConsoleOutputAttribute(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] CharacterAttribute[] Attribute, uint Length, Coord WriteCoord, out uint NumberOfAttrsWriten);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool WriteConsoleOutputAttribute(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, ref CharacterAttribute Attribute, uint Length, Coord WriteCoord, out uint NumberOfAttrsWriten);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool WriteConsoleOutputCharacter(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] char[] Character, uint Length, Coord WriteCoord, out uint NumberOfCharsWriten);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool WriteConsoleOutputCharacter(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, ref char Character, uint Length, Coord WriteCoord, out uint NumberOfCharsWriten);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool ScrollConsoleScreenBuffer(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, ref SmallRect ScrollRectangle, IntPtr Reserved, Coord DestinationOrigin, ref CharInfo Fill);

        // Quick hack so we don't have to try to force a SMALL_REC into an IntPtr.
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "ScrollConsoleScreenBuffer")]
        static public extern bool ScrollConsoleScreenBufferWithClipping(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, ref SmallRect ScrollRectangle, ref SmallRect ClippingRectangle, Coord DestinationOrigin, ref CharInfo Fill);

        #endregion

        #region Input Buffer Functions

        [DllImport("Kernel32.dll", SetLastError = true, EntryPoint = "GetConsoleMode")]
        static public extern bool GetConsoleInputMode(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleHandle, out ConsoleExInputMode Mode);

        [DllImport("Kernel32.dll", SetLastError = true, EntryPoint = "SetConsoleMode")]
        static public extern bool SetConsoleInputMode(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleHandle, ConsoleExInputMode Mode);

        
        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool FlushConsoleInputBuffer(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleInput);

        
        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool GetNumberOfConsoleInputEvents(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleInput, out uint NumberOfEvents);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool PeekConsoleInput(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleInput, [Out] InputRecord[] Buffer, uint Length, out uint NumberOfEventsRead);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool ReadConsoleInput(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleInput, [Out] InputRecord[] Buffer, uint Length, out uint NumberOfEventsRead);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool ReadConsoleInput(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleInput, out InputRecord Buffer, uint Length, out uint NumberOfEventsRead);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool WriteConsoleInput(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleInput, InputRecord[] Buffer, uint Length, out uint NumberOfEventsWritten);


        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool ReadConsole(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleInput, StringBuilder Buffer, uint NumberofCharsToRead, out uint NumberOfCharsRead, ref ConsoleReadConsoleControl InputControl);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool ReadConsole(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleInput, StringBuilder Buffer, uint NumberofCharsToRead, out uint NumberOfCharsRead, IntPtr Reserved);



        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi, EntryPoint = "ReadConsoleA")]
        static public extern bool ReadConsole(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleInput, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] Buffer, uint NumberofCharsToRead, out uint NumberOfCharsRead, IntPtr Reserved);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi, EntryPoint = "ReadConsoleA")]
        static public extern bool ReadConsole(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleInput, ref byte Buffer, uint NumberofCharsToRead, out uint NumberOfCharsRead, IntPtr Reserved);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi, EntryPoint = "WriteConsoleA")]
        static public extern bool WriteConsole(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] Buffer, uint NumberOfCharsToWrite, out uint NumberOfCharsWritten, IntPtr Reserved);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi, EntryPoint = "WriteConsoleA")]
        static public extern bool WriteConsole(Microsoft.Win32.SafeHandles.SafeFileHandle ConsoleOutput,ref byte Buffer, uint NumberOfCharsToWrite, out uint NumberOfCharsWritten, IntPtr Reserved);

        #endregion
    }
}
