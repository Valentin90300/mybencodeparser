using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTorrent.BencodeDataParser.Tests.BencodeDataParserTestStuff;

namespace MyTorrent.BencodeDataParser.Tests
{
    [TestClass]
    public class BencodeDataParserTest
    {
        [TestMethod]
        public void BencodeDataParsingTest()
        {
            MemoryStream memoryStream = new MemoryStream();

            memoryStream.Write
            (
                new byte[] { (byte) 'b', (byte) 'a' }, 0, 2
            );

            BencodeDataParser bencodeDataParser = new BencodeDataParser
            (
                new TestAggregativeParser(), memoryStream
            );

            IEnumerable<IElement> rootElements = null;

            try
            {
                rootElements = bencodeDataParser.Parse();
            }
            catch (Exception e)
            {
                new AssertFailedException
                (
                    "Метод BencodeDataParser.Parse сгенерировал исключение при корректных " +
                    "исходных данных в потоке.", e
                );
            }

            Assert.IsNotNull
            (
                rootElements
            );

            Assert.AreEqual<int>
            (
                2, rootElements.Count()
            );

            Assert.IsInstanceOfType
            (
                rootElements.ElementAt(0), typeof(TestElementB)
            );

            Assert.IsInstanceOfType
            (
                rootElements.ElementAt(1), typeof(TestElementA)
            );
        }
    }
}
