namespace Rewriting.Services.Contracts;

public class ResultCompareModel
{
    public Guid ResultUid { get; set; }
    public string SourceText { get; set; }
    public string ResultText { get; set; }
}