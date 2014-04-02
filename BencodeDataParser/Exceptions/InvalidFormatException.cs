using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTorrent.BencodeDataParser
{
    public class InvalidFormatException : Exception, IOffsetProvider
    {
        private readonly long offset;

        public long Offset
        {
            get { return offset; }
        }

        public InvalidFormatException(long offset)
            :
            base("В потоке \"stream\" обнаружен bencode-элемент с ошибочным форматом.")
        {
            this.offset = offset;
        }
    }
}
