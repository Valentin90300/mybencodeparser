using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTorrent.BencodeDataParser
{
    public class InvalidMarkerException : Exception, IOffsetProvider
    {
        private readonly long offset;

        public long Offset
        {
            get { return offset; }
        }

        public InvalidMarkerException(long offset)
            :
            base("В потоке \"stream\" обнаружен недопустимый маркер bencode-элемента.")
        {
            this.offset = offset;
        }
    }
}
