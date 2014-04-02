using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTorrent.BencodeDataParser.Tests.SParserTestStuff;

namespace MyTorrent.BencodeDataParser.Tests
{
    [TestClass]
    public class SParserTest : ParserTestBase
    <
        TestSElement, ISElement, IEnumerable<byte>
    >
    {
        private readonly IElementParser<IElement> parser;

        private readonly IEnumerable<IEnumerable<byte>> expectedDataSet;

        private readonly DataComparer<IEnumerable<byte>> dataComparer;

        internal override IElementParser<IElement> Parser
        {
            get { return parser; }
        }

        internal override IEnumerable<IEnumerable<byte>> ExpectedDataSet
        {
            get { return expectedDataSet; }
        }

        internal override DataComparer<IEnumerable<byte>> DataComparer
        {
            get { return dataComparer; }
        }

        public SParserTest()
        {
            // Создаем тестируемый парсер
            this.parser = new SParser( new TestSElementFactory() );

            // Создаем массив с ожидаемыми результатами парсинга для различных
            // положительных наборов исходных данных
            this.expectedDataSet = new[]
            {
                "", "string", "строка"
            }
            .Select
            (
                s => Encoding.UTF8.GetBytes(s)
            )
            .ToArray();


            // Создаем компаратор фактических и ожидаемых данных
            this.dataComparer = (expected, actual) => expected.SequenceEqual(actual);
        }

        /// <summary>
        /// Проверка парсинга корректного bencode-элемента
        /// </summary>
        [
            DataSource
            (
                "Microsoft.VisualStudio.TestTools.DataSource.XML", 
                "|DataDirectory|\\SParser Test Data Set (Valid).xml", 
                "ValidElement", DataAccessMethod.Sequential
            ),
            
            DeploymentItem
            (
                "BencodeDataParser.Tests\\1 SParser Tests\\SParser Test Data Set (Valid).xml"
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
                "|DataDirectory|\\SParser Test Data Set (Incomplete).xml", 
                "IncompleteElement", DataAccessMethod.Sequential
            ), 
        
            DeploymentItem
            (
                "BencodeDataParser.Tests\\1 SParser Tests\\SParser Test Data Set (Incomplete).xml"
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
                "|DataDirectory|\\SParser Test Data Set (Invalid).xml", 
                "InvalidElement", DataAccessMethod.Sequential
            ), 
        
            DeploymentItem
            (
                "BencodeDataParser.Tests\\1 SParser Tests\\SParser Test Data Set (Invalid).xml"
            ), 
        
            TestMethod
        ]
        public override void InvalidElementParsingTest()
        {
            base.InvalidElementParsingTest();
        }
    }
}
