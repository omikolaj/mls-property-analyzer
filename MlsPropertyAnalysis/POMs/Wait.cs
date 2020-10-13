using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlsPropertyAnalysis.POMs
{
    public class Wait
    {
        public static IWebElement WaitUntilElementVisible(By elementLocator, IWebDriver driver, int timeout = 10)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                return wait.Until(ExpectedConditions.ElementIsVisible(elementLocator));
            }
            catch
            {
                Console.WriteLine("Element with locator: '" + elementLocator + "' was not found.");
                return null;
            }
        }
    }
}
