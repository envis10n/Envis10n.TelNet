using System.Collections.Generic;
using Envis10n.TelNet.Constants;
using System;
using System.Text;

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

    public class TelnetDataEvent : ITelnetEvent
    {
        public TelnetEventType EventType { get; } = TelnetEventType.DataReceive;
        public byte[] Buffer { get; }

        public TelnetDataEvent(byte[] buffer)
        {
            Buffer = Parser.UnEscapeIac(buffer);
        }
        public byte[] ToBytes()
        {
            return Parser.EscapeIac(Buffer);
        }

        public static TelnetDataEvent Build(byte[] buffer)
        {
            return new TelnetDataEvent(buffer);            
        }
    }
    public class TelnetSendEvent : ITelnetEvent
    {
        public TelnetEventType EventType { get; } = TelnetEventType.DataSend;
        public byte[] Buffer { get; }

        public TelnetSendEvent(byte[] buffer)
        {
            Buffer = Parser.EscapeIac(buffer);
        }
        public byte[] ToBytes()
        {
            return Buffer;
        }
        public static TelnetSendEvent Build(byte[] buffer)
        {
            return new TelnetSendEvent(buffer);
        }
    }
    public class TelnetDecompressEvent : ITelnetEvent
    {
        public TelnetEventType EventType { get; } = TelnetEventType.DecompressImmediate;
        public byte[] Buffer { get; }

        public TelnetDecompressEvent(byte[] buffer)
        {
            Buffer = buffer;
        }
        public byte[] ToBytes()
        {
            return Parser.EscapeIac(Buffer);
        }
        public static TelnetDecompressEvent Build(byte[] buffer)
        {
            return new TelnetDecompressEvent(buffer);            
        }
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
        public static TelnetIacEvent Build(byte command)
        {
            byte[] buffer = new byte[] {255, command};
            return new TelnetIacEvent(buffer);
        }
    }

    public class TelnetNegotiationEvent : ITelnetEvent
    {
        public TelnetEventType EventType { get; } = TelnetEventType.Negotiation;
        public byte Command { get; }
        public byte Option { get; }

        public TelnetNegotiationEvent()
        {
        }

        public TelnetNegotiationEvent(byte[] buffer)
        {
            Command = buffer[1];
            Option = buffer[2];
        }

        public byte[] ToBytes()
        {
            return new byte[] {255, Command, Option};
        }

        public static TelnetNegotiationEvent Build(byte command, byte option)
        {
            byte[] buffer = new byte[] {255, command, option};
            return new TelnetNegotiationEvent(buffer);
        }
    }

    public class TelnetSubNegotiationEvent : ITelnetEvent
    {
        public TelnetEventType EventType { get; } = TelnetEventType.SubNegotiation;
        public byte Option { get; }
        public byte[] Buffer { get; }

        public TelnetSubNegotiationEvent()
        {
        }

        public TelnetSubNegotiationEvent(byte[] buffer)
        {
            Option = buffer[2];
            Buffer = new byte[buffer.Length - 5];
            System.Buffer.BlockCopy(buffer, 3, Buffer, 0, Buffer.Length);
            Buffer = Parser.UnEscapeIac(Buffer);
        }

        public byte[] ToBytes()
        {
            byte[] data = Parser.EscapeIac(Buffer);
            byte[] temp = new byte[Buffer.Length + 5];
            temp[0] = 255;
            temp[1] = TelnetCommand.SB;
            temp[2] = Option;
            System.Buffer.BlockCopy(data, 0, temp, 3, data.Length);
            temp[^2] = 255;
            temp[^1] = TelnetCommand.SE;
            return temp;
        }
        public static TelnetSubNegotiationEvent Build(byte option, byte[] data)
        {
            data = Parser.EscapeIac(data);
            byte[] buffer = new byte[data.Length + 5];
            buffer[0] = 255;
            buffer[1] = TelnetCommand.SB;
            buffer[2] = option;
            System.Buffer.BlockCopy(data,0,buffer, 3,data.Length);
            buffer[^2] = 255;
            buffer[^1] = TelnetCommand.SE;
            return new TelnetSubNegotiationEvent(buffer);
        }
        public static TelnetSubNegotiationEvent Build(byte option, string content)
        {
            byte[] data = Encoding.UTF8.GetBytes(content);
            return Build(option, data);
        }

        public static TelnetSubNegotiationEvent Build(byte option, string content, Encoding encoding)
        {
            byte[] data = encoding.GetBytes(content);
            return Build(option, data);
        }
    }
}