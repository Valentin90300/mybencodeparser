using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyTorrent.BencodeDataParser.Tests.DParserTestStuff
{
    public class TestDElement : IDElement, ITestElement
    <
        IEnumerable<KeyValuePair<ISElement, IElement>>
    >
    {
        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public IElement this[ISElement key]
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerator<KeyValuePair<ISElement, IElement>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(ISElement key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<ISElement, IElement>> Data
        {
            get; set;
        }
    }

    internal class TestDElementFactory : IDElementFactory
    {
        IDElement IDElementFactory.CreateElement
        (
            IEnumerable<KeyValuePair<ISElement, IElement>> value
        )
        {
            return new TestDElement() { Data = value };
        }
    }

    internal class TestAggregativeParser : IAggregativeParser
    {
        /// <summary>
        /// Данное свойство позволяет методу Parse парсера DParser делать рекурсивный вызов
        /// (через IAggregativeParser.ParseWithAppropriateParser).
        /// </summary>
        internal IElementParser<IElement> DParser { get; set; }

        IElement IAggregativeParser.ParseWithAppropriateParser(BinaryReader stream)
        {
            int readedValue = -1;

            try
            {
                readedValue = stream.PeekChar();
            }
            catch (Exception e)
            {
                throw new UnitTestException(e);
            }

            if (readedValue == -1)
            {
                throw new UnitTestException();
            }

            char marker = (char) readedValue;

            Func<IElement, IElement> IncrementAndReturn = (element) =>
            {
                try
                {
                    stream.BaseStream.Position ++;
                }
                catch (Exception e)
                {
                    throw new UnitTestException(e);
                }

                return element;
            };

            switch (marker)
            {
                case 'x':
                    return IncrementAndReturn( new TestElementX() );
                case 'y':
                    return IncrementAndReturn( new TestElementY() );
                case 'a':
                    return IncrementAndReturn( new TestElementA() );
                case 'b':
                    return IncrementAndReturn( new TestElementB() );
                case 'w':
                    throw new InvalidMarkerException(1);
                case 'd':
                    return DParser.Parse(stream);
                default:
                    throw new UnitTestException();
            }
        }
    }

    public class TestElementX : ISElement
    {
        public byte[] AsByteString
        {
            get { throw new NotImplementedException(); }
        }

        public string AsUTF8String
        {
            get { return "x"; }
        }
    }

    public class TestElementY : ISElement
    {
        public byte[] AsByteString
        {
            get { throw new NotImplementedException(); }
        }

        public string AsUTF8String
        {
            get { return "y"; }
        }
    }

    public class TestElementA : IElement
    {
    }

    public class TestElementB : IElement
    {
    }
}
