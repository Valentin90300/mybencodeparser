using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTorrent.BencodeDataParser.Tests
{
    public class UnitTestException : Exception
    {
        public UnitTestException()
            :
            this(null)
        {
        }

        public UnitTestException(Exception innerException)
            : 
            base("Во время выполнения юнит-теста возникла внутренняя ошибка.", innerException)
        {
        }
    }
}
