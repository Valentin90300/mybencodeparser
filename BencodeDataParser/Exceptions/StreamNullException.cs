using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTorrent.BencodeDataParser
{
    public class StreamNullException : Exception
    {
        public StreamNullException()
            : 
            base("Параметр ссылочного типа \"stream\" имеет значение null.")
        {
        }
    }
}
