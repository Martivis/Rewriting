using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Rewriting.Context.Interceptors;

public class TaggedQueryCommandInterceptor : DbCommandInterceptor
{
    private const string HINT_TEXT = "-- ";
    
    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command, 
        CommandEventData eventData, 
        InterceptionResult<DbDataReader> result)
    {
        AddRequestedStatement(command);
        return base.ReaderExecuting(command, eventData, result);
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command, 
        CommandEventData eventData, 
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        AddRequestedStatement(command);
        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    private static void AddRequestedStatement(DbCommand command)
    {
        if (command.CommandText.StartsWith(HINT_TEXT))
        {
            var option = "\r\n" + ExtractHint(command.CommandText);
            command.CommandText += option;
        }
    }

    private static string ExtractHint(string commandText)
    {
        var chars = commandText.Skip(HINT_TEXT.Length).TakeWhile(c => c != '\n').ToArray();
        return new string(chars).Trim();
    }
}