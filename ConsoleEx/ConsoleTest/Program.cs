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
            ConsoleEx.ScreenBuffer.WriteLine("Lorem ipsum dolor sit amet, nec fusce potenti eget egestas, mauris commodo felis vestibulum vestibulum. Urna consequat sed faucibus duis, ac non tristique tortor, eu tristique lacinia vehicula diam, consectetuer quisque felis sollicitudin. Non non diam duis, consectetuer a tortor est nisl. Accumsan pellentesque dictum nulla eget, elit non commodo turpis, libero in viverra nibh amet.");
            ConsoleEx.InputBuffer.Flush();
            ScreenBuffer NewBuffer = new ScreenBuffer(ConsoleEx.ScreenBuffer, true);
            var old = ConsoleEx.SwapBuffers(NewBuffer);
            //ConsoleEx.StdOutput = NewBuffer.Handle;
            Console.Write("123456789...");
            
            ConsoleEx.ScreenBuffer.MoveBufferArea(3, 1, 50, 4, 10, 10, '!', ConsoleColor.White, ConsoleColor.Cyan);
            //ConsoleEx.ScreenBuffer.MoveBufferArea(new System.Drawing.Rectangle (3, 1, 40, 5), new System.Drawing.Rectangle (0, 0, 80, 25), new System.Drawing.Point(10, 10), '!', ConsoleColor.White, ConsoleColor.Cyan);
            ConsoleEx.ScreenBuffer.Scroll(4);


            var t = ConsoleEx.InputBuffer.Stream;
            //var t = ConsoleEx.InputBuffer.NextEvent(WinAPI.InputRecordType.KeyEvent);

            //ConsoleEx.InputBuffer.Mode &= ~ConsoleExInputMode.EchoInput;
            //ConsoleEx.InputBuffer.Mode &= ~ConsoleExInputMode.LineInput;
            //ConsoleEx.InputBuffer.Mode &= ~ConsoleExInputMode.ProcessedInput;
            
            var k = ConsoleEx.InputBuffer.ReadLine();

            //var t = ConsoleEx.Window;

            ConsoleEx.SwapBuffers(old);


        }
    }
}
