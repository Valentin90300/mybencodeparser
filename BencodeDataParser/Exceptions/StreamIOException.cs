using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyTorrent.BencodeDataParser
{
    public class StreamIOException : Exception
    {
        public StreamIOException(IOException innerException)
            :
            base
            (
                "В процессе работы с потоком, адресуемым ссылкой \"stream\", " + 
                "возникла ошибка ввода вывода.", innerException
            )
        {
        }
    }
}
