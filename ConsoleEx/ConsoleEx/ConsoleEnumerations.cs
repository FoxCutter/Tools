using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        LVBGridWoldview             = 0x0010,
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
       
    // Making a special struct for this to simplify the converstion from the preexisting ConsoleColor enum to the
    // direct character attributes (the values for both are the same).
    [StructLayout(LayoutKind.Sequential)]
    public struct CharacterAttribute
    {
        public WinAPI.CharacterAttributeEnum Value;

        public CharacterAttribute(WinAPI.CharacterAttributeEnum nAttribute)
        {
            Value = nAttribute;
        }

        public CharacterAttribute(int nAttribute)
        {
            Value = (WinAPI.CharacterAttributeEnum)nAttribute;
        }

        public CharacterAttribute(System.ConsoleColor Foreground, System.ConsoleColor Background)
        {
            Value = (WinAPI.CharacterAttributeEnum)(((ushort)Background << 4) | (ushort)Foreground);
        }

        public void SetCharacterAttribute(System.ConsoleColor Foreground, System.ConsoleColor Background)
        {
            Value = (WinAPI.CharacterAttributeEnum)(((ushort)Background << 4) | (ushort)Foreground);
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

                Value = (WinAPI.CharacterAttributeEnum)(bg | fg);
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

                Value = (WinAPI.CharacterAttributeEnum)(bg | fg);
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
