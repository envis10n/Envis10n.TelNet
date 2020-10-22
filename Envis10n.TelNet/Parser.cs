using System;
using System.Collections.Generic;

namespace Envis10n.TelNet
{
    using Constants;
    enum ParseState
    {
        Normal,
        Iac,
        Neg,
        Sub
    }
    public class Parser
    {
        public CompatibilityTable Options = new CompatibilityTable();
        private byte[] _buffer = new byte[0];
        public Parser() {}
        public Parser(IEnumerable<Tuple<byte, byte>> supported)
        {
            Options = new CompatibilityTable(supported);
        }
        public List<ITelnetEvent> Receive(byte[] data)
        {
            List<byte> temp = new List<byte>(_buffer);
            // Add the data to the list by byte.
            temp.AddRange(data);
            // Convert the list to set our internal buffer.
            _buffer = temp.ToArray();
            // Clean up the temp list?
            temp = null;
            return Process();
        }
        private List<ITelnetEvent> Process()
        {
            List<ITelnetEvent> events = new List<ITelnetEvent>();
            // Skip processing if there is nothing to do.
            if (_buffer.Length > 0)
            {
                ParseState iterState = ParseState.Normal;
                int cmdBegin = 0;
                for (int i = 0; i < _buffer.Length; i++)
                {
                    byte val = _buffer[i];
                    switch (iterState)
                    {
                        case ParseState.Normal:
                            if (val == TelnetCommand.IAC)
                            {
                                if (cmdBegin < i)
                                {
                                    events.Add(new TelnetDataEvent(Utility.Enumerables.Slice(_buffer, cmdBegin, i)));
                                }
                                cmdBegin = i;
                                iterState = ParseState.Iac;
                            }
                            break;
                        case ParseState.Iac:
                            switch (val)
                            {
                                case TelnetCommand.IAC:
                                    iterState = ParseState.Normal;
                                    break;
                                case TelnetCommand.GA:
                                case TelnetCommand.EOR:
                                case TelnetCommand.NOP:
                                    events.Add(new TelnetIacEvent(Utility.Enumerables.Slice(_buffer, cmdBegin, i + 1)));
                                    cmdBegin = i + 1;
                                    iterState = ParseState.Normal;
                                    break;
                                case TelnetCommand.SB:
                                    iterState = ParseState.Sub;
                                    break;
                                default:
                                    iterState = ParseState.Neg;
                                    break;
                            }
                            break;
                        case ParseState.Neg:
                            events.Add(new TelnetNegotiationEvent(Utility.Enumerables.Slice(_buffer, cmdBegin, i + 1)));
                            cmdBegin = i + 1;
                            iterState = ParseState.Normal;
                            break;
                        case ParseState.Sub:
                            if (val == TelnetCommand.SE)
                            {
                                // End of subnegotiation
                                byte opt = _buffer[cmdBegin + 2];
                                events.Add(new TelnetSubNegotiationEvent(Utility.Enumerables.Slice(_buffer, cmdBegin, i + 1)));
                                if (opt == TelnetOption.MCCP2 || opt == TelnetOption.MCCP3)
                                {
                                    // Data after requires decompression.
                                    events.Add(new TelnetDecompressEvent(Utility.Enumerables.Slice(_buffer, i + 1, _buffer.Length)));
                                    cmdBegin = _buffer.Length;
                                    i = _buffer.Length;
                                }
                                else cmdBegin = i + 1;
                                iterState = ParseState.Normal;
                            }
                            break;
                    }
                }
                if (cmdBegin < _buffer.Length)
                {
                    events.Add(new TelnetDataEvent(Utility.Enumerables.Slice(_buffer, cmdBegin,  _buffer.Length)));
                }

                _buffer = new byte[0];
            }
            return events;
        }
        public static byte[] EscapeIac(byte[] buffer)
        {
            List<byte> temp = new List<byte>();
            for (int i = 0; i < buffer.Length; i++)
            {
                byte c = buffer[i];
                temp.Add(c);
                byte n = 0;
                if (i + 1 != buffer.Length)
                {
                    n = buffer[i + 1];
                }
                if (c == 255 && n != 255)
                {
                    temp.Add(255);
                }
            }
            return temp.ToArray();
        }
        public static byte[] UnEscapeIac(byte[] buffer)
        {
            List<byte> temp = new List<byte>();
            for (int i = 0; i < buffer.Length; i++)
            {
                byte c = buffer[i];
                temp.Add(c);
                byte n = 0;
                if (i + 1 != buffer.Length)
                {
                    n = buffer[i + 1];
                }
                if (c == 255 && n == 255)
                {
                    i++;
                }
            }
            return temp.ToArray();
        }
    }
}