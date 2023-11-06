using Rewriting.Common.StringExtensions;
using System.IO.Hashing;

namespace Rewriting.Services.TextComparer;

internal class Crc32HashCounter : IHashCounter
{
    public byte[] Hash(byte[] data)
    {
        return Crc32.Hash(data);
    }

    public byte[] Hash(string data)
    {
        return Hash(data.ToByteArray());
    }
}
