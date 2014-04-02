using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Practices.Unity;

namespace MyTorrent.BencodeDataParser
{
    sealed public class BencodeDataParser : IBencodeDataParser
    {
        /// <summary>
        /// Поток, содержащий данные в bencode-формате.
        /// </summary>
        private readonly BinaryReader stream;

        /// <summary>
        /// Ссылка на объект - агрегатор парсеров для всех 4 типов bencode-элементов.
        /// </summary>
        private readonly IAggregativeParser aggregativeParser;

        public BencodeDataParser(Stream stream)
            : 
            this
            (
                ServiceLocator.It.Resolve<IAggregativeParser>(), stream
            )
        {            
        }

        /// <summary>
        /// Выполняет парсинг находящихся в потоке bencode-элементов. Перед выполнением парсинга
        /// указатель текущей позиции потока устанавливается в начало.
        /// </summary>
        /// <returns>Корневые bencode-элементы, считанные из потока.</returns>
        public IEnumerable<IElement> Parse()
        {
            var rootElements = new List<IElement>();

            try
            {
                if (stream.BaseStream.Position != 0)
                {
                    stream.BaseStream.Position = 0;
                }

                while (stream.BaseStream.Position < stream.BaseStream.Length)
                {
                    rootElements.Add
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

            return rootElements;
        }

        internal BencodeDataParser(IAggregativeParser aggregativeParser, Stream stream)
        {
            this.aggregativeParser = aggregativeParser;

            if (stream == null)
            {
                throw new StreamNullException();
            }

            if (!stream.CanRead && !stream.CanWrite)
            {
                throw new StreamDisposedException();
            }

            if (!stream.CanRead)
            {
                throw new StreamCantReadException();
            }

            if (!stream.CanSeek)
            {
                throw new StreamCantSeekException();
            }

            this.stream = new BinaryReader(stream);
        }
    }
}
