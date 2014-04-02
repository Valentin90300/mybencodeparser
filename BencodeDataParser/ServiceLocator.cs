using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace MyTorrent.BencodeDataParser
{
    #region Aliases
    using ILParserFactory = IContainerParserFactory<IElementParser<ILElement>>;
    using IDParserFactory = IContainerParserFactory<IElementParser<IDElement>>;
    #endregion

    internal static class ServiceLocator
    {
        internal static IUnityContainer It { get; private set; }

        static ServiceLocator()
        {
            It = new UnityContainer();

            /* Зарегистрировать классы фабрик bencode-элементов */

            It.RegisterType< ISElementFactory, SElementFactory >();
            It.RegisterType< INElementFactory, NElementFactory >();
            It.RegisterType< ILElementFactory, LElementFactory >();
            It.RegisterType< IDElementFactory, DElementFactory >();

            /* Зарегистрировать классы парсеров элементов */

            It.RegisterType<IElementParser<ISElement>, SParser >();
            It.RegisterType<IElementParser<INElement>, NParser >();

            /* Зарегистрировать классы фабрик парсеров составных bencode-элементов */

            It.RegisterType< ILParserFactory, LParserFactory >();
            It.RegisterType< IDParserFactory, DParserFactory >();

            /* Зарегистрировать реализацию IAggregativeParser */

            It.RegisterType< IAggregativeParser, AggregativeParser >();
        }
    }
}
