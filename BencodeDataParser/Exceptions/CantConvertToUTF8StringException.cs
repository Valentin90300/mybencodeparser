using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTorrent.BencodeDataParser
{
    public class CantConvertToUTF8StringException : Exception
    {
        public CantConvertToUTF8StringException()
            : 
            base
            (
                "Невозможно интерпретировать заданную последовательность байтов " +
                "как строку в кодировке UTF-8."
            )
        {
        }
    }
}
