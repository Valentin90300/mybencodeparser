using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTorrent.BencodeDataParser.Tests
{
    public interface ITestElement<TData>
    {
        TData Data { get; set; }
    }
}
