using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyTorrent.BencodeDataParser
{
    internal interface IElementParser<out TIElement> where TIElement : IElement
    {
        /// <summary>
        /// При переопределении в производном классе возвращает символы, являющиеся маркерами
        /// для данного типа парсера.
        /// </summary>
        /// <returns></returns>
        IEnumerable<char> GetMarkerSet();

        /// <summary>
        /// При переопределении в производном классе выполняет парсинг находящегося в потоке
        /// bencode-элемента и возвращает объект, инкапсулирующий извлеченные данные.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        TIElement Parse(BinaryReader stream);
    }
}
