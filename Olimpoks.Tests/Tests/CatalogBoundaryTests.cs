using Olimpoks.Tests.Pages;
using Olimpoks.Tests.Tests;

namespace Olimpoks.Tests;

public sealed class CatalogBoundaryTests : WebDriverTestBase
{
    private CatalogPage OpenCatalog()
    {
        return new HomePage(Driver)
            .Open()
            .NavigateToLaborProtectionCatalog()
            .WaitForCatalogLoaded();
    }

    /// <summary>
    /// Минимальная длина поискового запроса на сайте — 2 символа (catalog.js, SEARCH_MIN_LENGTH).
    /// При 1 символе фильтрация не применяется.
    /// </summary>
    [Fact]
    public void Search_WithSingleCharacter_DoesNotReduceVisibleCourses()
    {
        var catalog = OpenCatalog();
        var baselineCount = catalog.GetVisibleCoursesCount();

        catalog.SearchCourses("а");
        var countAfterShortSearch = catalog.GetVisibleCoursesCount();

        Assert.True(baselineCount > 0);
        Assert.Equal(baselineCount, countAfterShortSearch);
    }

    /// <summary>
    /// Несуществующий длинный запрос должен дать 0 видимых курсов и сообщение в блоке счётчика.
    /// </summary>
    [Fact]
    public void Search_WithNonExistentLongQuery_ReturnsZeroCourses()
    {
        var catalog = OpenCatalog();
        var nonsenseQuery = "zzzznonexistentcourse12345";

        catalog.SearchCourses(nonsenseQuery);

        Assert.Equal(0, catalog.GetVisibleCoursesCount());

        var countText = catalog.GetProductsCountText();
        Assert.Contains(nonsenseQuery, countText, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("ничего не найдено", countText, StringComparison.OrdinalIgnoreCase);
    }
}
