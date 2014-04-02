using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTorrent.BencodeDataParser.Tests.NParserTestStuff;

namespace MyTorrent.BencodeDataParser.Tests
{
    [TestClass]
    public class NParserTest : ParserTestBase
    <
        TestNElement, INElement, long
    >
    {
        private readonly IElementParser<IElement> parser;

        private readonly IEnumerable<long> expectedDataSet;

        private readonly DataComparer<long> dataComparer;

        internal override IElementParser<IElement> Parser
        {
            get { return parser; }
        }

        internal override IEnumerable<long> ExpectedDataSet
        {
            get { return expectedDataSet; }
        }

        internal override DataComparer<long> DataComparer
        {
            get { return dataComparer; }
        }

        public NParserTest()
        {
            // Создаем тестируемый парсер
            this.parser = new NParser( new TestNElementFactory() );

            // Создаем массив с ожидаемыми результатами парсинга для различных
            // положительных наборов исходных данных
            this.expectedDataSet = new long[]
            {
                0,
                10,
                -20,
                10000000000
            };

            // Создаем компаратор фактических и ожидаемых данных
            this.dataComparer = (expected, actual) => expected.Equals(actual);
        }

       /// <summary>
       /// Проверка парсинга корректного bencode-элемента
       /// </summary>
       [
            DataSource
            (
                "Microsoft.VisualStudio.TestTools.DataSource.XML",
                "|DataDirectory|\\NParser Test Data Set (Valid).xml",
                "ValidElement", DataAccessMethod.Sequential
            ),
            
            DeploymentItem
            (
                "BencodeDataParser.Tests\\2 NParser Tests\\NParser Test Data Set (Valid).xml"
            ),
            
            TestMethod
       ]
       public override void ValidElementParsingTest()
       {
           base.ValidElementParsingTest();
       }

       /// <summary>
       /// Проверка генерации исключения StreamEndException при попытке парсинга
       /// неполностью записанного bencode-элемента
       /// </summary>
       [
            DataSource
            (
                "Microsoft.VisualStudio.TestTools.DataSource.XML",
                "|DataDirectory|\\NParser Test Data Set (Incomplete).xml",
                "IncompleteElement", DataAccessMethod.Sequential
            ),

            DeploymentItem
            (
                "BencodeDataParser.Tests\\2 NParser Tests\\NParser Test Data Set (Incomplete).xml"
            ),

            TestMethod
       ]
       public override void IncompleteElementParsingTest()
       {
           base.IncompleteElementParsingTest();
       }

       /// <summary>
       /// Проверка генерации исключения InvalidFormatException при попытке парсинга
       /// некорректного bencode-элемента
       /// </summary>
       [
            DataSource
            (
                "Microsoft.VisualStudio.TestTools.DataSource.XML",
                "|DataDirectory|\\NParser Test Data Set (Invalid).xml",
                "InvalidElement", DataAccessMethod.Sequential
            ),

            DeploymentItem
            (
                "BencodeDataParser.Tests\\2 NParser Tests\\NParser Test Data Set (Invalid).xml"
            ),

            TestMethod
       ]
       public override void InvalidElementParsingTest()
       {
           base.InvalidElementParsingTest();
       }
    }
}
