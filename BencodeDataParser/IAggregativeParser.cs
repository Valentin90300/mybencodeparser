using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyTorrent.BencodeDataParser
{
    internal interface IAggregativeParser
    {
        IElement ParseWithAppropriateParser(BinaryReader stream);
    }
}
