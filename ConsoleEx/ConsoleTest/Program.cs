using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleLib;

using System.Runtime.InteropServices;


namespace ConsoleTest
{
    // " ☺☻♥♦♣♠•◘○◙♂♀♪♫☼►◄↕‼¶§▬↨↑↓→←∟↔▲▼ !"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~⌂ÇüéâäàåçêëèïîìÄÅÉæÆôöòûùÿÖÜ¢£¥₧ƒáíóúñÑªº¿⌐¬½¼¡«»░▒▓│┤╡╢╖╕╣║╗╝╜╛┐└┴┬├─┼╞╟╚╔╩╦╠═╬╧╨╤╥╙╘╒╓╫╪┘┌█▄▌▐▀αßΓπΣσµτΦΘΩδ∞φε∩≡±≥≤⌠⌡÷≈°∙·√ⁿ²■ "
    public enum Drawing
    {
        // Control Charcaters
        NUL,
        SOH,
        STX,
        ETX,
        EOT,
        ENQ,
        ACK,
        BEL,
        BS,
        HT,
        LF,
        VT,
        FF,
        CR,
        SO,
        SI,
        DLE,
        DC1,
        DC2, 
        DC3, 
        DC4,
        NAK,
        SYN,
        ETB,
        CAN,
        EM,
        SUB,
        ESC,
        FS,
        GS,
        RS,
        US,

        
        // General Image charcaters
        Face                    = '☺',
        InvertedFace            = '☻',
        Heart                   = '♥',
        Dimand                  = '♦',
        Club                    = '♣',
        Spade                   = '♠',
        Dot                     = '•',
        InvertedDot             = '◘',
        Circle                  = '○',
        InvertedCircle          = '◙',
        Mars                    = '♂',
        Venus                   = '♀',
        EighthNote              = '♪',
        DoubleNote              = '♫',
        Solar                   = '☼',
        RightTriangle           = '►',
        LeftTriangle            = '◄',
        UpDownArrow             = '↕',
        DoubleExclimation       = '‼',
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

        NonBreakSpace           = 0x00a0,

        // Box Drawing
        LightShade              = 0x2591, // These charcaters render double height in my default font, so just stick to using the code points.
        MediumShade             = 0x2592,
        DarkShade               = 0x2593,
        FullBlock               = '█',
        LowerHalfBlock          = '▄',
        LeftHalfBlock           = '▌',
        RightHalfBlock          = '▐',
        UpperHalfBlock          = '▀',
        BlackSquare             = '■',

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
            //ConsoleEx.ScreenBuffer.WriteLine("Lorem ipsum dolor sit amet, nec fusce potenti eget egestas, mauris commodo felis vestibulum vestibulum. Urna consequat sed faucibus duis, ac non tristique tortor, eu tristique lacinia vehicula diam, consectetuer quisque felis sollicitudin. Non non diam duis, consectetuer a tortor est nisl. Accumsan pellentesque dictum nulla eget, elit non commodo turpis, libero in viverra nibh amet.");


            //ConsoleEx.AddAlias("aaa", "bbb", "consoletest.exe");

            //string t1 = ConsoleEx.InputBuffer.ReadLine();
            //string t2 = ConsoleEx.InputBuffer.ReadLine();

            //System.Windows.Form.Keys;
            
                        
            //ConsoleEx.ScreenBuffer.WriteLine("Lorem ipsum dolor sit amet, nec fusce potenti eget egestas, mauris commodo felis vestibulum vestibulum. Urna consequat sed faucibus duis, ac non tristique tortor, eu tristique lacinia vehicula diam, consectetuer quisque felis sollicitudin. Non non diam duis, consectetuer a tortor est nisl. Accumsan pellentesque dictum nulla eget, elit non commodo turpis, libero in viverra nibh amet.");

            ConsoleEx.ScreenBuffer.ProcessedOutput = false;
            byte[] val = new byte[1];

            for (int x = 0; x < 0x100; x++)
            {
                uint Writen;
                val[0] = (byte)x;
                //WinAPI.WriteConsole(ConsoleEx.ScreenBuffer.Handle, val, 1, out Writen, IntPtr.Zero);
                WinAPI.WriteFile(ConsoleEx.ScreenBuffer.Handle, val, 1, out Writen, IntPtr.Zero);
                //ConsoleEx.ScreenBuffer.Write(x);
            }



            ConsoleEx.ScreenBuffer.ProcessedOutput = true;
            ConsoleEx.InputBuffer.LineInput = false;
            ConsoleEx.InputBuffer.ProcessedInput = false;
            ConsoleEx.InputBuffer.QuickEditMode = false;
            ConsoleEx.InputBuffer.MouseInput = true;

            ConsoleEx.ScreenBuffer.SetWindowSize(80, 25);
            ConsoleEx.ScreenBuffer.SetBufferSize(80, 25);
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
                            if (Event.KeyEvent.Character == '\r')
                                ConsoleEx.ScreenBuffer.Write('\n');
                            else
                                ConsoleEx.ScreenBuffer.Write(Event.KeyEvent.Character);
                        }
                        //else
                        //{
                        //    var Key = Event.KeyEvent;
                            
                        //    ConsoleEx.ScreenBuffer.Write("'{0}' {3:X4} {1} Rep: {2} ", Key.Character, (Key.KeyDown != 0) ? "DOWN" : "UP  ", Key.RepeatCount, (int)Key.Character);
                        //    ConsoleEx.ScreenBuffer.Write("Key: {0}, Scan: {1,2:X} ", ((System.ConsoleKey)Key.VirtualKeyCode).ToString(), Key.VirtualScanCode);
                        //    ConsoleEx.ScreenBuffer.Write("Ctrl: {0,4:X4} ", (int)Key.ControlKeyState);
                        //    ConsoleEx.ScreenBuffer.WriteLine();

                        //    //ConsoleEx.ScreenBuffer.WriteLine("'{3}' {0}:{1}-{2} {4}", Event.KeyEvent.VirtualKeyCode, Event.KeyEvent.VirtualScanCode, Event.KeyEvent.ControlKeyState.ToString(), Event.KeyEvent.Character, Event.KeyEvent.KeyDown);
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
