using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleLib;

using System.Runtime.InteropServices;


namespace ConsoleTest
{
    // " ☺☻♥♦♣♠•◘○◙♂♀♪♫☼►◄↕‼¶§▬↨↑↓→←∟↔▲▼ !"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~⌂
    //  ÇüéâäàåçêëèïîìÄÅÉæÆôöòûùÿÖÜ¢£¥₧ƒáíóúñÑªº¿⌐¬½¼¡«»░▒▓│┤╡╢╖╕╣║╗╝╜╛┐└┴┬├─┼╞╟╚╔╩╦╠═╬╧╨╤╥╙╘╒╓╫╪┘┌█▄▌▐▀αßΓπΣσµτΦΘΩδ∞φε∩≡±≥≤⌠⌡÷≈°∙·√ⁿ²■ "
    public enum Drawing
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
        DoubleHorVertical       = '│',
        DoubleHorHorizontal     = '═',
        DoubleHorCrossLeft      = '╞',
        DoubleHorCrossRight     = '╡',
        DoubleHorCrossDown      = '╤',
        DoubleHorCrossUp        = '╧',
        DoubleHorCrossMiddle    = '╪',

        DoubleVerUpperLeft      = '╓',
        DoubleVerUpperRight     = '╖',
        DoubleVerLowerLeft      = '╙',
        DoubleVerLowerRight     = '╜',
        DoubleVerDoubleVertical = '║',
        DoubleVerHorizontal     = '─',
        DoubleVerCrossLeft      = '╟',
        DoubleVerCrossRight     = '╢',
        DoubleVerCrossDown      = '╥',
        DoubleVerCrossUp        = '╨',
        DoubleVerCrossMiddle    = '╫',    
    }

    class Program
    {

        
        static void Main(string[] args)
        {

            ConsoleEx.InputBuffer.LineInput = false;
            ConsoleEx.InputBuffer.ProcessedInput = false;
            ConsoleEx.InputBuffer.QuickEditMode = false;
            ConsoleEx.InputBuffer.MouseInput = true;
            //var a = ConsoleEx.InputBuffer.ReadLine();

            //ConsoleEx.ScreenBuffer.ProcessedOutput = false;
            ConsoleEx.ScreenBuffer.SetWindowSize(80, 25);
            ConsoleEx.ScreenBuffer.SetBufferSize(80, 125);
            ConsoleEx.ScreenBuffer.Clear();
            
            WinAPI.InputRecord Event = new WinAPI.InputRecord();
            bool Done = false;
            do
            {
                //ConsoleEx.ScreenBuffer.SetCursorPosition(10, 10);
                Event = ConsoleEx.InputBuffer.NextEvent();
                switch ((InputRecordType)Event.EventType)
                {
                    case InputRecordType.FocusEvent:
                        ConsoleEx.ScreenBuffer.WriteLine("F: {0}", Event.FocusEvent.SetFocus);
                        break;

                    case InputRecordType.MenuEvent:
                        //ConsoleEx.ScreenBuffer.WriteLine("M:{0:x}", Event.MenuEvent.CommandID);
                        break;

                    case InputRecordType.WindowBufferSizeEvent:
                        break;

                    case InputRecordType.MouseEvent:
                        if ((Event.MouseEvent.EventFlags & WinAPI.MouseEventFlags.DOUBLE_CLICK) != 0)
                            Done = true;

                        //ConsoleEx.ScreenBuffer.WriteLine(string.Format("{0}x{1}-{2} {3} {4}", Event.MouseEvent.MousePosition.X, Event.MouseEvent.MousePosition.Y, Event.MouseEvent.EventFlags.ToString(), Event.MouseEvent.ButtonState.ToString(), Event.MouseEvent.ControlKeyState.ToString()));
                        //ConsoleEx.ScreenBuffer.WritePos(new CharacterAttribute(ConsoleColor.Cyan, ConsoleColor.DarkRed), Event.MouseEvent.MousePosition.X, Event.MouseEvent.MousePosition.Y);
                        break;

                    case InputRecordType.KeyEvent:
                        if (ConsoleEx.InputBuffer.CanDisplay(Event.KeyEvent))
                        {
                            ConsoleEx.ScreenBuffer.ProcessedOutput = false;
                            ConsoleEx.ScreenBuffer.Write("{0:X4} ", (int)Event.KeyEvent.Character);
                            if (Event.KeyEvent.Character > 0 && Event.KeyEvent.Character < 32)
                            {
                                ConsoleEx.ScreenBuffer.Write('^');
                                ConsoleEx.ScreenBuffer.Write((char)('A' + Event.KeyEvent.Character - 1));
                            }
                            else
                                ConsoleEx.ScreenBuffer.Write(Event.KeyEvent.Character);

                            ConsoleEx.ScreenBuffer.ProcessedOutput = true;

                            //if (Event.KeyEvent.Character == '\r')
                                ConsoleEx.ScreenBuffer.Write('\n');
                        }
                        //else
                        //{
                        //    var Key = Event.KeyEvent;

                        //    ConsoleEx.ScreenBuffer.Write("'{0}' {3:X4} {1} Rep: {2} ", (char)Key.Character, (Key.KeyDown != 0) ? "DOWN" : "UP  ", Key.RepeatCount, (int)Key.Character);
                        //    ConsoleEx.ScreenBuffer.Write("Key: {0,2:X}, Scan: {1,2:X} ", Key.VirtualKeyCode, Key.VirtualScanCode);
                        //    ConsoleEx.ScreenBuffer.Write("Ctrl: {0} ", Key.ControlKeyState.ToString());
                        //    ConsoleEx.ScreenBuffer.WriteLine();
                        //}
                        break;

                    case InputRecordType.None:
                    default:
                        break;
                }
            } while (!Done);


            //bool Done = false;
            //do
            //{
            //    ConsoleKeyInfo t = ConsoleEx.InputBuffer.ReadKey(true);
            //    ConsoleEx.ScreenBuffer.Write(t.KeyChar);
            //    if (t.KeyChar == 'q')
            //        Done = true;

            //} while (!Done);


            //ConsoleEx.ScreenBuffer.WriteLine("Lorem ipsum dolor sit amet, nec fusce potenti eget egestas, mauris commodo felis vestibulum vestibulum. Urna consequat sed faucibus duis, ac non tristique tortor, eu tristique lacinia vehicula diam, consectetuer quisque felis sollicitudin. Non non diam duis, consectetuer a tortor est nisl. Accumsan pellentesque dictum nulla eget, elit non commodo turpis, libero in viverra nibh amet.");
            //ConsoleEx.InputBuffer.Flush();
            //ScreenBuffer NewBuffer = new ScreenBuffer(ConsoleEx.ScreenBuffer, true);
            ////var old = ConsoleEx.SwapBuffers(NewBuffer);
            ////ConsoleEx.StdOutput = NewBuffer.Handle;
            //Console.Out.Write("123456789...");
            
            //ConsoleEx.ScreenBuffer.MoveBufferArea(3, 1, 50, 4, 10, 10, '!', ConsoleColor.White, ConsoleColor.Cyan);
            ////ConsoleEx.ScreenBuffer.MoveBufferArea(new System.Drawing.Rectangle (3, 1, 40, 5), new System.Drawing.Rectangle (0, 0, 80, 25), new System.Drawing.Point(10, 10), '!', ConsoleColor.White, ConsoleColor.Cyan);
            //ConsoleEx.ScreenBuffer.Scroll(4);

            ////ConsoleEx.InputBuffer.EchoInput = false;
            //ConsoleEx.ScreenBuffer.VirtualTerminal = true;
            //ConsoleEx.ScreenBuffer.NewLineAutoReturn = false;
            //ConsoleEx.ScreenBuffer.LVBGridWoldwide = true;

            ////ConsoleEx.InputBuffer.Flush();
            ////var a2 = ConsoleEx.InputBuffer.PeekNextEvent(true);
            
            //////var a6 = ConsoleEx.InputBuffer.NextEvent(InputRecordType.All);
            ////var a = ConsoleEx.InputBuffer.ReadKey();

            ////ConsoleEx.InputBuffer.Mode &= ~ConsoleExInputMode.EchoInput;
            ////ConsoleEx.InputBuffer.Mode &= ~ConsoleExInputMode.LineInput;
            ////ConsoleEx.InputBuffer.Mode &= ~ConsoleExInputMode.ProcessedInput;

            ////ConsoleEx.OutputEncoding = System.Text.Encoding.UTF8;
            ////Console.OutputEncoding = System.Text.Encoding.UTF8;
            ////Console.InputEncoding = System.Text.Encoding.UTF8;

            ////ConsoleEx.OutputEncoding = System.Text.Encoding.Unicode;
            ////Console.OutputEncoding = System.Text.Encoding.Unicode;

            ////var i = ConsoleEx.In.ReadLine();
            ////var i2 = ConsoleEx.InputBuffer.Stream.ReadLine();
            //Console.WriteLine();
            //Console.Out.WriteLine("Testing 1®");
            //ConsoleEx.Out.WriteLine("Testing 2®");
            //ConsoleEx.ScreenBuffer.Stream.WriteLine("Testing 3®");
            ////var i = Console.In.ReadLine();

            //var k = ConsoleEx.InputBuffer.ReadLine();

            ////var t = ConsoleEx.Window;

            ////ConsoleEx.SwapBuffers(old);


        }
    }
}
