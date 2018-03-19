using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32.SafeHandles;

namespace ConsoleLib
{
    public class InputBuffer : IDisposable
    {
        Microsoft.Win32.SafeHandles.SafeFileHandle BufferHandle;
        bool FreeHandle;
        System.IO.TextReader InputStream = null;

        public InputBuffer(Microsoft.Win32.SafeHandles.SafeFileHandle Handle)
        {
            // If we're being attached to an existing handle, sanity check that it's something we can work with.
            if (WinAPI.GetFileType(Handle) != WinAPI.FileTypes.Character)
            {
                throw new Exception("Invalid handle type for Input Buffer.");
            }

            // Flag that we're attached to the handle, so we don't accidently free it.            
            FreeHandle = false;
            BufferHandle = Handle;
        }

        static internal InputBuffer OpenCurrentInputBuffer()
        {
            Microsoft.Win32.SafeHandles.SafeFileHandle Handle = ConsoleEx.GetConsoleInputHandle();

            if (Handle.IsInvalid)
                return null;
            
            InputBuffer Ret = new InputBuffer(Handle);
            
            // The buffer actually owns this handle, so make sure we free it.
            Ret.FreeHandle = true;

            return Ret;
        }

        #region Dispose/Finaize

        ~InputBuffer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool Disposing)
        {
            if (FreeHandle && !BufferHandle.IsClosed)
                BufferHandle.Close();
        }

        #endregion

        #region Properties

        internal Microsoft.Win32.SafeHandles.SafeFileHandle Handle
        {
            get { return BufferHandle; }
        }

        public System.IO.TextReader Stream
        {
            get
            {
                if (InputStream == null)
                {
                    InputStream = new System.IO.StreamReader(new WinAPI.ConsoleStream(BufferHandle, FileAccess.Read), ConsoleEx.InputEncoding);
                }

                return InputStream;
            }
        }

        public ConsoleExInputMode Mode
        {
            get
            {
                ConsoleExInputMode Ret;
                if (!WinAPI.GetConsoleInputMode(BufferHandle, out Ret))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to get input mode.");
                }
                return Ret;
            }
            set
            {
                if (!WinAPI.SetConsoleInputMode(BufferHandle, value))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set input mode.");
                }
            }
        }

        public int PendingEventCount
        {
            get
            {
                uint Count;
                if (!WinAPI.GetNumberOfConsoleInputEvents(BufferHandle, out Count))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to read pending event count");
                }

                return (int)Count;
            }
        }

        public bool KeyAvailable
        {
            get
            {
                if (PendingEventCount == 0)
                    return false;

                WinAPI.InputRecord[] Events = PeekPendingEvents();

                foreach (WinAPI.InputRecord Event in Events)
                {
                    if (Event.EventType == WinAPI.InputRecordType.KeyEvent)
                    {
                        if (Event.KeyEvent.KeyDown == true)
                            return true;
                    }
                }

                return false;
            }
        }

        public bool EchoInput
        {
            get
            {
                return (Mode & ConsoleExInputMode.EchoInput) == ConsoleExInputMode.EchoInput;
            }

            set
            {
                if (value)
                    Mode |= ConsoleExInputMode.EchoInput;
                else
                    Mode &= ~ConsoleExInputMode.EchoInput;
            }
        }

        public bool InsertMode
        {
            get
            {
                return (Mode & ConsoleExInputMode.InsertMode) == ConsoleExInputMode.InsertMode;
            }

            set
            {
                if (value)
                    Mode |= ConsoleExInputMode.InsertMode | ConsoleExInputMode.ExtendedFlags;
                else
                    Mode &= ~ConsoleExInputMode.InsertMode;
            }
        }

        public bool LineInput
        {
            get
            {
                return (Mode & ConsoleExInputMode.LineInput) == ConsoleExInputMode.LineInput;
            }

            set
            {
                if (value)
                    Mode |= ConsoleExInputMode.LineInput;
                else
                    Mode &= ~ConsoleExInputMode.LineInput;
            }
        }

        public bool MouseInput
        {
            get
            {
                return (Mode & ConsoleExInputMode.MouseInput) == ConsoleExInputMode.MouseInput;
            }

            set
            {
                if (value)
                    Mode |= ConsoleExInputMode.MouseInput;
                else
                    Mode &= ~ConsoleExInputMode.MouseInput;
            }
        }

        public bool ProcessedInput
        {
            get
            {
                return (Mode & ConsoleExInputMode.ProcessedInput) == ConsoleExInputMode.ProcessedInput;
            }

            set
            {
                if (value)
                    Mode |= ConsoleExInputMode.ProcessedInput;
                else
                    Mode &= ~ConsoleExInputMode.ProcessedInput;
            }
        }

        public bool QuickEditMode
        {
            get
            {
                return (Mode & ConsoleExInputMode.QuickEditMode) == ConsoleExInputMode.QuickEditMode;
            }

            set
            {
                if (value)
                    Mode |= ConsoleExInputMode.QuickEditMode | ConsoleExInputMode.ExtendedFlags;
                else
                    Mode &= ~ConsoleExInputMode.QuickEditMode;
            }
        }

        public bool WindowInput
        {
            get
            {
                return (Mode & ConsoleExInputMode.WindowInput) == ConsoleExInputMode.WindowInput;
            }

            set
            {
                if (value)
                    Mode |= ConsoleExInputMode.WindowInput;
                else
                    Mode &= ~ConsoleExInputMode.WindowInput;
            }
        }

        public bool VirtualTerminal
        {
            get
            {
                return (Mode & ConsoleExInputMode.VirutalTerminalInput) == ConsoleExInputMode.VirutalTerminalInput;
            }

            set
            {
                if (value)
                    Mode |= ConsoleExInputMode.VirutalTerminalInput;
                else
                    Mode &= ~ConsoleExInputMode.VirutalTerminalInput;
            }
        }

        public bool CapsLock 
        {
            get
            {
                return WinAPI.GetKeyState(0x14) != 0;
            }
        }

        public bool NumberLock
        {
            get
            {
                return WinAPI.GetKeyState(0x90) != 0;
            }
        }               

        public bool ScrollLock
        {
            get
            {
                return WinAPI.GetKeyState(0x91) != 0;
            }
        }  
        
        #endregion

        #region Events

        public void Flush()
        {
            if (!WinAPI.FlushConsoleInputBuffer(BufferHandle))
            {
                throw new ConsoleExException("ConsoleEx: Unable to flush input buffer.");
            }
        }
        
        public WinAPI.InputRecord[] PeekPendingEvents()
        {
            return PeekPendingEvents(PendingEventCount);
        }
        
        public WinAPI.InputRecord[] PeekPendingEvents(int Max)
        {
            if(Max == 0)
                return null;

            WinAPI.InputRecord[] EventList = new WinAPI.InputRecord[Max];

            uint ReadCount;
            if (!WinAPI.PeekConsoleInput(BufferHandle, EventList, (uint)Max, out ReadCount))
            {
                throw new ConsoleExException("ConsoleEx: Unable to peek at input buffer");
            }

            if (ReadCount < Max)
            {
                WinAPI.InputRecord[] NewList = new WinAPI.InputRecord[ReadCount];
                Array.Copy(EventList, NewList, ReadCount);
                EventList = NewList;
            }

            return EventList;
        }

        public WinAPI.InputRecord[] ReadPendingEvents()
        {
            return ReadPendingEvents(PendingEventCount);
        }

        public WinAPI.InputRecord[] ReadPendingEvents(int Max)
        {
            if (Max == 0)
                return null;

            WinAPI.InputRecord[] EventList = new WinAPI.InputRecord[Max];

            uint ReadCount;
            if (!WinAPI.ReadConsoleInput(BufferHandle, EventList, (uint)Max, out ReadCount))
            {
                throw new ConsoleExException("ConsoleEx: Unable to read input buffer");
            }

            if (ReadCount < Max)
            {
                WinAPI.InputRecord[] NewList = new WinAPI.InputRecord[ReadCount];
                Array.Copy(EventList, NewList, ReadCount);
                EventList = NewList;
            }

            return EventList;
        }

        public WinAPI.InputRecord NextEvent(WinAPI.InputRecordType Filter = WinAPI.InputRecordType.All)
        {
            while (PendingEventCount != 0)
            {
                WinAPI.InputRecord[] EventList = ReadPendingEvents(1);
                if (EventList == null)
                    break;

                if ((EventList[0].EventType & Filter) != WinAPI.InputRecordType.None)
                {
                    return EventList[0];
                }
            }

            return default(WinAPI.InputRecord);
        }

        public int WritePendingEvents(WinAPI.InputRecord[] Events)
        {
            if (Events == null || Events.Length == 0)
                return 0;

            uint Count;
            if (!WinAPI.WriteConsoleInput(BufferHandle, Events, (uint)Events.Length, out Count))
            {
                throw new ConsoleExException("ConsoleEx: Unable to write to input buffer");
            }

            return (int)Count;
        }

        #endregion

        #region Read functions
        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            ConsoleKeyInfo Info = ReadKey();
            if (!intercept)
                ConsoleEx.ScreenBuffer.Write(Info.KeyChar);

            return Info;
        }

        public ConsoleKeyInfo ReadKey()
        {
            WinAPI.InputRecord[] NextEvent = new WinAPI.InputRecord[1];

            uint NumberRead;
            WinAPI.KeyEventRecord KeyEvent = new WinAPI.KeyEventRecord();

            bool Done = false;
            do
            {
                if (!WinAPI.ReadConsoleInput(BufferHandle, NextEvent, 1, out NumberRead))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to read from input buffer.");
                }

                if (NextEvent[0].EventType == WinAPI.InputRecordType.KeyEvent)
                {
                    if (NextEvent[0].KeyEvent.KeyDown)
                    {
                        KeyEvent = NextEvent[0].KeyEvent;
                        Done = true;
                    }
                }
            } while (!Done);

            bool Shift = (KeyEvent.ControlKeyState & WinAPI.CtrlKeyState.SHIFT_PRESSED) != 0;
            bool Ctrl = (KeyEvent.ControlKeyState & (WinAPI.CtrlKeyState.LEFT_CTRL_PRESSED | WinAPI.CtrlKeyState.RIGHT_CTRL_PRESSED)) != 0;
            bool Alt = (KeyEvent.ControlKeyState & (WinAPI.CtrlKeyState.LEFT_ALT_PRESSED | WinAPI.CtrlKeyState.RIGHT_ALT_PRESSED)) != 0;

            return new ConsoleKeyInfo(KeyEvent.Character, (ConsoleKey)KeyEvent.VirtualKeyCode, Shift, Alt, Ctrl);
        }
        
        public int Read()
        {
            StringBuilder Data = new StringBuilder(1);

            uint DataRead;
            if (!WinAPI.ReadConsole(BufferHandle, Data, 1, out DataRead, IntPtr.Zero))
            {
                throw new ConsoleExException("ConsoleEx: Unable to read from input buffer.");
            }

            return (int)Data[0];
        }

        public string ReadLine()
        {
            StringBuilder Ret = new StringBuilder();
            StringBuilder Data = new StringBuilder(512);

            bool Done = false;

            do
            {
                uint DataRead;
                if (!WinAPI.ReadConsole(BufferHandle, Data, (uint)Data.Capacity, out DataRead, IntPtr.Zero))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to read from input buffer.");
                }

                Data.Length = (int)DataRead;
                Ret.Append(Data);

                // Check for both CL & LF. If it's processed input we'll get both CR/LF, if it's not we'll just get a CR.
                if (Data.Length > 0 && (Data[(int)(DataRead - 1)] == '\n' || Data[(int)(DataRead - 1)] == '\r'))
                    Done = true;

            } while (!Done);

            if (Ret.Length > 0 && Ret[Ret.Length - 1] == '\n')
                Ret.Length--;

            if (Ret.Length > 0 && Ret[Ret.Length - 1] == '\r')
                Ret.Length--;

            return Ret.ToString();
        }

        #endregion
    }
}
