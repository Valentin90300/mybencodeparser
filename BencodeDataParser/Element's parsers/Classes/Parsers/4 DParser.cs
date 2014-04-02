using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Practices.Unity;

namespace MyTorrent.BencodeDataParser
{
    internal class DParser : IElementParser<IDElement>
    {
        private readonly IDElementFactory dElementFactory;
        private readonly IAggregativeParser aggregativeParser;

        internal DParser(IAggregativeParser aggregativeParser)
            : 
            this( ServiceLocator.It.Resolve<IDElementFactory>(), aggregativeParser )
        {
        }

        internal DParser(IDElementFactory dElementFactory, IAggregativeParser aggregativeParser)
        {
            this.dElementFactory = dElementFactory;
            this.aggregativeParser = aggregativeParser;
        }

        IEnumerable<char> IElementParser<IDElement>.GetMarkerSet()
        {
            return new char[] { 'd' };
        }

        IDElement IElementParser<IDElement>.Parse(BinaryReader stream)
        {
            long currentElementOffset = -1;
            
            var pairs = new List
            <
                KeyValuePair<ISElement, IElement>
            >();

            #region getChar delegate
            Func<char> getChar = () =>
            {
                int peekedValue;

                try
                {
                    peekedValue = stream.PeekChar();
                }
                catch (ArgumentException)
                {
                    throw new InvalidFormatException(currentElementOffset);
                }

                if (peekedValue == -1)
                {
                    throw new StreamEndException();
                }

                char peekedChar = (char) peekedValue;

                return peekedChar;
            };
            #endregion

            try
            {
                // Запоминаем смещение текущего элемента и пропускаем маркер 'd' 
                // (перемещаемся на 1 байт вперед)
                currentElementOffset = stream.BaseStream.Position ++;

                while (true)
                {
                    char keyMarker = getChar();

                    if (keyMarker == 'e')
                    {
                        stream.BaseStream.Position ++;

                        break;
                    }

                    IElement keyIElement = aggregativeParser.ParseWithAppropriateParser(stream);

                    ISElement keyISElement = keyIElement as ISElement;

                    if (keyISElement == null)
                    {
                        throw new InvalidFormatException(currentElementOffset);
                    }

                    string keyString;

                    try
                    {
                        keyString = keyISElement.AsUTF8String;
                    }
                    catch (CantConvertToUTF8StringException)
                    {
                        throw new InvalidFormatException(currentElementOffset);
                    }

                    if
                    (
                        pairs.Any
                        (
                            p => string.CompareOrdinal(keyString, p.Key.AsUTF8String) == 0
                        )
                    )
                    {
                        throw new InvalidFormatException(currentElementOffset);
                    }

                    if (pairs.Count() > 0)
                    {
                        if (string.CompareOrdinal(keyString, pairs.Last().Key.AsUTF8String) < 0)
                        {
                            throw new InvalidFormatException(currentElementOffset);
                        }
                    }

                    char valueMarker = getChar();

                    if (valueMarker == 'e')
                    {
                        throw new InvalidFormatException(currentElementOffset);
                    }

                    IElement valueIElement = aggregativeParser.ParseWithAppropriateParser(stream);

                    pairs.Add
                    (
                        new KeyValuePair<ISElement, IElement>(keyISElement, valueIElement)
                    );
                }
            }
            catch (ObjectDisposedException e)
            {
                throw new StreamDisposedException(e);
            }
            catch (IOException e)
            {
                throw new StreamIOException(e);
            }

            return dElementFactory.CreateElement(pairs);
        }
    }
}
