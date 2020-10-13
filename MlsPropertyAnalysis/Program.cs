using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Microsoft.Office.Interop.Excel;
using MlsPropertyAnalysis.POMs;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MlsPropertyAnalysis
{
    class Program
    {
        public const Grades MinimumAcceptableNeighborhoodGrade = Grades.C;

        static void Main(string[] args)
        {
            IWebDriver driver = new ChromeDriver
            {
                Url = "https://portal.onehome.com/en-US/properties/map?token=eyJPU04iOiJZRVMtTUxTIiwidHlwZSI6IjEiLCJjb250YWN0aWQiOjE3MjQwMzgsInNldGlkIjoiMTAzNDkwNyIsInNldGtleSI6IjEzNCIsImVtYWlsIjoib21pa29sYWoxQGdtYWlsLmNvbSIsInJlc291cmNlaWQiOjAsImFnZW50aWQiOjM2MzIxMCwiaXNkZWx0YSI6ZmFsc2V9&searchId=e10abdb1-869b-3e1a-a413-cde27f0cdf10",
            };

            driver.Manage().Window.Maximize();
            
            Thread.Sleep(1000);

            var modal = Wait.WaitUntilElementVisible(By.ClassName("modal-content"), driver, 1000);

            if(modal != null)
            {
                var button = modal.FindElement(By.TagName("button"));
                button.Click();
            }

            HouseList list = new HouseList(driver);

            var houses = list.Houses;

            for (int i = 0; i < houses.Count; i++)
            {
                PropertyInfo propertyInfo = new PropertyInfo();
                try
                {
                    try
                    {
                        var house = new HouseList(driver).Houses.ElementAt(i);
                        OpenQA.Selenium.Interactions.Actions actions = new OpenQA.Selenium.Interactions.Actions(driver);                        
                        actions.MoveToElement(house).Build().Perform();

                        IJavaScriptExecutor je = (IJavaScriptExecutor)driver;                        
                        je.ExecuteScript("arguments[0].scrollIntoView(true);", house);
                        house.Click();

                    }
                    catch
                    {
                        var modal3 = Wait.WaitUntilElementVisible(By.ClassName("fsrInvite"), driver, 10);

                        if (modal3 != null)
                        {
                            var button = modal3.FindElement(By.TagName("button"));
                            button.Click();
                        }

                        var house = new HouseList(driver).Houses.ElementAt(i);
                        OpenQA.Selenium.Interactions.Actions actions = new OpenQA.Selenium.Interactions.Actions(driver);
                        actions.MoveToElement(house).Build().Perform();

                        actions.MoveToElement(house).Build().Perform();

                        IJavaScriptExecutor je = (IJavaScriptExecutor)driver;
                        je.ExecuteScript("arguments[0].scrollIntoView(true);", house);
                        house.Click();
                    }                

                    var status = Wait.WaitUntilElementVisible(By.ClassName("heading"), driver);
                    if (status != null)
                    {
                        if (status.Text == "For Sale")
                        {
                            string city = string.Empty;
                            propertyInfo.Address = ExtractHouseAddress(driver, out city);
                            Console.WriteLine($"Analyzing: {propertyInfo.Address}");

                            propertyInfo.ZipCode = int.Parse(propertyInfo.Address.Split('_').Last());

                            if (ExistsInDesiredNeighborhood(propertyInfo.ZipCode, city) == true)
                            {
                                propertyInfo.Price = ExtractHousePrice(driver);

                                ExtractPropertyDetails(driver, ref propertyInfo);

                                ExcelWorker worker = new ExcelWorker
                                {
                                    PropertyInfo = propertyInfo
                                };

                                worker.AnalyzeProperty();
                            }                      
                        }
                    }

                    //driver.Navigate().Back();
                }
                //catch (Exception ex)
                //{
                //    driver.Navigate().Back();
                //}
                finally
                {
                    driver.Navigate().Back();
                }
            }
        }

        static bool ExistsInDesiredNeighborhood(int zipCode, string city = "")
        {
            Neighborhoods neighborhoods = new Neighborhoods();

            Neighborhood zipCodeGrades = neighborhoods.GetZipCodeGrades(zipCode);

            if (zipCodeGrades.Grades.Any(g => g.Grade <= MinimumAcceptableNeighborhoodGrade))
            {
                // filter the list of neighborhood grades to the ones that meet minimum 
                var filtered = zipCodeGrades.Grades.Select(g => g).Where(g => g.Grade <= MinimumAcceptableNeighborhoodGrade).ToList();

                // find out if any of the grades match the city name if they do return
                if(filtered.Any(g => g.Cities.Any(c => c == city)))
                {
                    return true;
                }
            }

            return false;
        }

        static void ExtractPropertyDetails(IWebDriver driver, ref PropertyInfo propertyInfo)
        {
            IReadOnlyCollection<IWebElement> detailSections = driver.FindElements(By.ClassName("property-section"));

            foreach (var section in detailSections)
            {
                if (section.Text == "Other Facts & Features")
                {
                    section.Click();
                    var propertyDetailsSections = section.FindElements(By.TagName("aotf-property-details"));
                    foreach (var propertyDetail in propertyDetailsSections)
                    {
                        try
                        {
                            var sectionHeader = propertyDetail.FindElement(By.TagName("h3"));
                            if (sectionHeader.Text == "LEGAL & FINANCIAL DETAILS")
                            {
                                var values = propertyDetail.FindElements(By.TagName("dd"));
                                string annualTaxes = values.Last().Text ?? string.Empty;
                                if (annualTaxes != string.Empty)
                                {
                                    annualTaxes = annualTaxes.Replace("$", "");
                                    annualTaxes = annualTaxes.Replace(",", "");
                                    propertyInfo.AnnualTaxes = annualTaxes;
                                    break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                }
            }
        }

        static string ExtractHouseAddress(IWebDriver driver, out string city)
        {
            IWebElement address = Wait.WaitUntilElementVisible(By.TagName("address"), driver);

            var fullAddress = address.FindElement(By.ClassName("address-line-one")).Text;
            city = address.FindElement(By.ClassName("address-line-two")).Text.Split(',').First() ?? string.Empty;
            fullAddress += " " + address.FindElement(By.ClassName("address-line-two")).Text;
            fullAddress = fullAddress.Replace(",", "");
            fullAddress = fullAddress.Replace(" ", "_");
            return fullAddress;
        }

        static string ExtractHousePrice(IWebDriver driver)
        {
            IWebElement price = Wait.WaitUntilElementVisible(By.ClassName("price"), driver, 3);

            if (price != null)
            {
                string housePrice = price.Text ?? string.Empty;
                if (housePrice != string.Empty)
                {
                    housePrice = housePrice.Replace("$", "");
                    housePrice = housePrice.Replace(",", "");
                    return housePrice;
                }
            }

            return string.Empty;
        }
    }
}
