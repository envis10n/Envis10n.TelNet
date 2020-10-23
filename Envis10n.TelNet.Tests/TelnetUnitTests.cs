using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Envis10n.TelNet.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// TODO: Add tests for negotiation and subnegotiation methods on Parser.

namespace Envis10n.TelNet.Tests
{
    [TestClass]
    public class TelnetUnitTests
    {
        [TestMethod]
        public void CompatibilityTableTest()
        {
            CompatibilityTable t = new CompatibilityTable();
            t.Support(Constants.TelnetOption.GMCP);
            CompatibilityEntry entry = t.GetOption(Constants.TelnetOption.GMCP);
            Assert.AreEqual((byte)entry,  (byte)0b0011);
        }
        [TestMethod]
        public void IacEscapeTest()
        {
            byte[] start = new byte[] {255, 42, 201, 255, 206, 25, 255, 41};
            byte[] escaped_expected = new byte[] {255, 255, 42, 201, 255, 255, 206, 25, 255, 255, 41};
            byte[] escaped = Parser.EscapeIac(start);
            byte[] unescaped = Parser.UnEscapeIac(escaped);
            Assert.AreEqual(true, escaped.SequenceEqual(escaped_expected));
            Assert.AreEqual(true, unescaped.SequenceEqual(start));
        }

        [TestMethod]
        public void ConcatTest()
        {
            byte[] a = new byte[] {0, 1, 2, 3, 4};
            byte[] b = new byte[] {5, 6, 7, 8, 9};
            byte[] expected = new byte[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
            byte[] result = Utility.Enumerables.Concat(a, b);
            Assert.AreEqual(true, result.SequenceEqual(expected));
        }
        [TestMethod]
        public void ParserTest()
        {
            Encoding encoding = Encoding.UTF8;
            Parser parser = new Parser();
            parser.Options.Support(TelnetOption.GMCP);
            parser.Options.Support(TelnetOption.MCCP2);
            byte[] gaCall = new byte[] {255, TelnetCommand.GA};
            byte[] input = Utility.Enumerables.Concat(gaCall, encoding.GetBytes("Hello, parser!"));
            List<ITelnetEvent> events = parser.Receive(input);
            foreach (ITelnetEvent ev in events)
            {
                switch (ev.EventType)
                {
                    case TelnetEventType.Iac:
                        TelnetIacEvent iacEvent = ev as TelnetIacEvent;
                        Console.WriteLine($"IAC Event: {iacEvent.Command}");
                        break;
                    case TelnetEventType.DataReceive:
                        TelnetDataEvent dataEvent = ev as TelnetDataEvent;
                        Console.WriteLine($"Data Receive: {encoding.GetString(dataEvent.Buffer)}");
                        break;
                    default:
                        Console.WriteLine($"Event not supposed to be here: {ev}");
                        break;
                }
            }
            ITelnetEvent[] evs = events.ToArray();
            Assert.AreEqual(2, evs.Length);
            Assert.AreEqual(true, evs[0] is TelnetIacEvent);
            Assert.AreEqual(true, evs[1] is TelnetDataEvent);
        }
        [TestMethod]
        public void ParserSBTest()
        {
            Encoding encoding = Encoding.UTF8;
            Parser parser = new Parser();
            parser.Options.Support(TelnetOption.GMCP);
            parser.Options.Support(TelnetOption.MCCP2);
            byte[] sbCall = new byte[] {TelnetCommand.IAC, TelnetCommand.SB, TelnetOption.MCCP2, TelnetCommand.IAC, TelnetCommand.SE};
            byte[] input = Utility.Enumerables.Concat(sbCall, encoding.GetBytes("This data would be compressed."));
            List<ITelnetEvent> events = parser.Receive(input);
            foreach (ITelnetEvent ev in events)
            {
                switch (ev.EventType)
                {
                    case TelnetEventType.SubNegotiation:
                        TelnetSubNegotiationEvent sbEvent = ev as TelnetSubNegotiationEvent;
                        Console.WriteLine($"SB Event: {sbEvent.Option}\nSB Data: {encoding.GetString(sbEvent.Buffer)}");
                        break;
                    case TelnetEventType.DecompressImmediate:
                        TelnetDecompressEvent dataEvent = ev as TelnetDecompressEvent;
                        Console.WriteLine($"Decompress IMMEDIATELY: {encoding.GetString(dataEvent.Buffer)}");
                        break;
                    default:
                        Console.WriteLine($"Event not supposed to be here: {ev}");
                        break;
                }
            }
            ITelnetEvent[] evs = events.ToArray();
            Assert.AreEqual(2, evs.Length);
            Assert.AreEqual(true, evs[0] is TelnetSubNegotiationEvent);
            Assert.AreEqual(true, evs[1] is TelnetDecompressEvent);
        }
    }
}