using System;
using System.Collections.Generic;

namespace Envis10n.TelNet
{
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
            // Skip processing time if there is nothing to do.
            if (_buffer.Length > 0)
            {
                // TODO: Handle processing.
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