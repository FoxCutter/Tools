using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32.SafeHandles;

namespace ConsoleLib
{
    public static class ConsoleEx
    {
        /* 
        TODO: From System.Console
        
        public static Stream OpenStandardError();
        public static Stream OpenStandardInput();
        public static Stream OpenStandardOutput();
        
        public static Stream OpenStandardError(int bufferSize);
        public static Stream OpenStandardInput(int bufferSize);
        public static Stream OpenStandardOutput(int bufferSize);        
        
        */

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

        // Direct access to the Input Buffer ignoring redirection
        static public InputBuffer InputBuffer { get; private set; }

        // Direct access to the Output Buffer ignoring redirection
        static public ScreenBuffer ScreenBuffer { get; private set; }

        static System.IO.TextReader StdInputStream = null;
        static System.IO.TextWriter StdErrorStream = null;
        static System.IO.TextWriter StdOutputStream = null;


        static WinAPI.CtrlHandlerRoutine CancelKeyPress = null;
        
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
            if (IsOutputRedirected)
                ScreenBuffer = ScreenBuffer.OpenCurrentScreenBuffer();
            else
                ScreenBuffer = new ScreenBuffer(StdOutput);

            if (IsInputRedirected)
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


        static public ScreenBuffer SwapBuffers(ScreenBuffer NewBuffer, bool SetStdOutput = true)
        {
            if (!WinAPI.SetConsoleActiveScreenBuffer(NewBuffer.Handle))
            {
                throw new ConsoleExException("ConsoleEx: Unable to setacctive output buffer.");
            }

            ScreenBuffer OldBuffer = ScreenBuffer;
            ScreenBuffer = NewBuffer;

            if (SetStdOutput)
                StdOutput = NewBuffer.Handle;

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
            if (CancelKeyPress == null)
                CancelKeyPress = new WinAPI.CtrlHandlerRoutine(CtrlHandlerFunction);

            if (!WinAPI.SetConsoleCtrlHandler(CancelKeyPress, true))
            {
                throw new ConsoleExException("ConsoleEx: Unable to set Ctrl handler.");
            }
        }

        static void DetachCtrlHandler()
        {
            if (CancelKeyPress == null)
                return;

            if (!WinAPI.SetConsoleCtrlHandler(CancelKeyPress, false))
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

        static public Microsoft.Win32.SafeHandles.SafeFileHandle StdInput
        {
            get { return GetStdHandle(WinAPI.StdHandleType.STD_INPUT_HANDLE); }
            set
            {
                if (!WinAPI.SetStdHandle(WinAPI.StdHandleType.STD_INPUT_HANDLE, value))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set standard input.");
                }

                StdInputStream = null;
            }
        }

        static public Microsoft.Win32.SafeHandles.SafeFileHandle StdOutput
        {
            get { return GetStdHandle(WinAPI.StdHandleType.STD_OUTPUT_HANDLE); }
            set
            {
                if (!WinAPI.SetStdHandle(WinAPI.StdHandleType.STD_OUTPUT_HANDLE, value))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set standard output.");
                }

                StdOutputStream = null;
            }
        }

        static public Microsoft.Win32.SafeHandles.SafeFileHandle StdError
        {
            get { return GetStdHandle(WinAPI.StdHandleType.STD_ERROR_HANDLE); }
            set
            {
                if (!WinAPI.SetStdHandle(WinAPI.StdHandleType.STD_ERROR_HANDLE, value))
                {
                    throw new ConsoleExException("ConsoleEx: Unable to set standard error.");
                }

                StdErrorStream = null;
            }
        }

        // Stream for the StdInput handle
        public static TextReader In 
        {
            get
            {
                if (StdInputStream == null)
                {
                    //StdInputStream = new System.IO.StreamReader(new System.IO.(StdInput, System.IO.FileAccess.Read));
                }

                return StdInputStream;
            }
        }

        // Stream for the StdError handle
        public static TextWriter Error 
        {
            get
            {
                if (StdErrorStream == null)
                {
                    StdErrorStream = new System.IO.StreamWriter(new System.IO.FileStream(StdError, System.IO.FileAccess.Write));
                }

                return StdErrorStream;
            }
        }

        // Stream for the StdOutput handle
        public static TextWriter Out 
        {
            get 
            {
                if (StdOutputStream == null)
                {
                    StdOutputStream = new System.IO.StreamWriter(new System.IO.FileStream(StdOutput, System.IO.FileAccess.Write));
                }

                return StdOutputStream;
            }
        }

        public static void SetIn(TextReader newIn)
        {
            StdInputStream = newIn;
        }
        
        public static void SetError(TextWriter newError)
        {
            StdErrorStream = newError;
        }

        public static void SetOut(TextWriter newOut)
        {
            StdOutputStream = newOut;
        }


        internal static IntPtr RawStdInput
        {
            get { return WinAPI.GetStdHandle(WinAPI.StdHandleType.STD_INPUT_HANDLE); }
        }

        internal static IntPtr RawStdOutput
        {
            get { return WinAPI.GetStdHandle(WinAPI.StdHandleType.STD_OUTPUT_HANDLE); }
        }

        internal static IntPtr RawStdError
        {
            get { return WinAPI.GetStdHandle(WinAPI.StdHandleType.STD_ERROR_HANDLE); }
        }
        
        public static bool IsInputRedirected
        {
            get
            {
                if (WinAPI.GetFileType(StdInput) != WinAPI.FileTypes.Character)
                    return true;

                return false;
            }
        }

        public static bool IsOutputRedirected
        {
            get
            {
                if (WinAPI.GetFileType(StdOutput) != WinAPI.FileTypes.Character)
                    return true;

                return false;
            }
        }
        
        public static bool IsErrorRedirected
        {
            get
            {
                if (WinAPI.GetFileType(StdError) != WinAPI.FileTypes.Character)
                    return true;

                return false;
            }
        }

        public static bool TreatControlCAsInput
        {
            get
            {
                return !InputBuffer.ProcessedInput;
            }

            set
            {
                InputBuffer.ProcessedInput = !value;
            }
        }
        #endregion

        internal static Microsoft.Win32.SafeHandles.SafeFileHandle GetStdHandle(WinAPI.StdHandleType StdHandle)
        {
            return new SafeFileHandle(WinAPI.GetStdHandle(StdHandle), false);
        }

        internal static Microsoft.Win32.SafeHandles.SafeFileHandle GetConsoleOutputHandle()
        {
            // Open up the handle to the buffer attached to the console, not what is attached to the handle.
            Microsoft.Win32.SafeHandles.SafeFileHandle Handle = WinAPI.CreateFile("CONOUT$", WinAPI.DesiredAccess.GENERIC_READ | WinAPI.DesiredAccess.GENERIC_WRITE,
                                                         WinAPI.ShareMode.FILE_SHARE_READ | WinAPI.ShareMode.FILE_SHARE_WRITE,
                                                         IntPtr.Zero, WinAPI.CreationDispositionType.OPEN_EXISTING, 0, IntPtr.Zero);

            return Handle;
        }

        internal static Microsoft.Win32.SafeHandles.SafeFileHandle GetConsoleInputHandle()
        {
            // Open up the handle to the buffer attached to the console, not what is attached to the handle.
            Microsoft.Win32.SafeHandles.SafeFileHandle Handle = WinAPI.CreateFile("CONIN$", WinAPI.DesiredAccess.GENERIC_READ | WinAPI.DesiredAccess.GENERIC_WRITE,
                                                        WinAPI.ShareMode.FILE_SHARE_READ | WinAPI.ShareMode.FILE_SHARE_WRITE,
                                                        IntPtr.Zero, WinAPI.CreationDispositionType.OPEN_EXISTING, 0, IntPtr.Zero);

            return Handle;
        }

        public static void Beep()
        {
            Beep(800, 200);
        }

        public static void Beep(int frequency, int duration)
        {
            WinAPI.Beep((uint)frequency, (uint)duration);
        }
    }
}
