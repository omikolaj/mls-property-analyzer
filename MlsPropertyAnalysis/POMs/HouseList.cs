using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MlsPropertyAnalysis.POMs
{
    class HouseList
    {
        public HouseList(IWebDriver driver)
        {
            _driver = driver;
        }

        private IWebDriver _driver { get; }

        private IWebElement _houseList
        {
            get
            {
                return Wait.WaitUntilElementVisible(By.ClassName("list-container"), _driver, 10);                
            }
        }

        public IReadOnlyCollection<IWebElement> Houses
        {
            get
            {
                Thread.Sleep(1000);
                try
                {
                    var houseList = _houseList.FindElements(By.ClassName("item"));
                    return houseList;
                }
                catch
                {
                    return null;
                }
            }
        }

    }
}
