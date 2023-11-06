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

    private List<Hash>? _result;
    private LinkedList<string>? _shingle;
    private int _shingleLength;

    public ShingleParser(IHashCounter hashCounter)
    {
        _hashCounter = hashCounter;
    }

    public List<Hash> ParseToShingles(IList<string> words, int shingleLength)
    {
        ValidateParameters(words, shingleLength);

        _shingleLength = shingleLength;
        _result = new List<Hash>(words.Count - shingleLength + 1);
        _shingle = new LinkedList<string>();

        FillFirstShingleFrom(words);
        AppendShingleToResult();

        for (int i = shingleLength; i < words.Count; i++)
        {
            MoveShingleOnto(words, i);
            AppendShingleToResult();
        }

        return _result;
    }

    private static void ValidateParameters(IList<string> words, int shingleLength)
    {
        if (shingleLength <= 0)
            throw new ArgumentException("Shingle length should be greater then 0", nameof(shingleLength));
        if (shingleLength > words.Count)
            throw new ArgumentException("Shingle length should be smaller that words count", nameof(shingleLength));
    }

    private void FillFirstShingleFrom(IList<string> words)
    {
        for (int i = 0; i < _shingleLength; i++)
        {
            _shingle!.AddLast(words[i]);
        }
    }

    private void AppendShingleToResult()
    {
        var hash = _hashCounter.Hash(ShingleBytes());
        _result!.Add(hash);
    }

    private void MoveShingleOnto(IList<string> words, int nextIndex)
    {
        _shingle!.RemoveFirst();
        _shingle!.AddLast(words[nextIndex]);
    }

    private byte[] ShingleBytes()
    {
        var result = new List<byte>();
        foreach (var word in _shingle!)
        {
            result.AddRange(word.ToByteArray());
        }
        return result.ToArray();
    }
}
