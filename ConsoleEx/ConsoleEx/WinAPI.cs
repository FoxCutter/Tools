using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace ConsoleLib
{
    public class WinAPI
    {
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
        public static extern IntPtr CreateFile(string Filename, DesiredAccess dwDesiredAccess, ShareMode dwShareMode, IntPtr lpSecurityAttributes, CreationDispositionType CreationDisposition, uint FlagsAndAttributes, IntPtr TemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        static public extern FileTypes GetFileType(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        static public extern bool CloseHandle(IntPtr hObject);

        #endregion

        #region Enums

        public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

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

        public enum ConsoleFlags : uint
        {
            TextModeBuffer = 0x0001,
        }

        public enum StdHandleType : int
        {
            STD_INPUT_HANDLE    = (int) -10,
            STD_OUTPUT_HANDLE   = (int) -11,
            STD_ERROR_HANDLE    = (int) -12
        }

        [Flags]
        public enum SelectionFlags : uint
        {
            CONSOLE_NO_SELECTION            = 0x0000,
            CONSOLE_SELECTION_IN_PROGRESS   = 0x0001,
            CONSOLE_SELECTION_NOT_EMPTY     = 0x0002,
            CONSOLE_MOUSE_SELECTION         = 0x0004,
            CONSOLE_MOUSE_DOWN              = 0x0008,
        }

        [Flags]
        public enum HistoryFlags : uint
        {
            HISTORY_NO_DUP_FLAG = 0x0001,
        }

        [Flags]
        public enum CtrlKeyState : uint
        {
            RIGHT_ALT_PRESSED   = 0x0001,
            LEFT_ALT_PRESSED    = 0x0002,
            RIGHT_CTRL_PRESSED  = 0x0004,
            LEFT_CTRL_PRESSED   = 0x0008,
            SHIFT_PRESSED       = 0x0010,
            NUMLOCK_ON          = 0x0020,
            SCROLLLOCK_ON       = 0x0040,
            CAPSLOCK_ON         = 0x0080,
            ENHANCED_KEY        = 0x0100,
        }

       
        [Flags]
        public enum MouseButtonState : uint
        {
            FROM_LEFT_1ST_BUTTON_PRESSED    = 0x0001,
            RIGHTMOST_BUTTON_PRESSED        = 0x0002,
            FROM_LEFT_2ND_BUTTON_PRESSED    = 0x0004,
            FROM_LEFT_3RD_BUTTON_PRESSED    = 0x0008,
            FROM_LEFT_4TH_BUTTON_PRESSED    = 0x0010,
        }

        [Flags]
        public enum MouseEventFlags : uint
        {
            MOUSE_MOVED     = 0x0001,
            DOUBLE_CLICK    = 0x0002,
            MOUSE_WHEELED   = 0x0004,
            MOUSE_HWHEELED  = 0x0008,
        }

        public enum InputRecordType : ushort
        {
            None                    = 0x0000,

            KeyEvent                = 0x0001,
            MouseEvent              = 0x0002,
            WindowBufferSizeEvent   = 0x0004,
            MenuEvent               = 0x0008,
            FocusEvent              = 0x0010,

            All = KeyEvent | MouseEvent | WindowBufferSizeEvent | MenuEvent | FocusEvent,
        }
        
        [Flags]
        public enum CharacterAttributeEnum : ushort
        {
            FOREGROUND_BLUE             = 0x0001, // text color contains blue.
            FOREGROUND_GREEN            = 0x0002, // text color contains green.
            FOREGROUND_RED              = 0x0004, // text color contains red.
            FOREGROUND_INTENSITY        = 0x0008, // text color is intensified.

            BACKGROUND_BLUE             = 0x0010, // background color contains blue.
            BACKGROUND_GREEN            = 0x0020, // background color contains green.
            BACKGROUND_RED              = 0x0040, // background color contains red.
            BACKGROUND_INTENSITY        = 0x0080, // background color is intensified.

            COMMON_LVB_LEADING_BYTE     = 0x0100, // Leading byte.
            COMMON_LVB_TRAILING_BYTE    = 0x0200, // Trailing byte.
            COMMON_LVB_GRID_HORIZONTAL  = 0x0400, // Top horizontal.
            COMMON_LVB_GRID_LVERTICAL   = 0x0800, // Left vertical.
            COMMON_LVB_GRID_RVERTICAL   = 0x1000, // Right vertical.
            COMMON_LVB_REVERSE_VIDEO    = 0x4000, // Reverse foreground and background attributes.
            COMMON_LVB_UNDERSCORE       = 0x8000, // Underscore.
        }

        // Delegate to handle calls from SetConsoleCtrlHandler
        public delegate bool CtrlHandlerRoutine(ConsoleExCtrlEventType CtrlType);
        
        #endregion

        #region Structs

        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;

            public Coord(int nX, int nY)
            {
                X = (short)nX;
                Y = (short)nY;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;

            public SmallRect(int nLeft, int nTop, int nRight, int nBottom)
            {
                Top = (short)nTop;
                Left = (short)nLeft;
                Bottom = (short)nBottom;
                Right = (short)nRight;
            }

            public int Width
            {
                get { return (Right - Left) + 1; }
                set { Right = (short)((Left + value) - 1); }
            }

            public int Height
            {
                get { return (Bottom - Top) + 1; }
                set { Bottom = (short)((Top + value) - 1); }
            }
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct ConsoleSelectionInfo
        {
            public SelectionFlags Flags;
            public Coord SelectionAnchor;
            public SmallRect Selection;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ConsoleHistoryInfo
        {
            public uint Size;
            public uint HistoryBufferSize;
            public uint NumberOfHistoryBuffers;
            public HistoryFlags Flags;

            public void SetSize()
            {
                Size = (uint)Marshal.SizeOf(typeof(ConsoleHistoryInfo));
            }            
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ConsoleReadConsoleControl
        {
            public uint Length;
            public uint InitialChars;
            public uint CtrlWakeupMask;
            public CtrlKeyState ControlKeyState;

            public void SetSize()
            {
                Length = (uint)Marshal.SizeOf(typeof(ConsoleReadConsoleControl));
            }
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct FocusEventRecord
        {
            public bool SetFocus;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ConsoleScreenBufferInfo
        {
            public Coord BufferSize;
            public Coord CursorPosition;
            public CharacterAttribute Attributes;
            public SmallRect Window;
            public Coord MaximumWindowSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ConsoleScreenBufferInfoEx
        {
            public uint Size;
            public Coord BufferSize;
            public Coord CursorPosition;
            public CharacterAttribute Attributes;
            public SmallRect Window;
            public Coord MaximumWindowSize;
            public short PopupAttributes;
            public bool FullscreenSupported;
            
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 16)]
            public uint [] ColorTable;

            public void SetSize()
            {
                Size = (uint)Marshal.SizeOf(typeof(ConsoleScreenBufferInfoEx));
            }

        }

        // I'm not sure what I'm doing wrong here, but when I use LayoutKind.Sequential it seems to get the field spacing wrong.
        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
        public struct KeyEventRecord
        {
            [FieldOffset(0)] public bool KeyDown;
            [FieldOffset(4)] public short RepeatCount;
            [FieldOffset(6)] public short VirtualKeyCode;
            [FieldOffset(8)] public short VirtualScanCode;
            [FieldOffset(10)] public char Character;
            [FieldOffset(12)] public CtrlKeyState ControlKeyState;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MenuEventRecord
        {
            public uint CommandID;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MouseEventRecord
        {
            public Coord MousePosition;
            public MouseButtonState ButtonState;
            public CtrlKeyState ControlKeyState;
            public MouseEventFlags EventFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WindowBufferSizeRecord
        {
            public Coord BufferSize;
        }

        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
        public struct InputRecord
        {
            [FieldOffset(0)] public InputRecordType EventType;

            // Event Union
            [FieldOffset(4)] public KeyEventRecord KeyEvent;
            [FieldOffset(4)] public MouseEventRecord MouseEvent;
            [FieldOffset(4)] public WindowBufferSizeRecord WindowBufferSizeEvent;
            [FieldOffset(4)] public MenuEventRecord MenuEvent;
            [FieldOffset(4)] public FocusEventRecord FocusEvent;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ConsoleCursorInfo
        {
            public uint Size;
            public bool Visable;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ConsoleFontInfo
        {
            public uint Font;
            public Coord FontSize;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct ConsoleFontInfoEx
        {
            public uint Size;
            public uint Font;
            public Coord FontSize;
            public uint FontFamily;
            public uint FontWeight;
            
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String FontName;

            public void SetSize()
            {
                Size = (uint)Marshal.SizeOf(typeof(ConsoleFontInfoEx));
            }
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
        static public extern bool GetConsoleHistoryInfo(out ConsoleHistoryInfo ConsoleHistoryInfo);

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
        static public extern bool SetStdHandle(StdHandleType StdHandle, IntPtr Handle);

        
        [DllImport("kernel32.dll", SetLastError = true)]
        static public extern bool GetConsoleDisplayMode(out ConsoleExDisplayModeFlags ModeFlags);

        
        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool GetNumberOfConsoleMouseButtons([Out]out uint NumberOfMouseButtons);

        #endregion

        #region Screen Buffer Functions

        [DllImport("Kernel32.dll", SetLastError = true, EntryPoint = "GetConsoleMode")]
        static public extern bool GetConsoleOutputMode(IntPtr ConsoleHandle, out ConsoleExOutputMode Mode);

        [DllImport("Kernel32.dll", SetLastError = true, EntryPoint = "SetConsoleMode")]
        static public extern bool SetConsoleOutputMode(IntPtr ConsoleHandle, ConsoleExOutputMode Mode);

        
        [DllImport("kernel32.dll", SetLastError = true)]
        static public extern bool SetConsoleDisplayMode(IntPtr ConsoleOutput, ConsoleExDisplayModeFlags Flags, out Coord NewScreenBufferDimensions);

        
        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern Coord GetLargestConsoleWindowSize(IntPtr ConsoleOutput);


        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern IntPtr CreateConsoleScreenBuffer(DesiredAccess DesiredAccess, ShareMode ShareMode, IntPtr SecurityAttributes, ConsoleFlags Flags, IntPtr ScreenBufferData);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool SetConsoleActiveScreenBuffer(IntPtr ConsoleOutput);

        
        
        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool FillConsoleOutputAttribute(IntPtr ConsoleOutput, CharacterAttributeEnum Attribute, uint Length, Coord WriteCoord, out uint NumberOfAttrsWritten);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool FillConsoleOutputCharacter(IntPtr ConsoleOutput, char Character, uint Length, Coord WriteCoord, out uint NumberOfAttrsWritten);


        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool GetConsoleCursorInfo(IntPtr ConsoleOutput, out ConsoleCursorInfo ConsoleCursorInfo);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool SetConsoleCursorInfo(IntPtr ConsoleOutput, ref ConsoleCursorInfo ConsoleCursorInfo);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool SetConsoleCursorPosition(IntPtr ConsoleOutput, Coord CurrentPosition);


        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool GetConsoleScreenBufferInfo(IntPtr ConsoleOutput, out ConsoleScreenBufferInfo ConsoleScreenBufferInfo);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool GetConsoleScreenBufferInfoEx(IntPtr ConsoleOutput, ref ConsoleScreenBufferInfoEx ConsoleScreenBufferInfoEx);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool SetConsoleScreenBufferInfoEx(IntPtr ConsoleOutput, ref ConsoleScreenBufferInfoEx ConsoleScreenBufferInfoEx);


        
        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool SetConsoleScreenBufferSize(IntPtr ConsoleOutput, Coord Size);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool SetConsoleTextAttribute(IntPtr ConsoleOutput, CharacterAttribute Attributes);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool SetConsoleWindowInfo(IntPtr ConsoleOutput, bool Absolute, ref SmallRect ConsoleWindow);


        
        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern Coord GetConsoleFontSize(IntPtr ConsoleOutput, uint Font);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool GetCurrentConsoleFont(IntPtr ConsoleOutput, bool MaximumWindow, out ConsoleFontInfo ConsoleCurrentFont);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool GetCurrentConsoleFontEx(IntPtr ConsoleOutput, bool MaximumWindow, ref ConsoleFontInfoEx ConsoleCurrentFontEx);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool SetCurrentConsoleFontEx(IntPtr ConsoleOutput, bool MaximumWindow, ref ConsoleFontInfoEx ConsoleCurrentFontEx);


        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool ReadConsoleOutput(IntPtr ConsoleOutput, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] CharInfo[,] Buffer, Coord BufferSize, Coord BufferCoord, ref SmallRect ReadRegion);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool ReadConsoleOutputAttribute(IntPtr ConsoleOutput, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] CharacterAttribute[] Attribute, uint Length, Coord ReadCoord, out uint NumberOfAttrsRead);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool ReadConsoleOutputCharacter(IntPtr ConsoleOutput, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] char[] Character, uint Length, Coord ReadCoord, out uint NumberOfCharsRead);


        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool WriteConsole(IntPtr ConsoleOutput, string Buffer, uint NumberOfCharsToWrite, out uint NumberOfCharsWritten, IntPtr Reserved);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool WriteConsole(IntPtr ConsoleOutput, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] char [] Buffer, uint NumberOfCharsToWrite, out uint NumberOfCharsWritten, IntPtr Reserved);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool WriteConsoleOutput(IntPtr ConsoleOutput, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] CharInfo[,] Buffer, Coord BufferSize, Coord BufferCoord, ref SmallRect WriteRegion);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool WriteConsoleOutputAttribute(IntPtr ConsoleOutput, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] CharacterAttribute[] Attribute, uint Length, Coord WriteCoord, out uint NumberOfAttrsWriten);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool WriteConsoleOutputCharacter(IntPtr ConsoleOutput, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] char[] Character, uint Length, Coord WriteCoord, out uint NumberOfCharsWriten);


        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool ScrollConsoleScreenBuffer(IntPtr ConsoleOutput, ref SmallRect ScrollRectangle, IntPtr Reserved, Coord DestinationOrigin, ref CharInfo Fill);

        // Quick hack so we don't have to try to force a SMALL_REC into an IntPtr.
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "ScrollConsoleScreenBuffer")]
        static public extern bool ScrollConsoleScreenBufferWithClipping(IntPtr ConsoleOutput, ref SmallRect ScrollRectangle, ref SmallRect ClippingRectangle, Coord DestinationOrigin, ref CharInfo Fill);

        #endregion

        #region Input Buffer Functions

        [DllImport("Kernel32.dll", SetLastError = true, EntryPoint = "GetConsoleMode")]
        static public extern bool GetConsoleInputMode(IntPtr ConsoleHandle, out ConsoleExInputMode Mode);

        [DllImport("Kernel32.dll", SetLastError = true, EntryPoint = "SetConsoleMode")]
        static public extern bool SetConsoleInputMode(IntPtr ConsoleHandle, ConsoleExInputMode Mode);

        
        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool FlushConsoleInputBuffer(IntPtr ConsoleInput);

        
        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool GetNumberOfConsoleInputEvents(IntPtr ConsoleInput, out uint NumberOfEvents);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool PeekConsoleInput(IntPtr ConsoleInput, [Out] InputRecord [] Buffer, uint Length, out uint NumberOfEventsRead);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool ReadConsoleInput(IntPtr ConsoleInput, [Out] InputRecord[] Buffer, uint Length, out uint NumberOfEventsRead);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static public extern bool WriteConsoleInput(IntPtr ConsoleInput, InputRecord[] Buffer, uint Length, out uint NumberOfEventsWritten);


        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool ReadConsole(IntPtr ConsoleInput, StringBuilder Buffer, uint NumberofCharsToRead, out uint NumberOfCharsRead, ConsoleReadConsoleControl InputControl);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static public extern bool ReadConsole(IntPtr ConsoleInput, StringBuilder Buffer, uint NumberofCharsToRead, out uint NumberOfCharsRead, IntPtr Reserved);

        #endregion

    }
}
