using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTorrent.BencodeDataParser
{
    public class StreamCantReadException : Exception
    {
        public StreamCantReadException()
            : 
            base
            (
                "Поток, на который указывает ссылка \"stream\", не поддерживает операцию чтения."
            )
        {
        }
    }
}
