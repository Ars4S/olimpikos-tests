using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Olimpoks.Tests.Pages;

public sealed class HomePage : BasePage
{
    private const string BaseUrl = "https://olimpoks.ru/";

    public HomePage(IWebDriver driver) : base(driver)
    {
    }

    public HomePage Open()
    {
        Driver.Navigate().GoToUrl(BaseUrl);
        return this;
    }

    public CatalogPage NavigateToLaborProtectionCatalog()
    {
        var solutionsMenu = WaitToBeClickable(
            By.XPath("//a[contains(@class,'menu-top_list-el-link') and normalize-space()='Решения']"));
        new Actions(Driver).MoveToElement(solutionsMenu).Perform();

        var laborProtectionLink = Wait.Until(driver =>
        {
            var link = driver.FindElement(
                By.XPath("//a[contains(@class,'menu-top_tab-link') and @href='/catalog/?s=1244']"));
            return link.Displayed ? link : null;
        })!;

        ScrollIntoView(laborProtectionLink);
        ClickViaJs(laborProtectionLink);

        return new CatalogPage(Driver);
    }
}
