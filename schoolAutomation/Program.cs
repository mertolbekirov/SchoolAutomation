using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace schoolAutomation
{
    class Program
    {
        static void Main(string[] args)
        {
            string username, password;
            List<string> classes = new List<string>();
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--disable-extensions"); // to disable extension
            options.AddArguments("--disable-notifications"); // to disable notification

            SetCredentials(out username, out password);
            Console.WriteLine("FORMAT FOR CLASS NAMES!");
            Console.WriteLine("Bulgarian - bg, English - eng, Geography - geo, Informacionnni- inf, Istoriq - ist, Matematika - mat, Nemski - nemski, Svqt i lichnost - svqt, FVS - fvs");
            Console.WriteLine("Fizikata ne e supportvana");
            Console.WriteLine("If you've written all the classes, type 'quit' to continue with the setup");
            for (int i = 0; i < 7; i++)
            {
                Console.Write($"Enter the name of class number {i+1}. MAKE SURE IT IS IN THE CORRECT FORMAT!: ");
                string classname = Console.ReadLine();
                if (classname == "quit")
                {
                    break;
                }
                classes.Add(classname);
            }

            Console.Write("How many classes have already passed?: ");
            int passedClasses = int.Parse(Console.ReadLine());
            int minutesToWait = 0;
            if (passedClasses >= 3)
            {
                minutesToWait += 10;
            }

            IWebDriver driver = new ChromeDriver(options);
            LogIn(username, password, driver);

            for (int i = 0; i < classes.Count; i++)
            {
                if(passedClasses > i)
                {
                    minutesToWait += 50;
                    continue;
                }
                WaitUntilClassesStart(minutesToWait, passedClasses);

                driver.Navigate().GoToUrl(GetClassURL(classes[i]));

                ClickJoinButtonWhenAvailable(driver);

                ClickListenOnlyAndWait40Mins(driver);

                driver.Close();
                driver.SwitchTo().Window(driver.WindowHandles.Last());
            }

        }

        private static void ClickListenOnlyAndWait40Mins(IWebDriver driver)
        {
            while (true)
            {
                try
                {
                    driver.SwitchTo().Window(driver.WindowHandles.Last());
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
                    driver.FindElement(By.CssSelector("[aria-label='Listen only']")).Click();

                    Thread.Sleep(2400000);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Attempting to find it again once the page has loaded.");
                    continue;
                }
            }
            
        }

        private static void LogIn(string username, string password, IWebDriver driver)
        {
            driver.Navigate().GoToUrl("https://u4ili6teto.bg/1sou-tg/login/index.php");
            driver.FindElement(By.Name("username")).SendKeys(username);
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.Id("loginbtn")).Click();
        }

        private static void SetCredentials(out string username, out string password)
        {
            Console.Write("Please enter username: ");
            username = Console.ReadLine();
            Console.Write("Please enter password: ");
            password = Console.ReadLine();
        }

        private static void WaitUntilClassesStart(int minutesToWait, int passedClasses)
        {
            TimeSpan alertTime = new TimeSpan(13, 15, 00);
            DateTime current = DateTime.Now;
            var ts = new TimeSpan(minutesToWait / 60, minutesToWait % 60, 0);
            var finalAlertTime = alertTime + ts;
            TimeSpan timeToGo = finalAlertTime - current.TimeOfDay;
            if((int)(timeToGo.TotalMilliseconds) < 0)
            {
                return;
            }
            Thread.Sleep((int)(timeToGo.TotalMilliseconds));
        }

        private static void ClickJoinButtonWhenAvailable(IWebDriver driver)
        {
            try
            {
                var dropdown = driver.FindElement(By.Name("group"));
                var selectElement = new SelectElement(dropdown);

                int counter = 1;
                
                while (driver.FindElement(By.Id("join_button_input")).Enabled == false)
                {
                    if(counter % 2 == 0)
                    {
                        selectElement.SelectByText("12 А");
                    }else
                    {
                        selectElement.SelectByText("Всички участници");
                    }
                    Thread.Sleep(30000);
                    driver.Navigate().Refresh();
                }

                driver.FindElement(By.Id("join_button_input")).Click();
            }
            catch
            {
                while (driver.FindElement(By.Id("join_button_input")).Enabled == false)
                {
                    Thread.Sleep(30000);
                }

                driver.FindElement(By.Id("join_button_input")).Click();
            }
            
        }

        private static string GetClassURL(string className)
        {
            switch (className)
            {
                case "bg": return "http://u4ili6teto.bg/1sou-tg/mod/bigbluebuttonbn/view.php?id=68550";
                case "eng": return "http://u4ili6teto.bg/1sou-tg/mod/bigbluebuttonbn/view.php?id=69179";
                case "geo": return "http://u4ili6teto.bg/1sou-tg/mod/bigbluebuttonbn/view.php?id=69863";
                case "inf": return "http://u4ili6teto.bg/1sou-tg/mod/bigbluebuttonbn/view.php?id=69638";
                case "ist": return "http://u4ili6teto.bg/1sou-tg/mod/bigbluebuttonbn/view.php?id=68541";
                case "mat": return "http://u4ili6teto.bg/1sou-tg/mod/bigbluebuttonbn/view.php?id=70910";
                case "nemski": return "http://u4ili6teto.bg/1sou-tg/mod/bigbluebuttonbn/view.php?id=69291";
                case "svqt": return "http://u4ili6teto.bg/1sou-tg/mod/bigbluebuttonbn/view.php?id=70858";
                case "fvs": return "http://u4ili6teto.bg/1sou-tg/mod/bigbluebuttonbn/view.php?id=70683";
                default:
                    throw new ArgumentException("class not found");
            }

        }
    }
}
