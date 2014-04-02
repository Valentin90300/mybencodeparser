using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTorrent.BencodeDataParser.Tests.NParserTestStuff
{
    public class TestNElement : INElement, ITestElement<long>
    {
        public long Value
        {
            get { throw new NotImplementedException(); }
        }

        public long Data
        {
            get; set;
        }
    }

    internal class TestNElementFactory : INElementFactory
    {
        INElement INElementFactory.CreateElement(long value)
        {
            return new TestNElement() { Data = value };
        }
    }
}
