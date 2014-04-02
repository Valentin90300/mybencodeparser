using System;
using System.Collections.Generic;

namespace MyTorrent.BencodeDataParser
{
    internal interface IContainerParserFactory
        <
            TIElementParser
        >
        where TIElementParser : IElementParser<IElement>
    {
        TIElementParser CreateParser(IAggregativeParser aggregativeParser);
    }
}