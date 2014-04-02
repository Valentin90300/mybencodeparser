using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Practices.Unity;

namespace MyTorrent.BencodeDataParser
{
    internal class NParser : IElementParser<INElement>
    {
        private readonly INElementFactory nElementFactory;

        internal NParser()
            :
            this(ServiceLocator.It.Resolve<INElementFactory>())
        {
        }

        internal NParser(INElementFactory nElementFactory)
        {
            this.nElementFactory = nElementFactory;
        }

        IEnumerable<char> IElementParser<INElement>.GetMarkerSet()
        {
           return new char[] { 'i' };
        }

        INElement IElementParser<INElement>.Parse(BinaryReader stream)
        {
            long currentElementOffset;

            StringBuilder numberStringBuilder = new StringBuilder();

            try
            {
                // Запоминаем смещение текущего элемента и пропускаем маркер 'i' 
                // (перемещаемся на 1 байт вперед)
                currentElementOffset = stream.BaseStream.Position ++;

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

                    if (readedChar == 'e')
                    {
                        break;
                    }

                    if (numberStringBuilder.Length == 0)
                    {
                        if (!char.IsDigit(readedChar) && readedChar != '-')
                        {
                            throw new InvalidFormatException(currentElementOffset);
                        }
                    }
                    else
                    {
                        if (!char.IsDigit(readedChar))
                        {
                            throw new InvalidFormatException(currentElementOffset);
                        }
                    }

                    numberStringBuilder.Append(readedChar);
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

            string numberString = numberStringBuilder.ToString();

            if (numberString == string.Empty)
            {
                throw new InvalidFormatException(currentElementOffset);
            }

            long number;

            try
            {
                number = long.Parse(numberString);
            }
            catch (OverflowException)
            {
                throw new InvalidFormatException(currentElementOffset);
            }

            if (number == 0)
            {
                // Отлавливаем ошибки типа:
                // 1)  0..0
                // 2)    -0
                // 3) -0..0

                if (numberString.Length > 1)
                {
                    throw new InvalidFormatException(currentElementOffset);
                }
            }
            else
            {
                // Отлавливаем ошибки типа:
                // 1)  010
                // 2) -010

                char firstDigit;

                if (number >= 0)
                {
                    firstDigit = numberString[0];
                }
                else
                {
                    firstDigit = numberString[1];
                }

                if (firstDigit == '0')
                {
                    throw new InvalidFormatException(currentElementOffset);
                }
            }

            return nElementFactory.CreateElement(number);
        }
    }
}
