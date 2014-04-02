using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyTorrent.BencodeDataParser
{
    public class StreamEndException : Exception
    {
        public StreamEndException()
            :
            this(null)
        {
        }

        public StreamEndException(EndOfStreamException innerException)
            :
            base
            (
                "В потоке \"stream\" обнаружены неполностью записанные bencode-данные.", innerException
            )
        {
        }
    }
}
