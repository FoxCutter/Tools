using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleLib;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //ConsoleEx.ScreenBuffer.WriteLine("Lorem ipsum dolor sit amet, nec fusce potenti eget egestas, mauris commodo felis vestibulum vestibulum. Urna consequat sed faucibus duis, ac non tristique tortor, eu tristique lacinia vehicula diam, consectetuer quisque felis sollicitudin. Non non diam duis, consectetuer a tortor est nisl. Accumsan pellentesque dictum nulla eget, elit non commodo turpis, libero in viverra nibh amet.");
            ConsoleEx.ScreenBuffer.Clear();
            ConsoleEx.ScreenBuffer.SetWindowSize(80, 25);
            ConsoleEx.ScreenBuffer.SetBufferSize(80, 25);

            ConsoleEx.InputBuffer.ProcessedInput = false;

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
                        //if (Event.KeyEvent.KeyDown != 0 || Event.KeyEvent.VirtualKeyCode == 0x12)
                        {
                            if (ConsoleEx.InputBuffer.CanDisplay(Event.KeyEvent))
                                ConsoleEx.ScreenBuffer.Write(Event.KeyEvent.Character);

                            //var Key = Event.KeyEvent;

                            //ConsoleEx.ScreenBuffer.Write("'{0}' {1} Rep: {2} ", Key.Character, (Key.KeyDown != 0) ? "DOWN" : "UP  ", Key.RepeatCount);
                            //ConsoleEx.ScreenBuffer.Write("Key: {0,2:X}, Scan: {1,2:X} ", Key.VirtualKeyCode, Key.VirtualScanCode);
                            //ConsoleEx.ScreenBuffer.Write("Ctrl: {0,4:X4} ", (int)Key.ControlKeyState);

                            //ConsoleEx.ScreenBuffer.WriteLine();
                            //System.Threading.Thread.Sleep(100);
                            
                            //ConsoleEx.ScreenBuffer.WriteLine("'{3}' {0}:{1}-{2} {4}", Event.KeyEvent.VirtualKeyCode, Event.KeyEvent.VirtualScanCode, Event.KeyEvent.ControlKeyState.ToString(), Event.KeyEvent.Character, Event.KeyEvent.KeyDown);
                        }
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
