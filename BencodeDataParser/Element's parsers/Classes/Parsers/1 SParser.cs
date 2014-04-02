using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Practices.Unity;

namespace MyTorrent.BencodeDataParser
{
    internal class SParser : IElementParser<ISElement>
    {
        private readonly ISElementFactory stringFactory;

        internal SParser()
            : 
            this(ServiceLocator.It.Resolve<ISElementFactory>())
        {
        }

        internal SParser(ISElementFactory stringFactory)
        {
            this.stringFactory = stringFactory;
        }

        IEnumerable<char> IElementParser<ISElement>.GetMarkerSet()
        {
            return Enumerable.Range(0, 10).Select(digit => digit.ToString().First());
        }

        ISElement IElementParser<ISElement>.Parse(BinaryReader stream)
        {
            long currentElementOffset = stream.BaseStream.Position;

            StringBuilder lengthStringBuilder = new StringBuilder();

            try
            {
                while (true)
                {
                    char readedChar;

                    try
                    {
                        readedChar = stream.ReadChar();
                    }
                    catch (EndOfStreamException e)
                    {
                        throw new StreamEndException(e);
                    }
                    catch (ArgumentException)
                    {
                        throw new InvalidFormatException(currentElementOffset);
                    }

                    if (readedChar == ':')
                    {
                        break;
                    }

                    if (!char.IsDigit(readedChar))
                    {
                        throw new InvalidFormatException(currentElementOffset);
                    }

                    lengthStringBuilder.Append(readedChar);
                }

                string lengthString = lengthStringBuilder.ToString();

                if (lengthString == string.Empty)
                {
                    throw new InvalidFormatException(currentElementOffset);
                }

                int length;

                try
                {
                    length = int.Parse(lengthString);
                }
                catch (OverflowException)
                {
                    throw new InvalidFormatException(currentElementOffset);
                }

                if (length > 0)
                {
                    if (lengthString.First() == '0')
                    {
                        throw new InvalidFormatException(currentElementOffset);
                    }
                }

                byte[] byteString = stream.ReadBytes(length);

                if (byteString.Length < length)
                {
                    throw new StreamEndException();
                }

                return stringFactory.CreateElement(byteString);
            }
            catch (ObjectDisposedException e)
            {
                throw new StreamDisposedException(e);
            }
            catch (IOException e)
            {
                throw new StreamIOException(e);
            }
        }
    }
}
