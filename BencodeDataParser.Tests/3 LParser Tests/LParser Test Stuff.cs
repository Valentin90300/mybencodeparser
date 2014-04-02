using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyTorrent.BencodeDataParser.Tests.LParserTestStuff
{
    public class TestLElement : ILElement, ITestElement<IEnumerable<IElement>>
    {
        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerator<IElement> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IElement> GetAllElements()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IElement> Data
        {
            get; set;
        }
    }

    internal class TestLElementFactory : ILElementFactory
    {
        ILElement ILElementFactory.CreateElement(IEnumerable<IElement> value)
        {
            return new TestLElement() { Data = value };
        }
    }

    internal class TestAggregativeParser : IAggregativeParser
    {
        /// <summary>
        /// Данное свойство позволяет методу Parse парсера LParser делать рекурсивный вызов
        /// (через IAggregativeParser.ParseWithAppropriateParser).
        /// </summary>
        internal IElementParser<IElement> LParser { get; set; }

        IElement IAggregativeParser.ParseWithAppropriateParser(BinaryReader stream)
        {
            int peekedValue = -1;

            try
            {
                peekedValue = stream.PeekChar();
            }
            catch (Exception e)
            {
                throw new UnitTestException(e);
            }

            if (peekedValue == -1)
            {
                throw new UnitTestException();
            }

            char marker = (char) peekedValue;

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
                case 'a':
                    return IncrementAndReturn( new TestElementA() );
                case 'b':
                    return IncrementAndReturn( new TestElementB() );
                case 'l':
                    return LParser.Parse(stream);
                case 'x':
                    throw new InvalidMarkerException(1);
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