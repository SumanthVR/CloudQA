using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

class CloudQATest
{
    static void Main()
    {
        IWebDriver driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        try
        {
            driver.Navigate().GoToUrl("https://app.cloudqa.io/home/AutomationPracticeForm");

            // === 1. Fill First Name (Standard DOM) ===
            var firstName = wait.Until(d => d.FindElement(By.Name("firstname")));
            firstName.Clear();
            firstName.SendKeys("John");
            Console.WriteLine("✅ First Name entered.");

            // === 2. Gender: Male (Shadow DOM) ===
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            // Locate and access shadow host (update selector if needed)
            IWebElement shadowHost = driver.FindElement(By.CssSelector("div#shadow_host")); // Adjust selector
            IWebElement shadowRoot = (IWebElement)js.ExecuteScript("return arguments[0].shadowRoot", shadowHost);
            IWebElement maleRadio = shadowRoot.FindElement(By.XPath("//label[contains(text(), 'Male')]/preceding-sibling::input"));
            maleRadio.Click();
            Console.WriteLine("✅ Gender selected.");

            // === 3. Mobile Number (inside iFrame) ===
            driver.SwitchTo().Frame(0); // Assuming first iFrame contains mobile field
            IWebElement mobile = wait.Until(d => d.FindElement(By.Name("mobile")));
            mobile.SendKeys("9876543210");
            Console.WriteLine("✅ Mobile number entered.");
            driver.SwitchTo().DefaultContent(); // Exit iFrame

            Console.WriteLine("\n All fields filled successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occurred: " + ex.Message);
        }
        finally
        {
            driver.Quit();
        }
    }
}
