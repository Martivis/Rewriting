using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.TextComparer;

internal class TextCanonizer : ITextCanonizer
{
    public List<string> Canonize(string text)
    {
        var minWordLength = 3;
        var words = text.ToLower().Split();

        var cleanedWords = words.AsParallel().Select(s => new string(s.Where(char.IsLetter).ToArray()));
        var bigWords = cleanedWords.Where(c => c.Length > minWordLength);

        return bigWords.ToList();
    }
}
