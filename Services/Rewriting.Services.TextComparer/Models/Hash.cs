namespace Rewriting.Services.TextComparer;

internal class Hash
{
    private byte[] _hash;

    public Hash(byte[] hash)
    {
        _hash = hash;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Hash other)
        {
            return Enumerable.SequenceEqual(_hash, other._hash);
        }
        else
            return false;
    }

    public override int GetHashCode()
    {
        //return new string(_hash.SelectMany(b => b.ToString()).ToArray()).GetHashCode();

        return _hash.Select(b => (int)b).Sum();
    }
}
