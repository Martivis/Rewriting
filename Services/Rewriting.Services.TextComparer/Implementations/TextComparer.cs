
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Rewriting.Services.TextComparer
{
    internal class TextComparer : ITextComparer
    {
        private readonly ITextCanonizer _textCanonizer;
        private readonly IShingleParser _parser;

        public TextComparer(ITextCanonizer textCanonizer, IShingleParser parser)
        {
            _textCanonizer = textCanonizer;
            _parser = parser;
        }

        public int Compare(string textA, string textB)
        {
            var canonizedA = _textCanonizer.Canonize(textA);
            var canonizedB = _textCanonizer.Canonize(textB);

            var shingleLength = GetShingleLength(canonizedA, canonizedB);

            var hashesA = GetHashedShingles(textA, shingleLength);
            var hashesB = GetHashedShingles(textB, shingleLength);

            var intersect = hashesA.Intersect(hashesB).ToList();
            var union = hashesA.Union(hashesB).ToList();

            return (int)((double)intersect.Count / union.Count * 100);
        }

        public Task<int> CompareAsync(string textA, string textB)
        {
            return Task.Run(() => Compare(textA, textB));
        }

        private List<Hash> GetHashedShingles(string text, int shingleLength)
        {
            var canonizedWords = _textCanonizer.Canonize(text);
            var hashes = _parser.ParseToShingles(canonizedWords, shingleLength);
            return hashes;
        }

        private static int GetShingleLength(ICollection<string> canonizedA, ICollection<string> canonizedB)
        {
            var minWordsCount = Math.Min(canonizedA.Count, canonizedB.Count);
            if (minWordsCount < 50)
                return minWordsCount / 10 + 1;
            return 10;
        }
    }
}
