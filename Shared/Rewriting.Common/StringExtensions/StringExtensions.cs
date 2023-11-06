
namespace Rewriting.Common.StringExtensions;

public static class StringExtensions
{
    public static byte[] ToByteArray(this string str)
    {
        return str.Select(c => (byte)c).ToArray();
    }
}
