using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Envis10n.TelNet;

namespace Envis10n.TelNet.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            CompatibilityTable t = new CompatibilityTable();
            t.Support(Constants.TelnetOption.GMCP);
            CompatibilityEntry entry = t.GetOption(Constants.TelnetOption.GMCP);
            Assert.AreEqual((byte)entry,  (byte)0b0011);
        }
    }
}