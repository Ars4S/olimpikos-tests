namespace Olimpoks.Tests.Models;

public sealed class CourseInfo
{
    public required string Name { get; init; }
    public required string Code { get; init; }
    public required int UpdatesCount { get; init; }

    public string ToExportLine() => $"{Name} ({Code}) – {UpdatesCount}";
}
