using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}