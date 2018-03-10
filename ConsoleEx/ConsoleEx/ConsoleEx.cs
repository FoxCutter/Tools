using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleLib
{
    public static class ConsoleEx
    {
        #region Event objcets

        public class ConsoleControlEventHandlerArgs : EventArgs
        {
            public readonly ConsoleExCtrlEventType EventType;
            public bool CancelProcessing = false;

            public ConsoleControlEventHandlerArgs(ConsoleExCtrlEventType NewEventType)
            {
                EventType = NewEventType;
            }
        }

        public delegate void ConsoleControlEventHandler(object sender, ConsoleControlEventHandlerArgs e);
        public static event ConsoleControlEventHandler ConsoleControlEvent = null;

        #endregion

        static public InputBuffer InputBuffer { get; private set; }
        static public ScreenBuffer ScreenBuffer { get; private set; }


        static WinAPI.CtrlHandlerRoutine CtrlHandlerRoutineCallback = null;
        
        static ConsoleEx()
        {
            if (Window != IntPtr.Zero)
            {
                SetupConsole();
                AttachCtrlHandler();
            }
        }

        static void SetupConsole()
        {
            if (StdOutputRedirected)
                ScreenBuffer = ScreenBuffer.OpenCurrentScreenBuffer();
            else
                ScreenBuffer = new ScreenBuffer(StdOutput);

            if (StdInputRedirected)
                InputBuffer = InputBuffer.OpenCurrentInputBuffer();
            else
                InputBuffer = new InputBuffer(StdInput);
        }
        
        public static void AllocConsole()
        {
            if (!WinAPI.AllocConsole())
            {
                throw new ConsoleExException("ConsoleEx: Unable to attach Allocate a new console.");
            }

            SetupConsole();
            AttachCtrlHandler();
        }

        public static void FreeConsole()
        {
            if (!WinAPI.FreeConsole())
            {
                throw new ConsoleExException("Unable to set Alloc Console.");
            }

            DetachCtrlHandler();

            InputBuffer = null;
            ScreenBuffer = null;
        }

        static public void AttachConsoleToProcess(uint ProcessID)
        {
            if (!WinAPI.AttachConsole(ProcessID))
            {
                throw new ConsoleExException("ConsoleEx: Unable to attach process to console.");
            }
        }
        
        static public List<uint> GetAttachedProcessList()
        {
            uint[] Data = new uint[1];

            uint Len = WinAPI.GetConsoleProcessList(Data, (uint)Data.Length);
            if (Len > Data.Length)
            {
                Data = new uint[Len];
                Len = WinAPI.GetConsoleProcessList(Data, (uint)Data.Length);
            }

            if (Len == 0)
            {
                throw new ConsoleExException("ConsoleEx: Unable to get attached process list.");
            }

            List<uint> Ret = new List<uint>(Data);

            return Ret;
        }


        static public ScreenBuffer SwapBuffers(ScreenBuffer NewBuffer)
        {
            if (!WinAPI.SetConsoleActiveScreenBuffer(NewBuffer.Handle))
            {
                throw new ConsoleExException("ConsoleEx: Unable to setacctive output buffer.");
            }

            ScreenBuffer OldBuffer = ScreenBuffer;
            ScreenBuffer = NewBuffer;
            return OldBuffer;
        }


        #region Alias Functions

        public static void AddAlias(string AliasName, string AliasValue, string AliasExe)
        {
            if (!WinAPI.AddConsoleAlias(AliasName, AliasValue, AliasExe))
            {
                throw new ConsoleExException("ConsoleEx: Unable to Add Alias.");
            }
        }

        public static string GetAlias(string AliasName, string AliasExe)
        {
            StringBuilder Data = new StringBuilder(500);

            // Buffer length in in bytes
            if (WinAPI.GetConsoleAlias(AliasName, Data, (uint)Data.Capacity * sizeof(char), AliasExe) == 0)
            {
                throw new ConsoleExException("ConsoleEx: Unable to Get Alias.");
            }

            return Data.ToString();
        }

        public static Dictionary<string, string> GetAliases(string AliasExe)
        {
            Dictionary<string, string> Ret = new Dictionary<string, string>();

            uint Length = WinAPI.GetConsoleAliasesLength(AliasExe);

            char[] Aliases = new char[Length];

            // Buffer length in in bytes
            if (WinAPI.GetConsoleAliases(Aliases, Length * sizeof(char), AliasExe) == 0)
            {
                throw new ConsoleExException("ConsoleEx: Unable to set Get Aliases.");
            }

            // The results come out in name=value pairs, seperated by Nulls.
            int Start = 0;
            int PairSplit = 0;

            try
            {

                for (int Pos = 0; Pos < Aliases.Length; Pos++)
                {
                    if (Aliases[Pos] == '=')
                    {
                        PairSplit = Pos;
                    }

                    if (Aliases[Pos] == '\0')
                    {
                        if (Pos != Start)
                        {
                            string Value = new string(Aliases, Start, PairSplit - Start);
                            string Data = new string(Aliases, PairSplit + 1, Pos - PairSplit - 1);
                            Ret.Add(Value, Data);
                        }
                        else
                        {
                            break;
                        }

                        Start = Pos + 1;
                    }
                }
            }
            catch (Exception e)
            {
                throw new ConsoleExException("ConsoleEx: Unable to set parse Aliases.", e);
            }

            return Ret;
        }

        public static List<string> GetAliesesExes()
        {
            List<string> Ret = new List<string>();

            uint Length = WinAPI.GetConsoleAliasExesLength() * 2;

            char[] Aliases = new char[Length];
            // Buffer length in in bytes
            if (WinAPI.GetConsoleAliasExes(Aliases, Length * sizeof(char)) == 0)
            {
                throw new ConsoleExException("ConsoleEx: Unable to Get Exe list.");
            }

            // The results come out seperated by Nulls.
            int Start = 0;

            try
            {
                for (int Pos = 0; Pos < Aliases.Length; Pos++)
                {
                    if (Aliases[Pos] == '\0')
                    {
                        if (Pos != Start)
                        {
                            string Value = new string(Aliases, Start, Pos - Start);
                            Ret.Add(Value);
                        }
                        else
                        {
                            break;
                        }

                        Start = Pos + 1;
                    }
                }
            }
            catch (Exception e)
            {
                throw new ConsoleExException("ConsoleEx: Unable to set parse Exe list.", e);
            }

            return Ret;
        }

        #endregion

        #region Ctrl Handler and Event functions

        static void AttachCtrlHandler()
        {
            if (CtrlHandlerRoutineCallback == null)
                CtrlHandlerRoutineCallback = new WinAPI.CtrlHandlerRoutine(CtrlHandlerFunction);

            if (!WinAPI.SetConsoleCtrlHandler(CtrlHandlerRoutineCallback, true))
            {
                throw new ConsoleExException("ConsoleEx: Unable to set Ctrl handler.");
            }
        }

        static void DetachCtrlHandler()
        {
            if (CtrlHandlerRoutineCallback == null)
                return;

            if (!WinAPI.SetConsoleCtrlHandler(CtrlHandlerRoutineCallback, false))
            {
                throw new ConsoleExException("ConsoleEx: Unable to remove Ctrl handler.");
            }
        }        
        
        static bool CtrlHandlerFunction(ConsoleExCtrlEventType CtrlType)
        {
            if (ConsoleControlEvent != null)
            {
                ConsoleControlEventHandlerArgs Events = new ConsoleControlEventHandlerArgs(CtrlType);
                OnConsoleControlEvent(Events);

                return Events.CancelProcessing;
            }

            return false;
        }

        public static void OnConsoleControlEvent(ConsoleControlEventHandlerArgs e)
        {
            if (ConsoleControlEvent != null)
            {
                ConsoleControlEvent(null, e);
            }
        }

        public static void GenerateCtrlEvent(ConsoleExCtrlEventType Event, uint ProcessGroupId)
        {
            if (!WinAPI.GenerateConsoleCtrlEvent(Event, ProcessGroupId))
            {
                throw new ConsoleExException("ConsoleEx: Unable to generate a control event.");
            }
        }

        #endregion

        #region Properties

        public static IntPtr Window
        {
            get { return WinAPI.GetConsoleWindow(); }
        }

        public static ConsoleExDisplayModeFlags DisplayMode
        {
            get 
            {
                ConsoleExDisplayModeFlags Ret;
                if (!WinAPI.GetConsoleDisplayMode(out Ret))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to get display mode.");
                }

                return Ret;
            }
        }

        public static int NumberOfMouseButtons
        {
            get
            {
                uint Ret;
                if (!WinAPI.GetNumberOfConsoleMouseButtons(out Ret))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to get mouse buttons.");
                }

                return (int)Ret;
            }
        }
        
        public static string Title
        {
            get 
            {
                StringBuilder Data = new StringBuilder(256);
                // GetConsoleTitle uses byte count not character count (this is an error in the documentation) 
                if(WinAPI.GetConsoleTitle(Data, (uint)Data.Capacity * sizeof(char)) == 0)
                {
                    throw new ConsoleExException("ConsoleEx: Unable to get title.");
                }
                return Data.ToString();
            }

            set
            {
                if (!WinAPI.SetConsoleTitle(value))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set title.");
                }
            }
        }

        public static string OriginalTitle
        {
            get
            {
                StringBuilder Data = new StringBuilder(256);
                if (WinAPI.GetConsoleOriginalTitle(Data, (uint)Data.Capacity) == 0)
                {
                    throw new ConsoleExException("ConsoleEx: Unable to get orginial title.");
                }
                return Data.ToString();
            }
        }

        public static Encoding InputEncoding
        {
            get 
            {
                return Encoding.GetEncoding((int)WinAPI.GetConsoleCP());
            }
            set 
            {
                if (!WinAPI.SetConsoleCP((uint)value.CodePage))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set input code page.");
                }
            }
        }
        
        public static Encoding OutputEncoding
        {
            get 
            {
                return Encoding.GetEncoding((int)WinAPI.GetConsoleOutputCP());
            }
            set 
            {
                if (!WinAPI.SetConsoleOutputCP((uint)value.CodePage))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set input code page.");
                }
            }
        }

        public static WinAPI.ConsoleSelectionInfo ConsoleSelectionInfo
        {
            get
            {
                WinAPI.ConsoleSelectionInfo Info;
                if (!WinAPI.GetConsoleSelectionInfo(out Info))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set console info.");
                }
                return Info;
            }
        }
      
        public static IntPtr StdInput
        {
            get { return WinAPI.GetStdHandle(WinAPI.StdHandleType.STD_INPUT_HANDLE); }
            set
            {
                if (!WinAPI.SetStdHandle(WinAPI.StdHandleType.STD_INPUT_HANDLE, value))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set standard input.");
                }
            }
        }
        
        public static IntPtr StdOutput
        {
            get { return WinAPI.GetStdHandle(WinAPI.StdHandleType.STD_OUTPUT_HANDLE); }
            set
            {
                if (!WinAPI.SetStdHandle(WinAPI.StdHandleType.STD_OUTPUT_HANDLE, value))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set standard output.");
                }
            }
        }
                
        public static IntPtr StdError
        {
            get { return WinAPI.GetStdHandle(WinAPI.StdHandleType.STD_ERROR_HANDLE); }
            set
            {
                if (!WinAPI.SetStdHandle(WinAPI.StdHandleType.STD_ERROR_HANDLE, value))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set standard error.");
                }
            }
        }

        public static bool StdInputRedirected
        {
            get
            {
                if (WinAPI.GetFileType(StdInput) != WinAPI.FileTypes.Character)
                    return true;

                return false;
            }
        }

        public static bool StdOutputRedirected
        {
            get
            {
                if (WinAPI.GetFileType(StdOutput) != WinAPI.FileTypes.Character)
                    return true;

                return false;
            }
        }
        
        public static bool StdErrorRedirected
        {
            get
            {
                if (WinAPI.GetFileType(StdError) != WinAPI.FileTypes.Character)
                    return true;

                return false;
            }
        }

        internal static IntPtr GetConsoleOutputHandle()
        {
            // Open up the handle to the buffer attached to the console, not what is attached to the handle.
            IntPtr Handle = WinAPI.CreateFile("CONOUT$", WinAPI.DesiredAccess.GENERIC_READ | WinAPI.DesiredAccess.GENERIC_WRITE,
                                                         WinAPI.ShareMode.FILE_SHARE_READ | WinAPI.ShareMode.FILE_SHARE_WRITE,
                                                         IntPtr.Zero, WinAPI.CreationDispositionType.OPEN_EXISTING, 0, IntPtr.Zero);

            return Handle;
        }

        internal static IntPtr GetConsoleInputHandle()
        {
            // Open up the handle to the buffer attached to the console, not what is attached to the handle.
            IntPtr Handle = WinAPI.CreateFile("CONIN$", WinAPI.DesiredAccess.GENERIC_READ | WinAPI.DesiredAccess.GENERIC_WRITE,
                                                        WinAPI.ShareMode.FILE_SHARE_READ | WinAPI.ShareMode.FILE_SHARE_WRITE,
                                                        IntPtr.Zero, WinAPI.CreationDispositionType.OPEN_EXISTING, 0, IntPtr.Zero);

            return Handle;
        }

        
        #endregion
    }
}
