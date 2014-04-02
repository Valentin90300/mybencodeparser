using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTorrent.BencodeDataParser
{
    internal class SElementFactory : ISElementFactory
    {
        ISElement ISElementFactory.CreateElement(IEnumerable<byte> value)
        {
            return new SElement(value);
        }
    }

    internal class NElementFactory : INElementFactory
    {
        INElement INElementFactory.CreateElement(long value)
        {
            return new NElement(value);
        }
    }

    internal class LElementFactory : ILElementFactory
    {
        ILElement ILElementFactory.CreateElement(IEnumerable<IElement> value)
        {
            return new LElement(value);
        }
    }

    internal class DElementFactory : IDElementFactory
    {
        IDElement IDElementFactory.CreateElement
            (
                IEnumerable< KeyValuePair<ISElement, IElement> > value
            )
        {
            return new DElement(value);
        }
    }
}
