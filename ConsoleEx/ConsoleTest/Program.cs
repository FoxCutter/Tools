using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleLib;

using System.Runtime.InteropServices;


namespace ConsoleTest
{
    class Program
    {
        static char[] BoxDrawingSingle = new char[] 
        { 
            (char)Characters.UpperLeft,
            (char)Characters.UpperRight,
            (char)Characters.LowerLeft,
            (char)Characters.LowerRight,
            (char)Characters.Vertical,
            (char)Characters.Horizontal,
            (char)Characters.CrossLeft,
            (char)Characters.CrossRight,
            (char)Characters.CrossDown,
            (char)Characters.CrossUp,
            (char)Characters.CrossMiddle,
            ' ',
        };

        static char[] BoxDrawingDouble = new char[] 
        { 
            (char)Characters.DoubleUpperLeft,
            (char)Characters.DoubleUpperRight,
            (char)Characters.DoubleLowerLeft,
            (char)Characters.DoubleLowerRight,
            (char)Characters.DoubleVertical,
            (char)Characters.DoubleHorizontal,
            (char)Characters.DoubleCrossLeft,
            (char)Characters.DoubleCrossRight,
            (char)Characters.DoubleCrossDown,
            (char)Characters.DoubleCrossUp,
            (char)Characters.DoubleCrossMiddle,
            (char)Characters.LightShade,
        };

        static char[] BoxDrawingDoubleHor = new char[] 
        { 
            (char)Characters.DoubleHorUpperLeft,
            (char)Characters.DoubleHorUpperRight,
            (char)Characters.DoubleHorLowerLeft,
            (char)Characters.DoubleHorLowerRight,
            (char)Characters.Vertical,
            (char)Characters.DoubleHorizontal,
            (char)Characters.DoubleHorCrossLeft,
            (char)Characters.DoubleHorCrossRight,
            (char)Characters.DoubleHorCrossDown,
            (char)Characters.DoubleHorCrossUp,
            (char)Characters.DoubleHorCrossMiddle,
            (char)Characters.MediumShade,
        };


        static char[] BoxDrawingDoubleVer = new char[] 
        { 
            (char)Characters.DoubleVerUpperLeft,
            (char)Characters.DoubleVerUpperRight,
            (char)Characters.DoubleVerLowerLeft,
            (char)Characters.DoubleVerLowerRight,
            (char)Characters.DoubleVertical,
            (char)Characters.Horizontal,
            (char)Characters.DoubleVerCrossLeft,
            (char)Characters.DoubleVerCrossRight,
            (char)Characters.DoubleVerCrossDown,
            (char)Characters.DoubleVerCrossUp,
            (char)Characters.DoubleVerCrossMiddle,
            (char)Characters.DarkShade,
        };


        static char[] BoxDrawingText = new char[] 
        { 
            '+',
            '+',
            '+',
            '+',
            '|',
            '-',
            '+',
            '+',
            '+',
            '+',
            '+',
            '#',
        };

        static void DrawBox(char[] Elements)
        {
            //0┌  1┐  2└  3┘  4│  5─  6├  7┤  8┬  9┴  10┼ 11(blank)

            // ┌─┬─┐
            ConsoleEx.ScreenBuffer.Write(Elements[0]);
            ConsoleEx.ScreenBuffer.Write(Elements[5]);
            ConsoleEx.ScreenBuffer.Write(Elements[5]);
            ConsoleEx.ScreenBuffer.Write(Elements[8]);
            ConsoleEx.ScreenBuffer.Write(Elements[5]);
            ConsoleEx.ScreenBuffer.Write(Elements[5]);
            ConsoleEx.ScreenBuffer.Write(Elements[1]);
            ConsoleEx.ScreenBuffer.WriteLine();

            // │ │ │
            ConsoleEx.ScreenBuffer.Write(Elements[4]);
            ConsoleEx.ScreenBuffer.Write(Elements[11]);
            ConsoleEx.ScreenBuffer.Write(Elements[11]);
            ConsoleEx.ScreenBuffer.Write(Elements[4]);
            ConsoleEx.ScreenBuffer.Write(Elements[11]);
            ConsoleEx.ScreenBuffer.Write(Elements[11]);
            ConsoleEx.ScreenBuffer.Write(Elements[4]);
            ConsoleEx.ScreenBuffer.WriteLine();

            // ├─┼─┤
            ConsoleEx.ScreenBuffer.Write(Elements[6]);
            ConsoleEx.ScreenBuffer.Write(Elements[5]);
            ConsoleEx.ScreenBuffer.Write(Elements[5]);
            ConsoleEx.ScreenBuffer.Write(Elements[10]);
            ConsoleEx.ScreenBuffer.Write(Elements[5]);
            ConsoleEx.ScreenBuffer.Write(Elements[5]);
            ConsoleEx.ScreenBuffer.Write(Elements[7]);
            ConsoleEx.ScreenBuffer.WriteLine();

            // │ │ │
            ConsoleEx.ScreenBuffer.Write(Elements[4]);
            ConsoleEx.ScreenBuffer.Write(Elements[11]);
            ConsoleEx.ScreenBuffer.Write(Elements[11]);
            ConsoleEx.ScreenBuffer.Write(Elements[4]);
            ConsoleEx.ScreenBuffer.Write(Elements[11]);
            ConsoleEx.ScreenBuffer.Write(Elements[11]);
            ConsoleEx.ScreenBuffer.Write(Elements[4]);
            ConsoleEx.ScreenBuffer.WriteLine();

            // └─┴─┘
            ConsoleEx.ScreenBuffer.Write(Elements[2]);
            ConsoleEx.ScreenBuffer.Write(Elements[5]);
            ConsoleEx.ScreenBuffer.Write(Elements[5]);
            ConsoleEx.ScreenBuffer.Write(Elements[9]);
            ConsoleEx.ScreenBuffer.Write(Elements[5]);
            ConsoleEx.ScreenBuffer.Write(Elements[5]);
            ConsoleEx.ScreenBuffer.Write(Elements[3]);
            ConsoleEx.ScreenBuffer.WriteLine();
        }

        static void Main(string[] args)
        {

            DrawBox(BoxDrawingSingle);
            DrawBox(BoxDrawingDouble);
            DrawBox(BoxDrawingDoubleHor);
            DrawBox(BoxDrawingDoubleVer);
            DrawBox(BoxDrawingText);
            
            ConsoleEx.InputBuffer.LineInput = false;
            ConsoleEx.InputBuffer.ProcessedInput = false;
            ConsoleEx.InputBuffer.QuickEditMode = false;
            ConsoleEx.InputBuffer.MouseInput = true;
            //var a = ConsoleEx.InputBuffer.ReadLine();

            //ConsoleEx.ScreenBuffer.ProcessedOutput = false;
            ConsoleEx.ScreenBuffer.SetWindowSize(80, 25);
            ConsoleEx.ScreenBuffer.SetBufferSize(80, 125);
            ConsoleEx.ScreenBuffer.Clear();
            
            InputRecord Event = new InputRecord();
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
                        if ((Event.MouseEvent.EventFlags & MouseEventFlags.DOUBLE_CLICK) != 0)
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
