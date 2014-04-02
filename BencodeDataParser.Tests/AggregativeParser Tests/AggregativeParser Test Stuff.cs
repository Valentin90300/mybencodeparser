using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyTorrent.BencodeDataParser.Tests.AggregativeParserTestStuff
{
    #region Тестовые реализации IElement

    public class TestSElement : ISElement
    {
        public byte[] AsByteString
        {
	        get { throw new NotImplementedException(); }
        }

        public string AsUTF8String
        {
	        get { throw new NotImplementedException(); }
        }
    }

    public class TestNElement : INElement
    {
        public long Value
        {
	        get { throw new NotImplementedException(); }
        }
    }

    public class TestLElement : ILElement
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
    }

    public class TestDElement : IDElement
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
    }

    #endregion

    #region Тестовые реализации IElementParser<TIElement>

    internal class TestSParser : IElementParser<ISElement>
    {
        IEnumerable<char> IElementParser<ISElement>.GetMarkerSet()
        {
            return new char[] { 's' };
        }

        ISElement IElementParser<ISElement>.Parse(BinaryReader stream)
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

            if (marker != 's')
            {
                throw new UnitTestException();
            }

            return new TestSElement();
        }
    }

    internal class TestNParser : IElementParser<INElement>
    {
        IEnumerable<char> IElementParser<INElement>.GetMarkerSet()
        {
            return new char[] { 'i' };
        }

        INElement IElementParser<INElement>.Parse(BinaryReader stream)
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

            if (marker != 'i')
            {
                throw new UnitTestException();
            }

            return new TestNElement();
        }
    }

    internal class TestLParser : IElementParser<ILElement>
    {
        IEnumerable<char> IElementParser<ILElement>.GetMarkerSet()
        {
            return new char[] { 'l' };
        }

        ILElement IElementParser<ILElement>.Parse(BinaryReader stream)
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

            if (marker != 'l')
            {
                throw new UnitTestException();
            }

            return new TestLElement();
        }
    }

    internal class TestDParser : IElementParser<IDElement>
    {
        IEnumerable<char> IElementParser<IDElement>.GetMarkerSet()
        {
            return new char[] { 'd' };
        }

        IDElement IElementParser<IDElement>.Parse(BinaryReader stream)
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

            if (marker != 'd')
            {
                throw new UnitTestException();
            }

            return new TestDElement();
        }
    }

    #endregion

    #region Тестовые реализации фабрик экземпляров классов-парсеров элементов-контейнеров

    internal class TestLParserFactory : IContainerParserFactory<IElementParser<ILElement>>
    {
        IElementParser<ILElement> IContainerParserFactory
            <
                IElementParser<ILElement>
            >
            .CreateParser(IAggregativeParser aggregativeParser)
        {
            return new TestLParser();
        }
    }

    internal class TestDParserFactory : IContainerParserFactory<IElementParser<IDElement>>
    {
        IElementParser<IDElement> IContainerParserFactory
            <
                IElementParser<IDElement>
            >
            .CreateParser(IAggregativeParser aggregativeParser)
        {
            return new TestDParser();
        }
    }

    #endregion
}
