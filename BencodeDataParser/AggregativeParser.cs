using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Practices.Unity;

namespace MyTorrent.BencodeDataParser
{
    #region Aliases
    using ILParserFactory = IContainerParserFactory<IElementParser<ILElement>>;
    using IDParserFactory = IContainerParserFactory<IElementParser<IDElement>>;
    #endregion

    internal class AggregativeParser : IAggregativeParser
    {
        private readonly IEnumerable<IElementParser<IElement>> parserSet;

        internal AggregativeParser() 
            : 
            this
            (
                //
                ServiceLocator.It.Resolve< IElementParser<ISElement> >(),
                ServiceLocator.It.Resolve< IElementParser<INElement> >(),
                
                //
                ServiceLocator.It.Resolve< ILParserFactory >(),
                ServiceLocator.It.Resolve< IDParserFactory >()
            )
        {
        }

        internal AggregativeParser
            (
                //
                IElementParser<ISElement> sParser,
                IElementParser<INElement> nParser,
                
                //
                ILParserFactory lParserFactory,
                IDParserFactory dParserFactory
            )
        {
            this.parserSet = new IElementParser<IElement>[]
            {
                sParser, 
                nParser, 
                lParserFactory.CreateParser(this), 
                dParserFactory.CreateParser(this)
            };
        }

        IElement IAggregativeParser.ParseWithAppropriateParser(BinaryReader stream)
        {
            long currentOffset = -1;

            int peekedValue = -1;

            try
            {
                currentOffset = stream.BaseStream.Position;

                try
                {
                    peekedValue = stream.PeekChar();
                }
                catch (ArgumentException)
                {
                    throw new InvalidMarkerException(currentOffset);
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

            if (peekedValue == -1)
            {
                throw new StreamEndException();
            }

            char marker = (char) peekedValue;

            IElementParser<IElement> appropriateParser = parserSet.SingleOrDefault
            (
                p => p.GetMarkerSet().Contains(marker)
            );

            if (appropriateParser == null)
            {
                throw new InvalidMarkerException(currentOffset);
            }

            return appropriateParser.Parse(stream);
        }
    }
}
