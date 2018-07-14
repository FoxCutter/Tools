using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLib
{
    public class TerminalEmulator
    {
        internal enum CharacterSet
        {
            DECLineDrawing,
            USASCII,
        };

        internal enum InputMode
        {
            Normal,
            Application,
        }

        internal CharacterSet[] CharacterSets = new CharacterSet[2] { CharacterSet.USASCII, CharacterSet.DECLineDrawing };

        TerminalScreenBuffer MainBuffer;
        TerminalScreenBuffer AlternateBuffer;
        TerminalInputBuffer InputBuffer;

        int MainBufferCursorTop;
        int MainBufferCursorLeft;
        
        internal InputMode Keypad;
        internal InputMode CursorKeys;

        public TerminalEmulator()
        {
            // New buffer for the current std output
            MainBuffer = new TerminalScreenBuffer(this, ConsoleEx.StdOutput);            
            InputBuffer = new TerminalInputBuffer(this, ConsoleEx.StdInput);

            // And put our buffers in control
            ConsoleEx.ScreenBuffer = MainBuffer;
            ConsoleEx.InputBuffer = InputBuffer;

            ConsoleEx.ScreenBuffer.ProcessedOutput = false;

            CharacterAttribute Temp = ConsoleEx.ScreenBuffer.Attribute;
            Temp.Foreground = ConsoleColor.DarkGray;
            Temp.Background = ConsoleColor.Black;
            ConsoleEx.ScreenBuffer.Attribute = Temp;

            Keypad = InputMode.Normal;
            CursorKeys = InputMode.Normal;

        }

        internal void SwitchToAlternateBuffer()
        {
            MainBufferCursorTop = MainBuffer.CursorTop;
            MainBufferCursorLeft = MainBuffer.CursorLeft;

            AlternateBuffer = new TerminalScreenBuffer(this);

            AlternateBuffer.SetCursorPosition(MainBufferCursorLeft, MainBufferCursorTop);
            ConsoleEx.SwapBuffers(AlternateBuffer);

        }

        internal void SwitchToMainBuffer()
        {
            MainBuffer.SetCursorPosition(MainBufferCursorLeft, MainBufferCursorTop);
            ConsoleEx.SwapBuffers(MainBuffer);
        }


    }
}
