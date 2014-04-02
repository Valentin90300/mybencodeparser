using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTorrent.BencodeDataParser.Tests.DParserTestStuff;

namespace MyTorrent.BencodeDataParser.Tests
{
    [TestClass]
    public class DParserTest : ParserTestBase
    <
        TestDElement, IDElement, IEnumerable<KeyValuePair<ISElement, IElement>>
    >
    {
        private readonly IElementParser<IElement> parser;

        private readonly IEnumerable
            <
                IEnumerable<KeyValuePair<ISElement, IElement>> 
            > 
            expectedDataSet;

        private readonly DataComparer
            <
                IEnumerable<KeyValuePair<ISElement, IElement>> 
            > 
            dataComparer;

        internal override IElementParser<IElement> Parser
        {
            get { return parser; }
        }

        internal override IEnumerable
            <
                IEnumerable<KeyValuePair<ISElement, IElement>>
            > 
            ExpectedDataSet
        {
            get { return expectedDataSet; }
        }

        internal override DataComparer
            <
                IEnumerable<KeyValuePair<ISElement, IElement>>
            >
            DataComparer
        {
            get { return dataComparer; }
        }

        public DParserTest()
        {
            // Создаем тестируемый парсер
            var testDElementFactory    =  new TestDElementFactory();
            var testAggregativeParser  =  new TestAggregativeParser();

            this.parser = new DParser
            (
                testDElementFactory, testAggregativeParser
            );

            testAggregativeParser.DParser = this.parser;

            // Создаем массив с ожидаемыми результатами парсинга для различных
            // положительных наборов исходных данных
            this.expectedDataSet = new IEnumerable<KeyValuePair<ISElement, IElement>>[]
            {
                new KeyValuePair<ISElement, IElement>[] {},
                new KeyValuePair<ISElement, IElement>[]
                {
                    new KeyValuePair<ISElement, IElement>
                    (
                        new TestElementX(), new TestElementA()
                    ),
                    new KeyValuePair<ISElement, IElement>
                    (
                        new TestElementY(), new TestElementB()
                    )
                },
                new KeyValuePair<ISElement, IElement>[]
                {
                    new KeyValuePair<ISElement, IElement>
                    (
                        new TestElementX(), new TestDElement()
                        {
                            Data = new KeyValuePair<ISElement, IElement>[]
                            {
                                new KeyValuePair<ISElement, IElement>
                                (
                                    new TestElementY(), new TestElementB()
                                )
                            }
                        }
                    )
                }
            };

            // Создаем компаратор фактических и ожидаемых данных
            this.dataComparer = (expectedData, actualData) =>
            {
                if (actualData.Any( p => p.Key == null || p.Value == null) )
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
                        var expectedPair  =  expectedData  .ElementAt(index);
                        var actualPair    =  actualData    .ElementAt(index);

                        if (
                            expectedPair .Key   .GetType() != actualPair .Key   .GetType()
                            ||
                            expectedPair .Value .GetType() != actualPair .Value .GetType()
                            )
                        {
                            return false;
                        }

                        if (expectedPair.Value.GetType() == typeof(TestDElement))
                        {
                            var expectedInnerDictionary =
                            (
                                ITestElement
                                <
                                    IEnumerable<KeyValuePair<ISElement, IElement>>
                                >
                            )
                            expectedPair.Value;

                            var actualInnerDictionary =
                            (
                                ITestElement
                                <
                                    IEnumerable<KeyValuePair<ISElement, IElement>>
                                >
                            )
                            actualPair.Value;

                            return this.dataComparer
                            (
                                expectedInnerDictionary.Data, actualInnerDictionary.Data
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
                 "|DataDirectory|\\DParser Test Data Set (Valid).xml",
                 "ValidElement", DataAccessMethod.Sequential
             ),

             DeploymentItem
             (
                 "BencodeDataParser.Tests\\4 DParser Tests\\DParser Test Data Set (Valid).xml"
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
                 "|DataDirectory|\\DParser Test Data Set (Incomplete).xml",
                 "IncompleteElement", DataAccessMethod.Sequential
             ),

             DeploymentItem
             (
                 "BencodeDataParser.Tests\\4 DParser Tests\\DParser Test Data Set (Incomplete).xml"
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
                 "|DataDirectory|\\DParser Test Data Set (Invalid).xml",
                 "InvalidElement", DataAccessMethod.Sequential
             ),

             DeploymentItem
             (
                 "BencodeDataParser.Tests\\4 DParser Tests\\DParser Test Data Set (Invalid).xml"
             ),

             TestMethod
        ]
        public override void InvalidElementParsingTest()
        {
            base.InvalidElementParsingTest();
        }
    }
}
