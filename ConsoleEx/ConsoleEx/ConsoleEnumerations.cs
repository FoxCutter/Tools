using System;
using System.Runtime.InteropServices;

namespace ConsoleLib
{
    public class ConsoleExException : System.Exception
    {
        public ConsoleExException(string ErrorMessage)
            : base(ErrorMessage)
        {
            HResult = Marshal.GetLastWin32Error();
        }

        public ConsoleExException(string ErrorMessage, Exception e)
            : base(ErrorMessage, e)
        {
            HResult = Marshal.GetLastWin32Error();
        }
    }
    
    [Flags]
    public enum ConsoleExInputMode : uint
    {
        // Input Buffer Flags
        ProcessedInput          = 0x0001,
        LineInput               = 0x0002,
        EchoInput               = 0x0004,
        WindowInput             = 0x0008,
        MouseInput              = 0x0010,
        InsertMode              = 0x0020,
        QuickEditMode           = 0x0040,
        ExtendedFlags           = 0x0080,
        AutoPosition            = 0x0100,
        VirutalTerminalInput    = 0x0200,
    }

    [Flags]
    public enum ConsoleExOutputMode : uint
    {
        // Output Buffer Flags
        ProcessedOutput             = 0x0001,
        WrapAtEOL                   = 0x0002,
        VirutalTerminalProcessing   = 0x0004,
        DisableNewLineAutoReturn    = 0x0008,
        LVBGridWoldwide             = 0x0010,
    }
    
    
    public enum ConsoleExCtrlEventType : uint
    {
        CtrlC = 0,
        CtrlBreak = 1,
        CtrlClose = 2,
        CtrlLogoff = 5,
        CtrlShutdown = 6,
    }

    public enum ConsoleExDisplayModeFlags : uint
    {
        None = 0,
        Fullscreen = 1,
        Fullscreen_Hardware = 2,

        Set_Fullscreen = 1,
        Set_Windowed = 2,
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
    
    // Making a special struct for this to simplify the converstion from the preexisting ConsoleColor enum to the
    // direct character attributes (the values for both are the same).
    [StructLayout(LayoutKind.Sequential)]
    public struct CharacterAttribute
    {
        public CharacterAttributeEnum Value;

        public CharacterAttribute(CharacterAttributeEnum nAttribute)
        {
            Value = nAttribute;
        }

        public CharacterAttribute(int nAttribute)
        {
            Value = (CharacterAttributeEnum)nAttribute;
        }

        public CharacterAttribute(System.ConsoleColor Foreground, System.ConsoleColor Background)
        {
            Value = (CharacterAttributeEnum)(((ushort)Background << 4) | (ushort)Foreground);
        }

        public void SetCharacterAttribute(System.ConsoleColor Foreground, System.ConsoleColor Background)
        {
            Value = (CharacterAttributeEnum)(((ushort)Background << 4) | (ushort)Foreground);
        }

        public ConsoleColor Foreground
        {
            get
            {
                return (ConsoleColor)((ushort)Value & 0x0F);
            }
            set
            {
                short fg = (short)value;
                short bg = (short)((ushort)Value & 0xF0);

                Value = (CharacterAttributeEnum)(bg | fg);
            }
        }

        public ConsoleColor Background
        {
            get
            {
                return (ConsoleColor)(((ushort)Value & 0xF0) >> 4);
            }
            set
            {
                short fg = (short)((ushort)Value & 0x0F);
                short bg = (short)((ushort)value << 4);

                Value = (CharacterAttributeEnum)(bg | fg);
            }
        }
    }

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
    public struct CharInfo
    {
        [FieldOffset(0)]
        public char UnicodeChar;
        [FieldOffset(2)]
        public CharacterAttribute Attributes;

        public CharInfo(char nChar, CharacterAttribute nAttribute)
        {
            UnicodeChar = nChar;
            Attributes = nAttribute;
        }
    }
}
