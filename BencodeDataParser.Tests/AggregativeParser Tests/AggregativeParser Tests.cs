using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTorrent.BencodeDataParser.Tests.AggregativeParserTestStuff;

namespace MyTorrent.BencodeDataParser.Tests
{
    [TestClass]
    public class AggregativeParserTest
    {
        /// <summary>
        /// Поток, изпользуемый в каждом из тестов.
        /// </summary>
        private BinaryReader binaryReader;

        /// <summary>
        /// Тестируемый парсер.
        /// </summary>
        private readonly IAggregativeParser aggregativeParser = new AggregativeParser
        (
            //
            new TestSParser(),
            new TestNParser(),

            //
            new TestLParserFactory(),
            new TestDParserFactory()
        );

        [TestInitialize]
        public void PrepareStream()
        {
            this.binaryReader = new BinaryReader( new MemoryStream() );
        }

        [TestCleanup]
        public void DisposeStream()
        {
            this.binaryReader.Close();
        }

        [TestMethod]
        public void SParserUsingTest()
        {
            ElementParserUsingTest<ISElement>();
        }

        [TestMethod]
        public void NParserUsingTest()
        {
            ElementParserUsingTest<INElement>();
        }

        [TestMethod]
        public void LParserUsingTest()
        {
            ElementParserUsingTest<ILElement>();
        }

        [TestMethod]
        public void DParserUsingTest()
        {
            ElementParserUsingTest<IDElement>();
        }

        [TestMethod]
        public void InvalidMarkerHandlingTest()
        {
            int offset = (new Random()).Next
            (
                // Произвольная верхняя граница
                byte.MaxValue
            );

            try
            {
                binaryReader.BaseStream.Write
                (
                    Enumerable.Range(0, offset).Select(i => (byte) 0).ToArray(), 0, offset
                );

                binaryReader.BaseStream.WriteByte
                (
                    (byte) 'x'
                );

                binaryReader.BaseStream.Position--;
            }
            catch (Exception e)
            {
                throw new UnitTestException(e);
            }

            Exception catchedException = null;

            try
            {
                this.aggregativeParser.ParseWithAppropriateParser(binaryReader);
            }
            catch (Exception e)
            {
                catchedException = e;
            }

            Assert.IsNotNull
            (
                catchedException
            );
            Assert.IsInstanceOfType
            (
                catchedException, typeof(InvalidMarkerException)
            );

            long actualOffset = 
            (
                (InvalidMarkerException) catchedException
            )
            .Offset;

            Assert.AreEqual<long>((long) offset, actualOffset);
        }

        [
            TestMethod(), ExpectedException(typeof(StreamEndException))
        ]
        public void EmptyStreamHandlingTest()
        {
            this.aggregativeParser.ParseWithAppropriateParser(binaryReader);
        }

        private void ElementParserUsingTest<TIElement>()
        {
            byte markerByte = 0;

            if (typeof(TIElement) == typeof(ISElement))
            {
                markerByte = (byte) 's';
            }
            else if (typeof(TIElement) == typeof(INElement))
            {
                markerByte = (byte) 'i';
            }
            else if (typeof(TIElement) == typeof(ILElement))
            {
                markerByte = (byte) 'l';
            }
            else if (typeof(TIElement) == typeof(IDElement))
            {
                markerByte = (byte) 'd';
            }

            try
            {
                binaryReader.BaseStream.WriteByte
                (
                    markerByte
                );

                binaryReader.BaseStream.Position --;
            }
            catch (Exception e)
            {
                throw new UnitTestException(e);
            }

            IElement element = null;

            try
            {
                element = this.aggregativeParser.ParseWithAppropriateParser(binaryReader);
            }
            catch (Exception e)
            {
                throw new AssertFailedException
                (
                    "Метод AggregativeParser.ParseWithAppropriateParser сгенерировал исключение " +
                    "при корректных исходных данных в потоке.", e
                );
            }

            Assert.IsNotNull
            (
                element
            );
            Assert.IsInstanceOfType
            (
                element, typeof(TIElement)
            );
        }
    }
}
