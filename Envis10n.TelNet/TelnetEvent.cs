using System.Collections.Generic;
using Envis10n.TelNet.Constants;
using System;

namespace Envis10n.TelNet
{
    public enum TelnetEventType : byte
    {
        Iac,
        Negotiation,
        SubNegotiation,
        DataReceive,
        DataSend,
        DecompressImmediate,
    }
    public interface ITelnetEvent
    {
        public TelnetEventType EventType { get; }
        public byte[] ToBytes();
    }
    public class TelnetIacEvent : ITelnetEvent
    {
        public TelnetEventType EventType { get; } = TelnetEventType.Iac;
        public byte Command { get; }
        public TelnetIacEvent() {}
        public TelnetIacEvent(byte[] buffer)
        {
            Command = buffer[1];
        }
        public byte[] ToBytes()
        {
            return new byte[] {255, Command};
        }
    }

    public class TelnetNegotiationEvent : ITelnetEvent
    {
        public TelnetEventType EventType { get; } = TelnetEventType.Negotiation;
        public byte Command { get; }
        public byte Option { get; }
        
        public TelnetNegotiationEvent() {}

        public TelnetNegotiationEvent(byte[] buffer)
        {
            Command = buffer[1];
            Option = buffer[2];
        }

        public byte[] ToBytes()
        {
            return new byte[] {255, Command, Option};
        }
    }

    public class TelnetSubNegotiationEvent : ITelnetEvent
    {
        public TelnetEventType EventType { get; } = TelnetEventType.SubNegotiation;
        public byte Option { get; }
        public byte[] Buffer { get; }
        public TelnetSubNegotiationEvent() {}

        public TelnetSubNegotiationEvent(byte[] buffer)
        {
            Option = buffer[2];
            Buffer = new byte[buffer.Length - 5];
            System.Buffer.BlockCopy(buffer, 3, Buffer, 0, Buffer.Length);
        }

        public byte[] ToBytes()
        {
            byte[] temp = new byte[Buffer.Length + 5];
            temp[0] = 255;
            temp[1] = TelnetCommand.SB;
            temp[2] = Option;
            System.Buffer.BlockCopy(Buffer, 0, temp, 3, Buffer.Length);
            temp[^2] = 255;
            temp[^1] = TelnetCommand.SE;
            return temp;
        }
    }
}