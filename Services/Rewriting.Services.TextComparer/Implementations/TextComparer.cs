

using System.Text;

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
            var canonizedTextA = _textCanonazer.Canonize(textA);
            var canonizedTextB = _textCanonazer.Canonize(textB);

            throw new NotImplementedException();
        }

        private List<byte[]> GetHashedShingles(string text)
        {
            var canonized = _textCanonazer.Canonize(text);

            throw new NotImplementedException();
        }

        
    }
}
