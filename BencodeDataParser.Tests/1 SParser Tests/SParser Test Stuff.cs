using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTorrent.BencodeDataParser.Tests.SParserTestStuff
{
    public class TestSElement : ISElement, ITestElement< IEnumerable<byte> >
    {
        public byte[] AsByteString
        {
            get { throw new NotImplementedException(); }
        }

        public string AsUTF8String
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<byte> Data
        {
            get; set;
        }
    }

    internal class TestSElementFactory : ISElementFactory
    {
        ISElement ISElementFactory.CreateElement(IEnumerable<byte> value)
        {
            return new TestSElement() { Data = value };
        }
    }
}
