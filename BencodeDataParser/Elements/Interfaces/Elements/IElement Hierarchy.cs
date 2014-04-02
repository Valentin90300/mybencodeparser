using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTorrent.BencodeDataParser
{
    /// <summary>
    /// Интерфейс, от которого должны наследоваться интерфейсы конкретных bencode-элементов.
    /// Данный интерфейс не определяет никаких членов, он предназначен только для образования
    /// иерархии интерфейсов.
    /// </summary>
    public interface IElement
    {
    }

    public interface ISElement : IElement
    {
        Byte[] AsByteString { get; }

        string AsUTF8String { get; }
    }

    public interface INElement : IElement
    {
        long Value { get; }
    }

    public interface ILElement : IElement
    {
        int Count { get; }

        IEnumerator<IElement> GetEnumerator();

        IEnumerable<IElement> GetAllElements();
    }

    public interface IDElement : IElement
    {
        int Count { get; }

        IElement this[ISElement key] { get; }

        IEnumerator< KeyValuePair<ISElement, IElement> > GetEnumerator();

        bool ContainsKey(ISElement key);
    }
}
