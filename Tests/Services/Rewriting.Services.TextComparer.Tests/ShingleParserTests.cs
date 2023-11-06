using Rewriting.Common.StringExtensions;

namespace Rewriting.Services.TextComparer.Tests
{
    [TestFixture]
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

        [TestCase(new[] { "word1", "word2", "word3", "word4", "word5" }, new[] { "word1", "word2", "word3", "word4", "word5" }, 1 )]
        [TestCase(new[] { "word1", "word2", "word3", "word4", "word5" }, new[] { "word1word2", "word2word3", "word3word4", "word4word5" }, 2 )]
        [TestCase(new[] { "word1", "word2", "word3", "word4", "word5" }, new[] { "word1word2word3", "word2word3word4", "word3word4word5" }, 3 )]
        [TestCase(new[] { "word1", "word2", "word3", "word4", "word5" }, new[] { "word1word2word3word4word5" }, 5 )]
        public void ParseToShingles_Tests(string[] words, string[] shingles, int shingleLength)
        {
            // Assert
            var expected = shingles.Select(_hash.Hash);

            // Act
            var actual = _parser.ParseToShingles(words, shingleLength);

            // Arrange
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }
}