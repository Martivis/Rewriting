
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Rewriting.Services.TextComparer
{
    internal class TextComparer : ITextComparer
    {
        ITextCanonizer _textCanonazer;
        IShingleParser _parser;

        public TextComparer(ITextCanonizer textCanonazer, IShingleParser parser)
        {
            _textCanonazer = textCanonazer;
            _parser = parser;
        }

        public int Compare(string textA, string textB)
        {
            var canonizedA = _textCanonazer.Canonize(textA);
            var canonizedB = _textCanonazer.Canonize(textB);

            var shingleLentgh = GetShingleLength(canonizedA, canonizedB);

            var hashesA = GetHashedShingles(textA, shingleLentgh);
            var hashesB = GetHashedShingles(textB, shingleLentgh);

            var intersect = hashesA.Intersect(hashesB).ToList();
            var union = hashesA.Union(hashesB).ToList();

            return (int)((double)intersect.Count / union.Count * 100);
        }

        private List<Hash> GetHashedShingles(string text, int shingleLength)
        {
            var canonizedWords = _textCanonazer.Canonize(text);
            var hashes = _parser.ParseToShingles(canonizedWords, shingleLength);
            return hashes;
        }

        private int GetShingleLength(IList<string> canonizedA, IList<string> canonizedB)
        {
            var minWordsCount = Math.Min(canonizedA.Count, canonizedB.Count);
            if (minWordsCount < 50)
                return minWordsCount / 5 + 1;
            return 10;
        }
    }
}
