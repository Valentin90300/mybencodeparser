using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTorrent.BencodeDataParser
{
    sealed public class SElement : ISElement
    {
        private byte[] byteString;

        public byte[] AsByteString
        {
            get
            {
                return byteString.Select(b => b).ToArray();
            }
        }

        public string AsUTF8String
        {
            get
            {
                string utf8String;

                try
                {
                    utf8String = Encoding.UTF8.GetString(byteString);
                }
                catch (ArgumentException)
                {
                    throw new CantConvertToUTF8StringException();
                }

                return utf8String;
            }
        }

        internal SElement(IEnumerable<byte> byteString)
        {
            this.byteString = byteString.ToArray();
        }
    }
}
