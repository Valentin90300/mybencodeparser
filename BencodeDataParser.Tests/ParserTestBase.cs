using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyTorrent.BencodeDataParser.Tests
{
    /// <summary>
    /// Делегат, используемый для сравнения данных элемента, возвращенного методом Parse()
    /// тестируемого парсера с ожидаемыми данными для текущего тестового набора.
    /// </summary>
    internal delegate bool DataComparer<TElementData>
    (
        TElementData expected, TElementData actual
    );

    /// <summary>
    /// Абстрактный базовый класс, содержащий логику для тестирования классов-парсеров, 
    /// реализующих IElementParser.
    /// </summary>
    /// <typeparam name="TElement">
    ///     Класс элемента, возвращаемого парсером.
    /// </typeparam>
    /// <typeparam name="TIElement">
    ///     Интерфейс элемента, реализуемого классом элемента.
    /// </typeparam> 
    /// <typeparam name="TElementData">
    ///     Тип данных, передаваемых конструктору класса элемента.
    /// </typeparam>
    public abstract class ParserTestBase
    <
        TElement, TIElement, TElementData
    >
        where TElement : TIElement, ITestElement<TElementData> where TIElement : IElement 
    {
        public TestContext TestContext
        { get; set; }

        internal abstract IElementParser<IElement> Parser
        { get; }

        internal abstract IEnumerable<TElementData> ExpectedDataSet
        { get; }

        internal abstract DataComparer<TElementData> DataComparer
        { get; }

        private readonly Random randomizer = new Random();

        public virtual void ValidElementParsingTest()
        {
            using (var binaryReader = new BinaryReader( new MemoryStream()) )
            {
                WriteBencodeDataToStream(binaryReader.BaseStream);

                var element = Parser.Parse(binaryReader);

                Assert.IsNotNull
                (
                    element
                );

                Assert.IsInstanceOfType
                (
                    element, typeof(TElement)
                );

                var testCaseID = Convert.ToInt32
                (
                    TestContext.DataRow["TestCaseID"]
                );

                var expectedData = ExpectedDataSet.ElementAt
                (
                    testCaseID
                );

                var actualData = ((TElement)element).Data;

                Assert.IsTrue
                (
                    DataComparer(expectedData, actualData)
                );
            }
        }

        public virtual void IncompleteElementParsingTest()
        {
            using (var binaryReader = new BinaryReader( new MemoryStream() ))
            {
                WriteBencodeDataToStream(binaryReader.BaseStream);

                CheckExceptionThrowing<StreamEndException>
                (
                    Parser, binaryReader
                );
            }
        }

        public virtual void InvalidElementParsingTest()
        {
            using (var binaryReader = new BinaryReader( new MemoryStream() ))
            {
                int expectedOffset = randomizer.Next(256);

                WriteBencodeDataToStream
                (
                    binaryReader.BaseStream, expectedOffset
                );

                var catchedException = CheckExceptionThrowing<InvalidFormatException>
                (
                    Parser, binaryReader
                );

                var actualOffset = catchedException.Offset;

                Assert.AreEqual
                (
                    expectedOffset, actualOffset
                );
            }
        }

        private void WriteBencodeDataToStream(Stream stream, int offset = 0)
        {
            string bencodeString = Convert.ToString
            (
                TestContext.DataRow["BencodeData"]
            );

            byte[] streamData = Encoding.UTF8.GetBytes(bencodeString);

            if (offset > 0)
            {
                byte[] temp = new byte[offset + streamData.Length];

                for (int i = 0; i < offset; i++)
                {
                    temp[i] = 0;
                }

                Array.Copy(streamData, 0, temp, offset, streamData.Length);

                streamData = temp;
            }

            stream.Write
            (
                streamData, 0, streamData.Length
            );

            stream.Position = offset;
        }

        private TException CheckExceptionThrowing<TException>
        (
            IElementParser<IElement> parserToCheck, BinaryReader binaryReader
        )
        where TException : Exception
        {
            Exception catchedException = null;

            try
            {
                parserToCheck.Parse(binaryReader);
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
                catchedException, typeof(TException)
            );

            return (TException) catchedException;
        }
    }
}
