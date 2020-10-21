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
            // TODO: Check for a faster / more performant way to handle this.
            // Init a temp List with our current buffer.
            List<byte> _temp = new List<byte>(_buffer);
            // Add the data to the list by byte.
            _temp.AddRange(data);
            // Convert the list to set our internal buffer.
            _buffer = _temp.ToArray();
            // Clean up the temp list?
            _temp = null;
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
    }
}