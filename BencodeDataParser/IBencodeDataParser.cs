﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTorrent.BencodeDataParser
{
    public interface IBencodeDataParser
    {
        IEnumerable<IElement> Parse();
    }
}
