using Olimpoks.Tests.Pages;
using Olimpoks.Tests.Tests;

namespace Olimpoks.Tests;

public sealed class VideoInstructionsExportTests : WebDriverTestBase
{
    [Fact]
    public void Export_VideoInstructions_ForWorkersAndInstruction_ToFile()
    {
        var outputPath = Path.Combine(
            AppContext.BaseDirectory,
            "..", "..", "..",
            "Output",
            "courses.txt");

        outputPath = Path.GetFullPath(outputPath);

        var catalog = new HomePage(Driver)
            .Open()
            .NavigateToLaborProtectionCatalog()
            .WaitForCatalogLoaded()
            .SelectWorkersCategory()
            .SelectInstructionTrainingType();

        var courses = catalog.GetVisibleVideoInstructionCourses();

        Assert.NotEmpty(courses);
        Assert.All(courses, course =>
        {
            Assert.Contains("инструктаж", course.Name, StringComparison.OrdinalIgnoreCase);
            Assert.False(string.IsNullOrWhiteSpace(course.Code));
            Assert.True(course.UpdatesCount > 0);
        });

        catalog.ExportCoursesToFile(courses, outputPath);

        Assert.True(File.Exists(outputPath));
        var lines = File.ReadAllLines(outputPath);
        Assert.Equal(courses.Count, lines.Length);
        Assert.Contains("инструктаж", lines[0], StringComparison.OrdinalIgnoreCase);
        Assert.Matches(@".+\(.+\)\s–\s\d+", lines[0]);
    }
}
