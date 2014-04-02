using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTorrent.BencodeDataParser
{
    sealed public class NElement : INElement
    {
        private long value;

        public long Value
        {
            get { return value; }
        }

        internal NElement(long value)
        {
            this.value = value;
        }
    }
}
