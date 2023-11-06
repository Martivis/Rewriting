using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.TextComparer;

internal class ShingleParser : IShingleParser
{
    private readonly IHashCounter _hashCounter;

    public ShingleParser(IHashCounter hashCounter)
    {
        _hashCounter = hashCounter;
    }

    public List<byte[]> ParseToShingles(IList<string> words, int shingleLength)
    {
        var result = new List<byte[]>(words.Count - shingleLength + 1);

        var shingle = new LinkedList<string>();
        for (int i = 0; i < shingleLength; i++)
        {
            shingle.AddLast(words[i]);
        }

        var hash = _hashCounter.Hash(ToByte(shingle));
        result.Add(hash);

        for (int i = shingleLength; i < words.Count; i++)
        {
            shingle.RemoveFirst();
            shingle.AddLast(words[i]);

            hash = _hashCounter.Hash(ToByte(shingle));
            result.Add(hash);
        }

        return result;
    }

    private byte[] ToByte(LinkedList<string> list)
    {
        var result = new List<byte>();
        foreach (var word in list)
        {
            result.AddRange(word.ToCharArray().Select(c => (byte)c));
        }
        return result.ToArray();
    }
}
