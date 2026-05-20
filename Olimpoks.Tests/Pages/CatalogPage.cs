using Olimpoks.Tests.Helpers;
using Olimpoks.Tests.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Olimpoks.Tests.Pages;

public sealed class CatalogPage : BasePage
{
    public const string WorkersFilterValue = "1033";
    public const string InstructionFilterValue = "1039";
    public const string VideoInstructionsSectionTitle = "Видеоинструктажи";

    private readonly By _catalogLoadedLocator = By.CssSelector("#products_container .product-card");
    private readonly By _spinnerLocator = By.CssSelector("#products_container .spinner-border");
    private readonly By _productsCountLocator = By.CssSelector(".products-count");
    private readonly By _searchInputLocator = By.Id("catalog-search-bar-input");
    private readonly By _searchButtonLocator = By.Id("catalog-search-bar-search-btn");
    private readonly By _clearFiltersButtonLocator = By.CssSelector(".catalog-filters_button-clear-filter");

    public CatalogPage(IWebDriver driver) : base(driver)
    {
    }

    public CatalogPage WaitForCatalogLoaded()
    {
        try
        {
            WaitUntilInvisible(_spinnerLocator);
        }
        catch (WebDriverTimeoutException)
        {
            // Спиннер мог не появиться — продолжаем, если карточки уже загружены.
        }

        Wait.Until(driver => driver.FindElements(_catalogLoadedLocator).Count > 0);
        return this;
    }

    public CatalogPage SelectWorkersCategory()
    {
        ToggleFilterCheckbox("#PERSONAL_CATEGORY", WorkersFilterValue);
        return this;
    }

    public CatalogPage SelectInstructionTrainingType()
    {
        ToggleFilterCheckbox("#TRAINING_TYPE", InstructionFilterValue);
        return this;
    }

    public CatalogPage ClearAllFilters()
    {
        var clearButton = Driver.FindElement(_clearFiltersButtonLocator);
        if (clearButton.Displayed && clearButton.Enabled)
        {
            clearButton.Click();
        }

        return this;
    }

    public CatalogPage SearchCourses(string query)
    {
        var input = WaitToBeClickable(_searchInputLocator);
        input.Clear();
        input.SendKeys(query);
        WaitToBeClickable(_searchButtonLocator).Click();
        return this;
    }

    public int GetVisibleCoursesCount()
    {
        return Driver.FindElements(By.CssSelector(".product-card_course-card-wrapper.show")).Count;
    }

    public string GetProductsCountText()
    {
        return Driver.FindElement(_productsCountLocator).Text.Trim();
    }

    public IReadOnlyList<CourseInfo> GetVisibleVideoInstructionCourses()
    {
        var section = Driver.FindElements(By.CssSelector(".product-card"))
            .FirstOrDefault(product =>
            {
                var headers = product.FindElements(By.CssSelector(".product-card_description-b1-header h2"));
                return headers.Any(h => h.Text.Trim() == VideoInstructionsSectionTitle);
            });

        if (section is null)
        {
            throw new InvalidOperationException($"Секция «{VideoInstructionsSectionTitle}» не найдена на странице.");
        }

        ScrollIntoView(section);

        var expandButton = section.FindElements(By.CssSelector(".product-card_button-show-courses"))
            .FirstOrDefault();
        if (expandButton is not null && expandButton.Displayed)
        {
            ClickViaJs(expandButton);
        }

        var courses = section.FindElements(By.CssSelector(".product-card_course-card-wrapper.show"));
        var result = new List<CourseInfo>();

        foreach (var course in courses)
        {
            var name = course.FindElement(By.CssSelector(".course-card_name")).Text.Trim();
            var cipher = course.FindElement(By.CssSelector(".course-card_code-number")).Text.Trim();
            var (code, updates) = CourseCipherParser.Parse(cipher);

            result.Add(new CourseInfo
            {
                Name = name,
                Code = code,
                UpdatesCount = updates
            });
        }

        return result;
    }

    public void ExportCoursesToFile(IEnumerable<CourseInfo> courses, string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var lines = courses.Select(c => c.ToExportLine()).ToArray();
        File.WriteAllLines(filePath, lines);
    }

    private void ToggleFilterCheckbox(string sectionId, string value)
    {
        var checkbox = Wait.Until(driver =>
        {
            var element = driver.FindElement(
                By.CssSelector($"{sectionId} input[type='checkbox'][value='{value}']"));
            return element.Displayed ? element : null;
        });

        ScrollIntoView(checkbox);

        if (!checkbox.Selected)
        {
            ClickViaJs(checkbox);
        }

        WaitForFilterApplied();
    }

    private void WaitForFilterApplied()
    {
        Thread.Sleep(300);
        Wait.Until(driver => driver.FindElements(_catalogLoadedLocator).Count > 0);
    }
}
