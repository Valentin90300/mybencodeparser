using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Practices.Unity;

namespace MyTorrent.BencodeDataParser
{
    internal class LParser : IElementParser<ILElement>
    {
        private readonly ILElementFactory lElementFactory;
        private readonly IAggregativeParser aggregativeParser;

        internal LParser(IAggregativeParser aggregativeParser)
            :
            this( ServiceLocator.It.Resolve<ILElementFactory>(), aggregativeParser )
        {
        }

        internal LParser(ILElementFactory lElementFactory, IAggregativeParser aggregativeParser)
        {
            this.lElementFactory = lElementFactory;
            this.aggregativeParser = aggregativeParser;
        }

        IEnumerable<char> IElementParser<ILElement>.GetMarkerSet()
        {
            return new char[] { 'l' };
        }

        ILElement IElementParser<ILElement>.Parse(BinaryReader stream)
        {
            List<IElement> innerElements = new List<IElement>();

            long currentElementOffset;

            try
            {
                // Запоминаем смещение текущего элемента и пропускаем маркер 'l' 
                // (перемещаемся на 1 байт вперед)
                currentElementOffset = stream.BaseStream.Position ++;

                while (true)
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

                    if (peekedChar == 'e')
                    {
                        stream.BaseStream.Position++;

                        break;
                    }

                    innerElements.Add
                    (
                        aggregativeParser.ParseWithAppropriateParser(stream)
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

            return lElementFactory.CreateElement(innerElements);
        }
    }
}
