using Rewriting.Common.StringExtensions;
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

    public List<Hash> ParseToShingles(IList<string> words, int shingleLength)
    {
        ValidateParameters(words, shingleLength);
        
        var result = new List<Hash>(words.Count - shingleLength + 1);
        var shingle = new LinkedList<string>();

        FillFirstShingleFrom(words, shingle, shingleLength);
        AppendShingleToResult(shingle, result);

        for (int i = shingleLength; i < words.Count; i++)
        {
            MoveShingleOnto(words, i, shingle);
            AppendShingleToResult(shingle, result);
        }

        return result;
    }

    private static void ValidateParameters(IList<string> words, int shingleLength)
    {
        if (shingleLength <= 0)
            throw new ArgumentException("Shingle length should be greater then 0", nameof(shingleLength));
        if (shingleLength > words.Count)
            throw new ArgumentException("Shingle length should be smaller that words count", nameof(shingleLength));
    }

    private void FillFirstShingleFrom(IList<string> words, LinkedList<string> shingle, int shingleLength)
    {
        for (int i = 0; i < shingleLength; i++)
        {
            shingle.AddLast(words[i]);
        }
    }

    private void AppendShingleToResult(LinkedList<string>shingle, List<Hash> result)
    {
        var hash = _hashCounter.Hash(ShingleBytes(shingle));
        result.Add(hash);
    }

    private void MoveShingleOnto(IList<string> words, int nextIndex, LinkedList<string> shingle)
    {
        shingle.RemoveFirst();
        shingle.AddLast(words[nextIndex]);
    }

    private byte[] ShingleBytes(LinkedList<string> shingle)
    {
        var result = new List<byte>();
        foreach (var word in shingle)
        {
            result.AddRange(word.ToByteArray());
        }
        return result.ToArray();
    }
}
