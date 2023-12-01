namespace Rewriting.Services.TextComparer
{
    public interface ITextComparer
    {
        int Compare(string textA, string textB);
        Task<int> CompareAsync(string textA, string textB);
    }
}