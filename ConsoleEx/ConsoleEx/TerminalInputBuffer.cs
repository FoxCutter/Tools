using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLib
{
    class TerminalInputBuffer : InputBuffer
    {
        TerminalEmulator BaseTerminal;

        public TerminalInputBuffer(TerminalEmulator Terminal, Microsoft.Win32.SafeHandles.SafeFileHandle Handle)
            : base(Handle)
        {
            BaseTerminal = Terminal;
        }
    }
}
