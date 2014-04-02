using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTorrent.BencodeDataParser.Tests.LParserTestStuff; 

namespace MyTorrent.BencodeDataParser.Tests
{
    [TestClass]
    public class LParserTest : ParserTestBase
    <
        TestLElement, ILElement, IEnumerable<IElement>
    >
    {
        private readonly IElementParser<IElement> parser;

        private readonly IEnumerable<IEnumerable<IElement>> expectedDataSet;

        private readonly DataComparer<IEnumerable<IElement>> dataComparer;

        internal override IElementParser<IElement> Parser
        {
            get { return parser; }
        }

        internal override IEnumerable<IEnumerable<IElement>> ExpectedDataSet
        {
            get { return expectedDataSet; }
        }

        internal override DataComparer<IEnumerable<IElement>> DataComparer
        {
            get { return dataComparer; }
        }

        public LParserTest()
        {
            // Создаем тестируемый парсер
            var testLElementFactory    =  new TestLElementFactory();
            var testAggregativeParser  =  new TestAggregativeParser();

            this.parser = new LParser
            (
                testLElementFactory, testAggregativeParser
            );

            testAggregativeParser.LParser = this.parser;

            // Создаем массив с ожидаемыми результатами парсинга для различных
            // положительных наборов исходных данных
            this.expectedDataSet = new IEnumerable<IElement>[]
            {
                new IElement[] { },
                new IElement[]
                { 
                    new TestElementA(), new TestElementB()
                },
                new IElement[]
                { 
                    new TestLElement() 
                    {
                        Data = new IElement[]
                        { 
                            new TestElementA()
                        }
                    }
                }
            };

            // Создаем компаратор фактических и ожидаемых данных
            this.dataComparer = (expectedData, actualData) =>
            {
                if (actualData.Any( e => e == null ))
                {
                    return false;
                }

                if (expectedData.Count() != actualData.Count())
                {
                    return false;
                }

                return Enumerable.Range(0, expectedData.Count()).All
                (
                    index =>
                    {
                        var expectedElement  =  expectedData  .ElementAt(index);
                        var actualElement    =  actualData    .ElementAt(index);

                        if (expectedElement.GetType() != actualElement.GetType())
                        {
                            return false;
                        }

                        if (expectedElement.GetType() == typeof(TestLElement))
                        {
                            var expectedInnerData = 
                            (
                                (ITestElement<IEnumerable<IElement>>) expectedElement
                            )
                            .Data;

                            var actualInnerData =
                            (
                                (ITestElement<IEnumerable<IElement>>) actualElement
                            )
                            .Data;

                            return this.dataComparer
                            (
                                expectedInnerData, actualInnerData
                            );
                        }
                        else
                        {
                            return true;
                        }
                    }
                );
            };
        }

        /// <summary>
        /// Проверка парсинга корректного bencode-элемента
        /// </summary>
        [
             DataSource
             (
                 "Microsoft.VisualStudio.TestTools.DataSource.XML",
                 "|DataDirectory|\\LParser Test Data Set (Valid).xml",
                 "ValidElement", DataAccessMethod.Sequential
             ),

             DeploymentItem
             (
                 "BencodeDataParser.Tests\\3 LParser Tests\\LParser Test Data Set (Valid).xml"
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
                 "|DataDirectory|\\LParser Test Data Set (Incomplete).xml",
                 "IncompleteElement", DataAccessMethod.Sequential
             ),

             DeploymentItem
             (
                 "BencodeDataParser.Tests\\3 LParser Tests\\LParser Test Data Set (Incomplete).xml"
             ),

             TestMethod
        ]
        public override void IncompleteElementParsingTest()
        {
            base.IncompleteElementParsingTest();
        }

        /*
         * Элемент ListElement сам по себе не определяет данных или связей между ними, 
         * он только служит контейнером для других элементов, поэтому парсер ListParser
         * в принципе не генерирует исключения InvalidFormatException
         */

        /* Добавить тест на пропускание исключения InvalidMarkerException от IAggregativeParser */
    }
}
