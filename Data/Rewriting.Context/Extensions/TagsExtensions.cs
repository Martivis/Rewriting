using Microsoft.EntityFrameworkCore;

namespace Rewriting.Context.Extensions;

public static class TagsExtensions
{
    public static IQueryable<T> ForUpdate<T>(this IQueryable<T> set, bool skipLocked = false)
    {
        var option = skipLocked ? "FOR UPDATE SKIP LOCKED" : "FOR UPDATE";
        return set.TagWith(option);
    }
}