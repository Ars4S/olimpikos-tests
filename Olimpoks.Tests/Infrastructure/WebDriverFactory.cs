using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Olimpoks.Tests.Infrastructure;

public static class WebDriverFactory
{
    public static IWebDriver CreateChrome()
    {
        var options = new ChromeOptions();
        options.AddArgument("--window-size=1920,1080");
        options.AddArgument("--disable-notifications");
        options.AddArgument("--lang=ru-RU");

        if (Environment.GetEnvironmentVariable("HEADLESS") == "1")
        {
            options.AddArgument("--headless=new");
        }

        var driver = new ChromeDriver(options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
        return driver;
    }
}
