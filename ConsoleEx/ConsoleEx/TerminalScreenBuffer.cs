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
            (char)Characters.NonBreakSpace,
            (char)0x25C6,
            (char)0x2592,
            (char)0x2409,
            (char)0x240C,
            (char)0x240D,
            (char)0x240A,
            (char)Characters.Degree,
            (char)Characters.PlusMinus,
            (char)0x2424,
            (char)0x240B,
            (char)Characters.LowerRight,
            (char)Characters.UpperRight,
            (char)Characters.UpperLeft,
            (char)Characters.LowerLeft,
            (char)Characters.CrossMiddle,
            (char)0x23BA,
            (char)0x23BB,
            (char)Characters.Horizontal,
            (char)0x23BC,
            (char)0x23BD,
            (char)Characters.CrossLeft,
            (char)Characters.CrossRight,
            (char)Characters.CrossUp,
            (char)Characters.CrossDown,
            (char)Characters.Vertical,
            (char)Characters.LessThenEqual,
            (char)Characters.GreaterThenEqual,
            (char)Characters.Pi,
            (char)0x2260,
            (char)Characters.Pound,
            (char)Characters.MiddleDot,
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
                    case (char)Characters.BS:
                        if (this.CursorLeft == 0)
                        {
                            if (this.CursorTop != 0)
                            {
                                this.CursorTop--;
                                this.CursorLeft = this.BufferWidth - 1;
                            }
                        }
                        else
                        {
                            this.CursorLeft--;
                        }
                        break;

                    case (char)Characters.BEL:
                        break;
                    
                    case (char)Characters.CR:
                        this.CursorLeft = 0;
                        break;

                    case (char)Characters.FF:
                    case (char)Characters.LF:
                    case (char)Characters.VT:
                        if (this.CursorTop == this.BufferHeight - 1)
                            this.Scroll(1);
                        else
                            this.CursorTop++;
                        break;

                    case (char)Characters.SI:
                        CharacterSet = 0;
                        break;

                    case (char)Characters.SO:
                        CharacterSet = 1;
                        break;

                    case (char)Characters.HT:
                        break;

                    case (char)Characters.ESC:
                        CurrentState = State.ESC;
                        SavedData.Clear();
                        Params.Clear();
                        EscapeData.Clear();

                        break;

                    default:
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
                    EscapeData.Clear();
                }
            }
            else if (CurrentState == State.OSC)
            {
                // OSC ends with a BEL or a string terminator escape sequence.
                if (Data == 0x07 || (SavedData.Last() == 0x1B && Data == '\\'))
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
                case '(':
                    if (Closing == '0')
                        BaseTerminal.CharacterSets[0] = TerminalEmulator.CharacterSet.DECLineDrawing;

                    else if (Closing == 'B')
                        BaseTerminal.CharacterSets[0] = TerminalEmulator.CharacterSet.USASCII;
                    break;

                case '-':
                case ')':
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
                case '7':
                    SavedCursorTop = this.CursorTop;
                    SavedCursorLeft = this.CursorLeft;
                    break;

                case '8':
                    this.SetCursorPosition(SavedCursorLeft, SavedCursorTop);
                    break;

                case '=':
                    BaseTerminal.Keypad = TerminalEmulator.InputMode.Application;
                    break;
                case '>':
                    BaseTerminal.Keypad = TerminalEmulator.InputMode.Normal;
                    break;

                //case 'A':
                //    break;
                //case 'B':
                //    break;
                //case 'C':
                //    break;
                //case 'D':
                //    break;
                //case 'E':
                //    break;
                //case 'F':
                //    break;
                //case 'H':
                //    break;
                //case 'M':
                //    break;
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
                //case 'Z':
                //    break;

                //case '~':
                //    break;

                default:
                    break;

            }
        }

        void ProcessingCSI(List<string> Params, List<char> Intermediate, char FinalByte)
        {
            switch (FinalByte)
            {
                case 'A':
                    {
                        int Step = GetParam(Params, 0, 1);
                        this.CursorTop -= Step;
                    }
                    break;

                case 'B':
                    {
                        int Step = GetParam(Params, 0, 1);
                        this.CursorTop += Step;
                    }
                    break;

                case 'C':
                    {
                        int Step = GetParam(Params, 0, 1);
                        this.CursorLeft += Step;
                    }
                    break;

                case 'D':
                    {
                        int Step = GetParam(Params, 0, 1);
                        this.CursorLeft -= Step;
                    }
                    break;

                case 'H':
                    {
                        int XPos = GetParam(Params, 0, 1);
                        int YPos = GetParam(Params, 1, 1);
                        this.SetCursorPosition(YPos - 1, XPos - 1);
                    }
                    break;

                case 'J':
                    {
                        int State = GetParam(Params, 0);
                        if (State == 0)
                        {
                            // Current position to the end of the page
                            break;
                        }
                        else if (State == 1)
                        {
                            // Start of the page to (and including) the current position
                            break;
                        }
                        else if (State == 2)
                        {
                            // Clear the screen
                            this.Clear(false);
                        }
                    }
                    break;

                case 'K':
                    {
                        int State = GetParam(Params, 0);
                        if (State == 0)
                        { 
                            // Erase from Cursor to end of line
                            int Len = this.BufferWidth - this.CursorLeft;
                            for (int x = 0; x < Len; x++)
                            {
                                this.WritePos(' ', this.CursorLeft+x, this.CursorTop);
                            }
                        }
                        else if (State == 1)
                        { 
                            // Erase from start of line to (and including) Cursor
                            break;
                        }
                        else if (State == 2)
                        {
                            // Erase Line
                            break;
                        }
                    }
                    break;

                case 'h':
                    if (Params.Count >= 1 && Params[0] == "?1")
                    {
                        BaseTerminal.CursorKeys = TerminalEmulator.InputMode.Application;
                    }
                    else if (Params.Count >= 1 && Params[0] == "?12")
                    {
                        // Blinking
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
                    
                    break;
                case 'l':
                    if (Params.Count >= 1 && Params[0] == "?1")
                    {
                        BaseTerminal.CursorKeys = TerminalEmulator.InputMode.Normal;
                    }
                    else if (Params.Count >= 1 && Params[0] == "?12")
                    {
                        // Don't blink
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

                    break;

                case 'm':
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
            // Make sure it routes to the corrcet buffer in case we switched between the Alternate and Main buffers
            foreach (char c in Data)
                ConsoleEx.ScreenBuffer.Write(c);
        }

        public override void Write(string Data)
        {
            // Make sure it routes to the corrcet buffer in case we switched between the Alternate and Main buffers
            foreach (char c in Data)
                ConsoleEx.ScreenBuffer.Write(c);
        }
    }
}
