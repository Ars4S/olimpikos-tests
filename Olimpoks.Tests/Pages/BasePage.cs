using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Olimpoks.Tests.Pages;

public abstract class BasePage
{
    protected readonly IWebDriver Driver;
    protected readonly WebDriverWait Wait;

    protected BasePage(IWebDriver driver, int timeoutSeconds = 30)
    {
        Driver = driver;
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
    }

    protected void ScrollIntoView(IWebElement element)
    {
        ((IJavaScriptExecutor)Driver).ExecuteScript(
            "arguments[0].scrollIntoView({block: 'center'});",
            element);
    }

    protected void ClickViaJs(IWebElement element)
    {
        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
    }

    protected IWebElement WaitToBeClickable(By locator)
    {
        return Wait.Until(driver =>
        {
            var element = driver.FindElement(locator);
            return element.Enabled && element.Displayed ? element : null;
        })!;
    }

    protected void WaitUntilInvisible(By locator)
    {
        Wait.Until(driver =>
        {
            var elements = driver.FindElements(locator);
            return elements.Count == 0 || elements.All(e => !e.Displayed);
        });
    }
}
