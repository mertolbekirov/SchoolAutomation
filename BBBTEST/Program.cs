using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using System.Threading;

namespace BBBTEST
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://test.bigbluebutton.org");
            driver.FindElement(By.Id("username")).SendKeys("asd");
            driver.FindElement(By.CssSelector("body > div > div.container > div:nth-child(1) > div > div > form > input.submit_btn.button.success.large")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.FindElement(By.CssSelector("[aria-label='Listen only']")).Click();
        }
    }
}
