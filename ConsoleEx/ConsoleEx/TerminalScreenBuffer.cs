using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLib
{
    class TerminalScreenBuffer : ScreenBuffer
    {
        char[] DECLineDrawing = new char[] 
        { 
            (char)Characters.NonBreakSpace,     // 5F
            (char)Characters.BlackDimond,       // 60
            (char)Characters.MediumShade,       // 61
            (char)Characters.SymbolHT,          // 62
            (char)Characters.SymbolFF,          // 63
            (char)Characters.SymbolCR,          // 64  
            (char)Characters.SymbolLF,          // 65
            (char)Characters.Degree,            // 66
            (char)Characters.PlusMinus,         // 67
            (char)Characters.SymbolNL,          // 68
            (char)Characters.SymbolVT,          // 69
            (char)Characters.LowerRight,        // 6A
            (char)Characters.UpperRight,        // 6B
            (char)Characters.UpperLeft,         // 6C   
            (char)Characters.LowerLeft,         // 6D
            (char)Characters.CrossMiddle,       // 6E
            (char)Characters.ScanLine1,         // 6F
            (char)Characters.ScanLine3,         // 70
            (char)Characters.Horizontal,        // 71
            (char)Characters.ScanLine7,         // 72
            (char)Characters.ScanLine9,         // 73
            (char)Characters.CrossLeft,         // 74
            (char)Characters.CrossRight,        // 75
            (char)Characters.CrossUp,           // 76
            (char)Characters.CrossDown,         // 77
            (char)Characters.Vertical,          // 78
            (char)Characters.LessThenEqual,     // 79
            (char)Characters.GreaterThenEqual,  // 7A
            (char)Characters.Pi,                // 7B
            (char)Characters.NotEqual,          // 7C
            (char)Characters.Pound,             // 7D
            (char)Characters.MiddleDot,         // 7E
        };
        
        enum State
        {
            Data,
            ESC,
            CSI,
            CSIIntermediate,
            OSC,

            ClosingCharacter,
            SingleCode,

            CommandString,
        };

        State CurrentState;
        int CharacterSet = 0;
        
        TerminalEmulator BaseTerminal;
        
        List<char> SavedData;
        char SavedCode;

        List<char> EscapeData;

        int SavedCursorTop;
        int SavedCursorLeft;

        List<string> Params;       

        public TerminalScreenBuffer(TerminalEmulator Terminal)
        {
            BaseTerminal = Terminal;
            
            SavedData = new List<char>();
            EscapeData = new List<char>();
            Params = new List<string>();
            CurrentState = State.Data;
        }

        public TerminalScreenBuffer(TerminalEmulator Terminal, Microsoft.Win32.SafeHandles.SafeFileHandle Handle)
            : base(Handle)
        {
            BaseTerminal = Terminal;

            SavedData = new List<char>();
            EscapeData = new List<char>();
            Params = new List<string>();
            CurrentState = State.Data;
        }

        public override void Write(char Data)        
        {
            EscapeData.Add(Data);
            
            if (CurrentState == State.Data)
            {
                switch (Data)
                {
                    case (char)Characters.ENQ:  // Return Terminal Status (Ctrl-E)
                        break;

                    case (char)Characters.BS:   // Backspace (Ctrl-H)
                        CursorBackward(1);
                        break;

                    case (char)Characters.BEL:  // Bell (Ctrl-G)
                        break;

                    case (char)Characters.CR:   // Carriage Return (Ctrl-M)
                        this.CursorLeft = 0;
                        break;

                    case (char)Characters.FF:   // Form Feed or New Page (NP)
                    case (char)Characters.LF:   // Line Feed or New Line (NL)
                    case (char)Characters.VT:   // Vertical Tab (Ctrl-K)
                        if (this.CursorTop == this.BufferHeight - 1)
                            this.Scroll(1);
                        else
                            this.CursorTop++;
                        break;

                    case (char)Characters.SI:   // Shift In (Ctrl-O)
                        CharacterSet = 0;
                        break;

                    case (char)Characters.SO:   // Shift Out (Ctrl-N)
                        CharacterSet = 1;
                        break;

                    case (char)Characters.HT:   // Horizontal Tab (HT)
                        break;

                    case (char)Characters.ESC:
                        CurrentState = State.ESC;
                        SavedData.Clear();
                        Params.Clear();
                        EscapeData.Clear();

                        break;

                    default:
                        if (this.CursorLeft == this.BufferWidth - 1)
                        {
                            if (this.CursorTop == this.BufferHeight - 1)
                                this.Scroll(1);
                            else
                                this.CursorTop++;

                            this.CursorLeft = 0;
                        }
                        
                        if (BaseTerminal.CharacterSets[CharacterSet] == TerminalEmulator.CharacterSet.DECLineDrawing)
                        {
                            if(Data >= 0x5F && Data <= 0x7E)
                            {
                                base.Write(DECLineDrawing[Data - 0x5F]);
                            }
                            else
                                base.Write(Data);
                                
                        }
                        else
                        {
                            base.Write(Data);
                        }
                        break;

                }
            }
            else if (Data == 0x1b && (CurrentState != State.CommandString && CurrentState != State.OSC))
            {
                // An ESC in an ESC aborts the current one and resets it (unless it's an OSC or Command string, then we need to look for others
                SavedData.Clear();
                Params.Clear();
                EscapeData.Clear();

                CurrentState = State.ESC;
            }
            else if (CurrentState == State.ESC)
            {
                // This range (Space to \ and ?) is followed by a closing character
                if ((Data >= 0x20 && Data <= 0x2F) || Data == 0x3F)
                {
                    SavedCode = Data;
                    CurrentState = State.ClosingCharacter;
                }
                // These three ranges are paramterless commands
                else if ((Data >= 0x30 && Data <= 0x3E) || (Data >= 0x40 && Data <= 0x57) || (Data >= 0x60 && Data <= 0x7E))
                {
                    ProcessingSingleByteCommand(Data);
                    CurrentState = State.Data;
                }

                // Command strings 
                else if (Data >= 0x58 && Data <= 0x5F)
                {
                    SavedCode = Data;

                    if (Data == '[')
                        CurrentState = State.CSI;

                    else if (Data == ']')
                        CurrentState = State.OSC;

                    else
                        CurrentState = State.CommandString;
                }
                else
                {
                    CurrentState = State.Data;
                }
            }
            else if (CurrentState == State.ClosingCharacter)
            {
                 ProcessingClosingCharacterCommand(SavedCode, Data);
                
                CurrentState = State.Data;
            }
            else if (CurrentState == State.CSI || CurrentState == State.CSIIntermediate)
            {
                // Seperator
                if (Data == ';')
                {
                    if (SavedData.Count == 0)
                        Params.Add(null);
                    else
                        Params.Add(string.Concat(SavedData));

                    SavedData.Clear();
                }
                else if (CurrentState == State.CSI && (Data >= 0x30 && Data <= 0x3F))
                {
                    // Parameter Bytes
                    SavedData.Add(Data);
                }
                else if (Data >= 0x20 && Data <= 0x2F)
                {
                    // Intermediate Bytes
                    if (SavedData.Count != 0)
                    {
                        Params.Add(string.Concat(SavedData));
                        SavedData.Clear();
                    }

                    CurrentState = State.CSIIntermediate;
                    SavedData.Add(Data);
                }
                else if (Data >= 0x40 && Data <= 0x7E)
                {
                    // Final Byte
                    if (SavedData.Count != 0 && CurrentState == State.CSI)
                    {
                        Params.Add(string.Concat(SavedData));
                        SavedData.Clear();
                    }

                    ProcessingCSI(Params, SavedData, Data);
                    CurrentState = State.Data;
                }
            }
            else if (CurrentState == State.OSC)
            {
                // OSC ends with a BEL or a string terminator escape sequence.
                if (Data == 0x07 || (SavedData.LastOrDefault() == 0x1B && Data == '\\'))
                {
                    CurrentState = State.Data;
                }

                SavedData.Add(Data);
            }
            else if (CurrentState == State.CommandString)
            {
                SavedData.Add(Data);

                // Commend strings always end with the string terminator
                if (SavedData.Last() == 0x1B && Data == '\\')
                {
                    CurrentState = State.Data;
                }
            }
        }

        void ProcessingClosingCharacterCommand(char EscapeCode, char Closing)
        {
            switch (EscapeCode)
            {
                case '(':   // SCS – Select Character Set - G0
                    if (Closing == '0')
                        BaseTerminal.CharacterSets[0] = TerminalEmulator.CharacterSet.DECLineDrawing;

                    else if (Closing == 'B')
                        BaseTerminal.CharacterSets[0] = TerminalEmulator.CharacterSet.USASCII;
                    break;

                case ')':   // SCS – Select Character Set - G1
                    if (Closing == '0')
                        BaseTerminal.CharacterSets[1] = TerminalEmulator.CharacterSet.DECLineDrawing;

                    else if (Closing == 'B')
                        BaseTerminal.CharacterSets[1] = TerminalEmulator.CharacterSet.USASCII;
                    break;

                default:
                    break;
            }
        }

        void ProcessingSingleByteCommand(char Code)
        {
            switch (Code)
            {
                case '7':   // DECSC – Save Cursor                    
                    SavedCursorTop = this.CursorTop - this.WindowTop;
                    SavedCursorLeft = this.CursorLeft - this.WindowLeft;
                    break;

                case '8':   // DECRC – Restore Cursor                     
                    this.SetCursorPosition(SavedCursorLeft, SavedCursorTop);
                    break;

                case '=':   // DECKPAM – Keypad Application Mode                    
                    BaseTerminal.Keypad = TerminalEmulator.InputMode.Application;
                    break;

                case '>':   // DECKPNM – Keypad Numeric Mode                    
                    BaseTerminal.Keypad = TerminalEmulator.InputMode.Normal;
                    break;

                case 'A': // VT52 - Cursor up
                    CursorUp(1);
                    break;

                case 'B': // VT52 - Cursor Down
                    CursorDown(1);
                    break;

                case 'C': // VT52 - Cursor Right.
                    CursorForward(1);
                    break;

                case 'D': // VT52 - Cursor Left.
                    CursorBackward(1);
                    break;

                //case 'D':   // IND – Index
                    //{
                    //    if (this.CursorTop == this.BufferHeight - 1)
                    //    {
                    //        this.Scroll(1);
                    //    }
                    //    else
                    //    {
                    //        this.CursorTop++;
                    //    }
                    //}
                    //break;

                //case 'E': // NEL – Next Line
                //    {
                //        if (this.CursorTop == this.BufferHeight - 1)
                //        {
                //            this.Scroll(1);
                //        }
                //        else
                //        {
                //            this.CursorTop++;
                //        }

                //        this.CursorLeft = 0;
                //    }
                //    break;


                //case 'F':
                //    break;
                //case 'G':
                //    break;

                case 'H':   // VT52 - Move the cursor to the home position
                    SetCursor(1, 1);
                    break;

                case 'I':   // VT52 - Reverse Line Feed
                case 'M':   // RI – Reverse Index
                    {
                        if (this.CursorTop == 0)
                        {
                            this.Scroll(-1);
                        }
                        else
                        {
                            this.CursorTop--;
                        }
                    }
                    break;

                case 'J':   // VT52 - Erase from the cursor to the end of the screen
                    EraseInDisplay(0);
                    break;

                case 'K':   // VT52 - Erase from the cursor to the end of the line.
                    EraseInLine(0);
                    break;

                //case 'N':
                //    break;
                //case 'O':
                //    break;
                //case 'V':
                //    break;
                //case 'W':
                //    break;
                //case 'X':
                //    break;
                //case 'Y':
                //    break;

                case 'Z':   // VT52 - Identify
                    break;

                //case '~':
                //    break;

                default:
                    break;

            }
        }

        void CursorUp(int Step)
        {
            if (Step == 0)
                Step = 1;

            if (this.CursorTop - Step < this.WindowTop)
                this.CursorTop = this.WindowTop;
            else
                this.CursorTop -= Step;
        }

        void CursorDown(int Step)
        {
            if (Step == 0)
                Step = 1;

            if (this.CursorTop + Step >= this.WindowTop + this.WindowHeight)
                this.CursorTop = this.WindowTop + this.WindowHeight - 1;

            else
                this.CursorTop += Step;
        }

        void CursorForward(int Step)
        {
            if (Step == 0)
                Step = 1;

            if (this.CursorLeft + Step >= this.WindowLeft + this.WindowWidth)
                this.CursorLeft = this.WindowLeft + this.WindowWidth - 1;

            else
                this.CursorLeft += Step;
        }

        void CursorBackward(int Step)
        {
            if (Step == 0)
                Step = 1;

            if (this.CursorLeft - Step < this.WindowLeft)
                this.CursorLeft = this.WindowLeft;
            else
                this.CursorLeft -= Step;
        }

        void SetCursor(int Horz, int Vert)
        {
            if (Vert == 0)
                Vert = this.WindowTop;

            else if (Vert > this.WindowHeight)
                Vert = this.WindowHeight;

            if (Horz == 0)
                Horz = this.WindowLeft;

            else if (Horz > this.WindowWidth)
                Horz = this.WindowWidth;

            Vert += this.WindowTop;
            Horz += this.WindowLeft;

            this.SetCursorPosition(Horz - 1, Vert - 1);
        }

        void EraseInDisplay(int State)
        {
            Coord Pos = ScreenBufferInfo.CursorPosition;
            uint FillTotal = 0;

            if (State == 0)
            {
                // Erase from the active position to the end of the screen, inclusive (default)
                FillTotal = (uint)((this.WindowHeight - this.CursorTop) * this.WindowWidth);
                FillTotal += (uint)this.CursorLeft;
            }
            else if (State == 1)
            {
                // Erase from start of the screen to the active position, inclusive
                Pos.X = (short)this.WindowLeft;
                Pos.Y = (short)this.WindowTop;
                FillTotal = (uint)(this.CursorTop * this.WindowWidth);
                FillTotal += (uint)this.CursorLeft;
            }
            else if (State == 2)
            {
                // Erase all of the display – all lines are erased, changed to single-width, and the cursor does not move.
                Pos.X = (short)this.WindowLeft;
                Pos.Y = (short)this.WindowTop;
                FillTotal = (uint)(this.WindowHeight * this.WindowWidth);
            }

            uint FillOutput = 0;
            WinAPI.FillConsoleOutputCharacter(Handle, ' ', FillTotal, Pos, out FillOutput);
            WinAPI.FillConsoleOutputAttribute(Handle, Attribute.Value, FillTotal, Pos, out FillOutput);
        }

        void EraseInLine(int State)
        {
            Coord Pos = ScreenBufferInfo.CursorPosition;
            uint FillTotal = 0;

            if (State == 0)
            {
                // Erase from the active position to the end of the line, inclusive (default)
                FillTotal = (uint)(this.WindowWidth - this.CursorLeft);
            }
            else if (State == 1)
            {
                // Erase from the start of the screen to the active position, inclusive
                FillTotal = (uint)(this.CursorLeft);
                Pos.X = (short)this.WindowLeft;
            }
            else if (State == 2)
            {
                // Erase all of the line, inclusive
                FillTotal = (uint)(this.WindowWidth);
                Pos.X = (short)this.WindowLeft;
            }

            uint FillOutput = 0;
            WinAPI.FillConsoleOutputCharacter(Handle, ' ', FillTotal, Pos, out FillOutput);
            WinAPI.FillConsoleOutputAttribute(Handle, Attribute.Value, FillTotal, Pos, out FillOutput);
        }
        
        void ProcessingCSI(List<string> Params, List<char> Intermediate, char FinalByte)
        {
            switch (FinalByte)
            {
                case 'R':   // CPR – Cursor Position Report
                    break;

                case 'A':   // CUU – Cursor Up
                    CursorUp(GetParam(Params, 0, 1));
                    break;

                case 'B':   // CUD – Cursor Down 
                    CursorDown(GetParam(Params, 0, 1));
                    break;

                case 'C':   // CUF – Cursor Forward 
                    CursorForward(GetParam(Params, 0, 1));
                    break;

                case 'D':   // CUB - Cursor Backward
                    CursorBackward(GetParam(Params, 0, 1));
                    break;

                case 'H':   // CUP – Cursor Position
                case 'f':   // HVP – Horizontal and Vertical Position
                    SetCursor(GetParam(Params, 1, 1), GetParam(Params, 0, 1)); 
                    break;

                case 'J':   // ED – Erase In Display
                    EraseInDisplay(GetParam(Params, 0));
                    break;

                case 'K':   // EL – Erase In Line
                    EraseInLine(GetParam(Params, 0));
                    break;

                case 'h': // SM – Set Mode
                    if (Params.Count >= 1 && Params[0] == "?1")
                    {
                        BaseTerminal.CursorKeys = TerminalEmulator.InputMode.Application;
                    }
                    else if (Params.Count >= 1 && Params[0] == "?12")
                    {
                        // Blinking
                        break;
                    }
                    else if (Params.Count >= 1 && Params[0] == "?25")
                    {
                        // Show Cursor
                        base.CursorVisible = true;
                    }
                    else if (Params.Count >= 1 && Params[0] == "?1049")
                    {
                        BaseTerminal.SwitchToAlternateBuffer();
                    }
                    else
                    {
                        break;
                    }
                    
                    break;

                case 'l': // RM – Reset Mode
                    if (Params.Count >= 1 && Params[0] == "?1")
                    {
                        BaseTerminal.CursorKeys = TerminalEmulator.InputMode.Normal;
                    }
                    else if (Params.Count >= 1 && Params[0] == "?12")
                    {
                        // Don't blink
                        break;
                    }
                    else if (Params.Count >= 1 && Params[0] == "?25")
                    {
                        // Hide Cursor
                        base.CursorVisible = false;
                    }
                    else if (Params.Count >= 1 && Params[0] == "?1049")
                    {
                        BaseTerminal.SwitchToMainBuffer();
                    }
                    else
                    {
                        break;
                    }

                    break;

                case 'm': // SGR – Select Graphic Rendition
                    for (int x = 0; x < Params.Count; x++)
                    {
                        int Val = GetParam(Params, x);
                        if (Val == 0)
                        {
                            CharacterAttribute Temp = this.Attribute;
                            Temp.Foreground = ConsoleColor.Gray;
                            Temp.Background = ConsoleColor.Black;
                            this.Attribute = Temp;
                        }
                        
                        else if (Val == 1)
                        {
                            CharacterAttribute Temp = this.Attribute;
                            Temp.Value |= CharacterAttributeEnum.FOREGROUND_INTENSITY;
                            this.Attribute = Temp;
                        }
                        else if (Val == 4)
                        {
                            CharacterAttribute Temp = this.Attribute;
                            Temp.Value |= CharacterAttributeEnum.BACKGROUND_INTENSITY;
                            this.Attribute = Temp;
                        }
                        else if (Val == 24)
                        {
                            CharacterAttribute Temp = this.Attribute;
                            Temp.Value &= ~CharacterAttributeEnum.BACKGROUND_INTENSITY;
                            this.Attribute = Temp;
                        }
                        else if (Val == 2 || Val == 22)
                        {
                            CharacterAttribute Temp = this.Attribute;
                            Temp.Value &= ~CharacterAttributeEnum.FOREGROUND_INTENSITY;
                            this.Attribute = Temp;
                        }
                        else if (Val == 7 || Val == 27)
                        {
                            CharacterAttribute Temp = this.Attribute;
                            Temp.Foreground = this.Attribute.Background;
                            Temp.Background = this.Attribute.Foreground;                            
                            this.Attribute = Temp;

                        }
                        else if (Val >= 30 && Val <= 39)
                        {
                            CharacterAttribute Temp = this.Attribute;
                            if (Val == 38)
                            {
                                x++;
                                if (GetParam(Params, x) == 2)
                                {
                                }
                                else if (GetParam(Params, x) == 5)
                                {
                                    x++;
                                    Temp.Foreground = ConvertColor(GetParam(Params, x));
                                }
                            }
                            else if (Val == 39)
                            {
                                Temp.Foreground = ConsoleColor.Gray;
                            }
                            else
                            {
                                Temp.Foreground = ConvertColor(Val - 30);
                                if (this.Attribute.Foreground >= ConsoleColor.DarkGray)
                                    Temp.Foreground += 8;
                            }
                            this.Attribute = Temp;
                        }
                        else if (Val >= 40 && Val <= 49)
                        {
                            CharacterAttribute Temp = this.Attribute;
                            if (Val == 48)
                            {
                                x++;
                                if (GetParam(Params, x) == 2)
                                {
                                }
                                else if (GetParam(Params, x) == 5)
                                {
                                    x++;
                                    Temp.Background = ConvertColor(GetParam(Params, x));
                                }
                            }
                            else if (Val == 49)
                            {
                                Temp.Background = ConsoleColor.Black;
                            }
                            else
                            {
                                Temp.Background = ConvertColor(Val - 40);
                                if (this.Attribute.Background >= ConsoleColor.DarkGray)
                                    Temp.Background += 8;
                            }
                            this.Attribute = Temp;
                        }
                        else if (Val >= 90 && Val <= 97)
                        {
                            CharacterAttribute Temp = this.Attribute;
                            Temp.Foreground = ConvertColor(Val - 90 + 8);
                            this.Attribute = Temp;
                        }
                        else if (Val >= 100 && Val <= 107)
                        {
                            CharacterAttribute Temp = this.Attribute;   
                            Temp.Background = ConvertColor(Val - 100 + 8);
                            this.Attribute = Temp;
                        }
                        else
                        {
                            break;
                        }
                    }

                    break;

                case 'n':   // DSR – Device Status Report
                    {                        
                        string Response = "";
                        switch (GetParam(Params, 0))
                        {
                            case 0:
                            case 1:
                                break;

                            case 5:
                                Response = string.Format("\x1b[{0};{1}R", this.CursorTop - this.WindowTop, this.CursorLeft - this.WindowLeft);
                                break;
                            case 6:
                                Response = string.Format("\x1b[0n");
                                break;
                        }

                        BaseTerminal.SendResponse(Response);
                    }
                    break;

                case '`':
                    {
                        int Value = GetParam(Params, 0, 1);
                        if (Value == 0)
                            Value = 1;

                        if (Value > this.WindowWidth)
                            Value = this.WindowWidth;

                        this.CursorLeft = this.WindowLeft + Value - 1;
                    }
                    break;

                case 'd':
                    {
                        int Value = GetParam(Params, 0, 1);
                        if (Value == 0)
                            Value = 1;

                        if (Value > this.WindowHeight)
                            Value = this.WindowHeight;

                        this.CursorTop = this.WindowTop + Value - 1;
                    }
                    break;

                case 't':
                    BaseTerminal.SetWindowAndBufferSize(GetParam(Params, 2, 0), GetParam(Params, 1, 0));
                    break;

                default:
                    break;
            }
        }

        ConsoleColor ConvertColor(int Value)
        {
            switch(Value)
            {
                case 0:
                    return ConsoleColor.Black;
                case 1:
                    return ConsoleColor.DarkRed;
                case 2:
                    return ConsoleColor.DarkGreen;
                case 3:
                    return ConsoleColor.DarkYellow;
                case 4:
                    return ConsoleColor.DarkBlue;
                case 5:
                    return ConsoleColor.DarkMagenta;
                case 6:
                    return ConsoleColor.DarkCyan;
                case 7:
                    return ConsoleColor.Gray;
                case 8:
                    return ConsoleColor.DarkGray;
                case 9:
                    return ConsoleColor.Red;
                case 10:
                    return ConsoleColor.Green;
                case 11:
                    return ConsoleColor.Yellow;
                case 12:
                    return ConsoleColor.Blue;
                case 13:
                    return ConsoleColor.Magenta;
                case 14:
                    return ConsoleColor.Cyan;
                case 15:
                    return ConsoleColor.White;
            }

            return ConsoleColor.White;
        }
        
        int GetParam(List<string> Params, int Index, int Default = 0)
        {
            if (Index >= Params.Count)
                return Default;

            if (Params[Index] == null)
                return Default;

            int Ret;
            if (int.TryParse(Params[Index], out Ret))
                return Ret;

            return Default;
        }
        
        public override void Write(char[] Data)
        {
            // Make sure it routes to the correct buffer in case we switched between the Alternate and Main buffers
            foreach (char c in Data)
                ConsoleEx.ScreenBuffer.Write(c);
        }

        public override void Write(string Data)
        {
            // Make sure it routes to the correct buffer in case we switched between the Alternate and Main buffers
            foreach (char c in Data)
                ConsoleEx.ScreenBuffer.Write(c);
        }
    }
}
