using Olimpoks.Tests.Infrastructure;
using OpenQA.Selenium;

namespace Olimpoks.Tests.Tests;

public abstract class WebDriverTestBase : IDisposable
{
    protected readonly IWebDriver Driver;

    protected WebDriverTestBase()
    {
        Driver = WebDriverFactory.CreateChrome();
    }

    public void Dispose()
    {
        Driver.Quit();
        Driver.Dispose();
    }
}
