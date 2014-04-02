using System;
using System.Collections.Generic;

namespace MyTorrent.BencodeDataParser
{
    internal interface ISElementFactory
    {
        ISElement CreateElement(IEnumerable<byte> value);
    }

    internal interface INElementFactory
    {
        INElement CreateElement(long value);
    }

    internal interface ILElementFactory
    {
        ILElement CreateElement(IEnumerable<IElement> value);
    }

    internal interface IDElementFactory
    {
        IDElement CreateElement(IEnumerable<KeyValuePair<ISElement, IElement>> value);
    }
}