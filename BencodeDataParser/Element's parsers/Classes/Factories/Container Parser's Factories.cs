using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTorrent.BencodeDataParser
{
    #region Aliases
    using ILParserFactory = IContainerParserFactory< IElementParser<ILElement> >;
    using IDParserFactory = IContainerParserFactory< IElementParser<IDElement> >;
    #endregion

    internal class LParserFactory : ILParserFactory
    {
        IElementParser<ILElement> ILParserFactory.CreateParser(IAggregativeParser aggregativeParser)
        {
            return new LParser(aggregativeParser);
        }
    }

    internal class DParserFactory : IDParserFactory
    {
        IElementParser<IDElement> IDParserFactory.CreateParser(IAggregativeParser aggregativeParser)
        {
            return new DParser(aggregativeParser);
        }
    }
}
