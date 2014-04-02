using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTorrent.BencodeDataParser
{
    public class StreamCantSeekException : Exception
    {
        public StreamCantSeekException()
            : 
            base
            (
                "Поток, на который указывает ссылка \"stream\", " +
                "не поддерживает операцию перемещения внутреннего указателя."
            )
        {
        }
    }
}
