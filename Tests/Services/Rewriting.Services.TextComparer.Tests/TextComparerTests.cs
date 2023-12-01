
namespace Rewriting.Services.TextComparer.Tests
{
    [TestFixture]
    public class TextComparerTests
    {
        private ITextComparer _textComparer;

        [SetUp]
        public void SetUp()
        {
            var canonizer = new TextCanonizer();
            var hashCounter = new Crc32HashCounter();
            var parser = new ShingleParser(hashCounter);
            _textComparer = new TextComparer(canonizer, parser);
        }

        [TestCase("abcd abcd", "abcd abcd")]
        [TestCase("ababa ababa ababa ababa ababa ababa ababa", "ababa ababa ababa ababa ababa ababe ababa")]
        [TestCase("abcd abcd abab ababa ababa ababa", "abcd abcd abab ababa ababa ababb")]
        [TestCase("abcd abcd abab ababa ababa ababa", "abcd abcd abab adada")]
        [TestCase("ababa ababa ababa ababa ababa", "adada adada adada adada adada")]
        [TestCase("ababa ababa ababa ababa ababa ababa ababa ababa ababa ababa ababa ababa ababa ababa ababa ababa ababa ababa ababa ababa ababa", "ababa ababa ababa ababa ababa ababa ababa ababa ababa ababa ababa ababa ababa ababa ababa ababa ababa ababa ababa ababa ababe")]
        public void Compare_Test(string text1, string text2)
        {
            var res = _textComparer.Compare(text1, text2);
            Console.WriteLine($"Method returned {res}");
        }
    }
}
