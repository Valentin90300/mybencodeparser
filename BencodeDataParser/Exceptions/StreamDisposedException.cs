using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyTorrent.BencodeDataParser
{
    public class StreamDisposedException : Exception
    {
        public StreamDisposedException()
            :
            this(null)
        {
        }

        public StreamDisposedException(ObjectDisposedException innerException)
            :
            base
            (
                "Поток, на который указывает ссылка \"stream\", " +
                "был закрыт вызовом метода Dispose().", innerException
            )
        {
        }
    }
}
