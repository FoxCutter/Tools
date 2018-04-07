using System;
using System.Runtime.InteropServices;

namespace ConsoleLib
{
    #region Drawing and Control Characters
    // " ☺☻♥♦♣♠•◘○◙♂♀♪♫☼►◄↕‼¶§▬↨↑↓→←∟↔▲▼ !"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~⌂
    //  ÇüéâäàåçêëèïîìÄÅÉæÆôöòûùÿÖÜ¢£¥₧ƒáíóúñÑªº¿⌐¬½¼¡«»░▒▓│┤╡╢╖╕╣║╗╝╜╛┐└┴┬├─┼╞╟╚╔╩╦╠═╬╧╨╤╥╙╘╒╓╫╪┘┌█▄▌▐▀αßΓπΣσµτΦΘΩδ∞φε∩≡±≥≤⌠⌡÷≈°∙·√ⁿ²■ "
    public enum Characters : ushort
    {
        // Control Charcaters
        NUL = 0,    //    - NULL
        SOH,        // ^A - START OF HEADING
        STX,        // ^B - START OF TEXT
        ETX,        // ^C - END OF TEXT
        EOT,        // ^D - END OF TRANSMISSION
        ENQ,        // ^E - ENQUIRY
        ACK,        // ^F - ACKNOWLEDGE
        BEL,        // ^G - BELL
        BS,         // ^H - BACKSPACE
        HT,         // ^I - HORIZONTAL TABULATION
        LF,         // ^J - LINE FEED
        VT,         // ^K - VERTICAL TABULATION
        FF,         // ^L - FORM FEED
        CR,         // ^M - CARRIAGE RETURN
        SO,         // ^N - SHIFT OUT
        SI,         // ^O - SHIFT IN
        DLE,        // ^P - DATA LINK ESCAPE
        DC1,        // ^Q - DEVICE CONTROL ONE
        DC2,        // ^R - DEVICE CONTROL TWO
        DC3,        // ^S - DEVICE CONTROL THREE
        DC4,        // ^T - DEVICE CONTROL FOUR
        NAK,        // ^U - NEGATIVE ACKNOWLEDGE
        SYN,        // ^V - SYNCHRONOUS IDLE
        ETB,        // ^W - END OF TRANSMISSION BLOCK
        CAN,        // ^X - CANCEL
        EM,         // ^Y - END OF MEDIUM
        SUB,        // ^Z - SUBSTITUTE
        ESC,        // ^[ - ESCAPE
        FS,         // ^\ - FILE SEPARATOR
        GS,         // ^] - GROUP SEPARATOR
        RS,         // ^^ - RECORD SEPARATOR
        US,         // ^_ - UNIT SEPARATOR
        DEL = 127,  //    - DELETE
        
        // General Image charcaters
        Face                    = '☺',
        InvertedFace            = '☻',
        Heart                   = '♥',
        Dimand                  = '♦',
        Club                    = '♣',
        Spade                   = '♠',
        Dot                     = '•',
        InverseDot              = '◘',
        Circle                  = '○',
        InverseCircle           = '◙',
        Mars                    = '♂',
        Venus                   = '♀',
        EighthNote              = '♪',
        DoubleNote              = '♫',
        Solar                   = '☼',
        RightTriangle           = '►',
        LeftTriangle            = '◄',
        UpDownArrow             = '↕',
        DoubleExclamation       = '‼',
        Paragraph               = '¶',
        Section                 = '§',
        BlackRectangle          = '▬',
        UpDownArrowWithBase     = '↨',
        UpArrow                 = '↑',
        DownArrow               = '↓',
        RightArrow              = '→',
        LeftArrow               = '←',
        RightAngle              = '∟',
        LeftRightArrow          = '↔',
        UpTriangle              = '▲',
        DownTriangle            = '▼',        

        House                   = '⌂',
        
        Cent                    = '¢',
        Pound                   = '£',
        Yen                     = '¥',
        Peseta                  = '₧',
        Florin                  = 'ƒ',

        OrdinalFeminine         = 'ª',
        OrdinalMasculine        = 'º',
        InvertedQuestion        = '¿',
        ReversedNotSign         = '⌐',
        NotSign                 = '¬',
        OneHalf                 = '½',
        OneForth                = '¼',
        InvertedExclimation     = '¡',
        AngleQuoteLeft          = '«',
        AngleQuoteRight         = '»',
        LightShade              = 0x2591, // These charcaters render double height in my default font, so just stick to using the code points.
        MediumShade             = 0x2592,
        DarkShade               = 0x2593,

        FullBlock               = '█',
        LowerHalfBlock          = '▄',
        LeftHalfBlock           = '▌',
        RightHalfBlock          = '▐',
        UpperHalfBlock          = '▀',
        Alpha                   = 'α',
        Beta                    = 'ß',
        Gamma                   = 'Γ',
        Pi                      = 'π',
        CapitalSigma            = 'Σ',
        Sigma                   = 'σ',
        Micro                   = 'µ',
        Tau                     = 'τ',
        CapitalPhi              = 'Φ',
        Theta                   = 'Θ',
        Omega                   = 'Ω',
        Delta                   = 'δ',
        Infinity                = '∞',
        Phi                     = 'φ',
        Epsilon                 = 'ε',
        Intersection            = '∩',
        IdenticalTo             = '≡',
        PlusMinus               = '±',
        GreaterThenEqual        = '≥',
        LessThenEqual           = '≤',
        IntegralTop             = '⌠',
        IntegralBottom          = '⌡',
        Division                = '÷',
        AlmostEqualTo           = '≈',
        Degree                  = '°',
        Bullet                  = 0x2219, // These two look almost the same so use the code point for them to be sure
        MiddleDot               = 0x00B7,
        SquareRoot              = '√',
        SuperScriptN            = 'ⁿ',
        SuperScript2            = '²',
        BlackSquare             = '■',

        NonBreakSpace           = 0x00a0,    

        // Box Drawing
        UpperLeft               = '┌',   
        UpperRight              = '┐',   
        LowerLeft               = '└',   
        LowerRight              = '┘',   
        Vertical                = '│',   
        Horizontal              = '─',
        CrossLeft               = '├',
        CrossRight              = '┤',
        CrossDown               = '┬',
        CrossUp                 = '┴',
        CrossMiddle             = '┼',

        DoubleUpperLeft         = '╔',
        DoubleUpperRight        = '╗',
        DoubleLowerLeft         = '╚',
        DoubleLowerRight        = '╝',
        DoubleVertical          = '║',
        DoubleHorizontal        = '═',
        DoubleCrossLeft         = '╠',
        DoubleCrossRight        = '╣',
        DoubleCrossDown         = '╦',
        DoubleCrossUp           = '╩',
        DoubleCrossMiddle       = '╬',

        DoubleHorUpperLeft      = '╒',
        DoubleHorUpperRight     = '╕',
        DoubleHorLowerLeft      = '╘',
        DoubleHorLowerRight     = '╛',
        //DoubleHorVertical       = '│',
        //DoubleHorHorizontal     = '═',
        DoubleHorCrossLeft      = '╞',
        DoubleHorCrossRight     = '╡',
        DoubleHorCrossDown      = '╤',
        DoubleHorCrossUp        = '╧',
        DoubleHorCrossMiddle    = '╪',

        DoubleVerUpperLeft      = '╓',
        DoubleVerUpperRight     = '╖',
        DoubleVerLowerLeft      = '╙',
        DoubleVerLowerRight     = '╜',
        //DoubleVerDoubleVertical = '║',
        //DoubleVerHorizontal     = '─',
        DoubleVerCrossLeft      = '╟',
        DoubleVerCrossRight     = '╢',
        DoubleVerCrossDown      = '╥',
        DoubleVerCrossUp        = '╨',
        DoubleVerCrossMiddle    = '╫',    
    }    
    #endregion

    #region Exception

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

    #endregion

    #region Console Enums

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

    [Flags]
    public enum FontFamily : uint
    {
        FixedPitch      = 0x01,
        Vector          = 0x02,
        TrueType        = 0x04,
        Device          = 0x08,

        Roman           = 0x10,
        Swiss           = 0x20,
        Modern          = 0x30,
        Script          = 0x40,
        Decorative      = 0x50,
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
    
    
    #endregion 

    #region Structs

    // Delegate to handle calls from SetConsoleCtrlHandler
    public delegate bool CtrlHandlerRoutine(ConsoleExCtrlEventType CtrlType);
        
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
            Left = (short)nLeft;
            Top = (short)nTop;
            Right = (short)nRight;
            Bottom = (short)nBottom;
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
        public uint SetFocus; // BOOLs have to be marshed as uints otherwise things get borked.
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

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct KeyEventRecord
    {
        public uint KeyDown;    // BOOLs have to be marshed as uints otherwise things get borked.
        public short RepeatCount;
        public short VirtualKeyCode;
        public short VirtualScanCode;
        public char Character;
        public CtrlKeyState ControlKeyState;
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

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
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
        public FontFamily FontFamily;
        public uint FontWeight;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public String FaceName;

        public void SetSize()
        {
            Size = (uint)Marshal.SizeOf(typeof(ConsoleFontInfoEx));
        }
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

    #endregion
}
