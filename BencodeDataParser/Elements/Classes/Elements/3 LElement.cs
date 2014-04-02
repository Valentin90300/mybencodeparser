using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTorrent.BencodeDataParser
{
    sealed public class LElement : ILElement
    {
        private List<IElement> list;

        public int Count
        {
            get { return list.Count; }
        }

        internal LElement(IEnumerable<IElement> elements)
        {
            list = elements.ToList();
        }

        public IEnumerator<IElement> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public IEnumerable<IElement> GetAllElements()
        {
            return list.Select(e => e);
        }
    }
}
