namespace Rewriting.Services.TextComparer.Tests
{
    public class ShingleParserTests
    {
        private IHashCounter _hash;
        private IShingleParser _parser;

        [SetUp]
        public void Setup()
        {
            _hash = new Crc32HashCounter();
            _parser = new ShingleParser(_hash);
        }

        [Test]
        public void ParseToShingles_5words_CorrectParsing()
        {
            // Assert
            var words = new List<string>
            {
                "word1",
                "word2",
                "word3",
                "word4",
                "word5"
            };
            var shingleLength = 3;
            var expected = new List<byte[]>
            {
                _hash.Hash("word1word2word3".Select(c => (byte)c).ToArray()),
                _hash.Hash("word2word3word4".Select(c => (byte)c).ToArray()),
                _hash.Hash("word3word4word5".Select(c => (byte)c).ToArray()),
            };

            // Act
            var actual = _parser.ParseToShingles(words, shingleLength);

            // Arrange
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }
}