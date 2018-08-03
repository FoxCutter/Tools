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

        TerminalScreenBuffer CurrentBuffer;

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
            CurrentBuffer = MainBuffer;
            ConsoleEx.ScreenBuffer = MainBuffer;
            ConsoleEx.InputBuffer = InputBuffer;

            ConsoleEx.ScreenBuffer.ProcessedOutput = false;
            ConsoleEx.ScreenBuffer.WrapAtEOL = false;

            CharacterAttribute Temp = ConsoleEx.ScreenBuffer.Attribute;
            Temp.Foreground = ConsoleColor.DarkGray;
            Temp.Background = ConsoleColor.Black;
            ConsoleEx.ScreenBuffer.Attribute = Temp;

            Keypad = InputMode.Normal;
            CursorKeys = InputMode.Normal;

        }

        internal void SwitchToAlternateBuffer()
        {
            MainBufferCursorTop = MainBuffer.CursorTop - MainBuffer.WindowTop;
            MainBufferCursorLeft = MainBuffer.CursorLeft - MainBuffer.WindowLeft;

            AlternateBuffer = new TerminalScreenBuffer(this);

            AlternateBuffer.SetCursorPosition(MainBufferCursorLeft, MainBufferCursorTop);
            ConsoleEx.SwapBuffers(AlternateBuffer);
            CurrentBuffer = AlternateBuffer;
        }

        internal void SwitchToMainBuffer()
        {
            MainBuffer.SetCursorPosition(MainBufferCursorLeft + MainBuffer.WindowLeft, MainBuffer.WindowTop + MainBufferCursorTop);
            ConsoleEx.SwapBuffers(MainBuffer);
            CurrentBuffer = MainBuffer;
        }

        internal void SendResponse(string Value)
        {

        }


        public void SetWindowAndBufferSize(int Width, int Height)
        {
            ConsoleScreenBufferInfoEx Buffer = CurrentBuffer.ScreenBufferInfo;

            Buffer.Window.Width = (short)Width;
            Buffer.BufferSize.X = (short)Width;
            Buffer.MaximumWindowSize.X = (short)Width;

            Buffer.Window.Height = Height;
            if (Buffer.BufferSize.Y < Buffer.Window.Bottom)
                Buffer.BufferSize.Y = Buffer.Window.Bottom;

            Buffer.MaximumWindowSize.Y = (short)Height;


            CurrentBuffer.ScreenBufferInfo = Buffer;
            

        }


    }
}
