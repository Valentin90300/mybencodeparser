using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTorrent.BencodeDataParser
{
    sealed public class DElement : IDElement
    {
        internal class ISElementComparator : IEqualityComparer<ISElement>
        {
            bool IEqualityComparer<ISElement>.Equals(ISElement x, ISElement y)
            {
                try
                {
                    return x.AsUTF8String.Equals(y.AsUTF8String);
                }
                catch (CantConvertToUTF8StringException)
                {
                    throw new InvalidOperationException();
                }
            }

            int IEqualityComparer<ISElement>.GetHashCode(ISElement obj)
            {
                try
                {
                    return obj.AsUTF8String.GetHashCode();
                }
                catch (CantConvertToUTF8StringException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        private Dictionary<ISElement, IElement> dictionary;

        public int Count
        {
            get { return dictionary.Count; }
        }

        public IElement this[ISElement key]
        {
            get { return dictionary[key]; }
        }

        internal DElement(IEnumerable< KeyValuePair<ISElement, IElement> > pairs)
        {
            dictionary = pairs.ToDictionary
            (
                p => p.Key, p => p.Value, new ISElementComparator()
            );
        }

        public IEnumerator< KeyValuePair<ISElement, IElement> > GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        public bool ContainsKey(ISElement key)
        {
            return dictionary.ContainsKey(key);
        }
    }
}