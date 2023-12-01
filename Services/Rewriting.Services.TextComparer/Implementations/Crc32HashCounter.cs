using Rewriting.Common.StringExtensions;
using System.IO.Hashing;

namespace Rewriting.Services.TextComparer;

internal class Crc32HashCounter : IHashCounter
{
    public Hash Hash(byte[] data)
    {
        return new Hash(Crc32.Hash(data));
    }

    public Hash Hash(string data)
    {
        return Hash(data.ToByteArray());
    }
}
