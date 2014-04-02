using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyTorrent.BencodeDataParser.Tests.BencodeDataParserTestStuff
{
    internal class TestAggregativeParser : IAggregativeParser
    {
        IElement IAggregativeParser.ParseWithAppropriateParser(BinaryReader stream)
        {
            char marker;

            try
            {
                marker = stream.ReadChar();
            }
            catch (Exception e)
            {
                throw new UnitTestException(e);
            }

            switch (marker)
            {
                case 'a':
                    return new TestElementA();
                case 'b':
                    return new TestElementB();
                default:
                    throw new UnitTestException();
            }
        }
    }

    public class TestElementA : IElement
    {
    }

    public class TestElementB : IElement
    {
    }
}