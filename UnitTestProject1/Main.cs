using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace MainSpace
{
    [TestFixture]
    public class UnitTest1 
    {
        //constants, to be changed if issues are found in delays
        private const string xpathNextButton = "//*[contains(@id, '_x_content')]/div[2]/div/div/button";
        private const int standardDelay = 750;
        private const int standardLoadTime = 2000;
        private const int standardTabSwitchTime = 2000;
        
        IWebDriver driver;
        WebDriverWait driverWait;
        AccountOpeningSimplify AOSimplify;
        PortalSimplify PortalSimplify;

        [OneTimeSetUp]
        public void Init()
        {
            driver = new ChromeDriver("C:\\ChromeDriver");
            driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("OMITTED");
            AOSimplify = new AccountOpeningSimplify(driver, driverWait);
            PortalSimplify = new PortalSimplify(driver, driverWait);
        }

        [OneTimeTearDown]
        public void Close()
        {
            driver.Close();
        }

        public void SESLogin(string username, string password)
        {
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"usernameBox\"]")));
            driver.FindElement(By.XPath("//*[@id=\"usernameBox\"]")).SendKeys(username);
            driver.FindElement(By.XPath("//*[@id=\"passwordBox\"]")).SendKeys(password);
            driver.FindElement(By.XPath("//button[@id='btn_logon']")).Click();
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@id='processFlowList']/ul/li")));
        }

        [Test, Order(1)]
        public void Loop()
        {
            SESLogin("estander", "");
            Thread.Sleep(500);
            driver.FindElement(By.XPath(PortalSimplify.ProcessFlow("8"))).Click();
            Thread.Sleep(100);
            driverWait.Until(ExpectedConditions.ElementIsVisible(By.XPath(PortalSimplify.PlayButton("1"))));
            Thread.Sleep(500);
            while (true)
            {
                CreateCase();
                Thread.Sleep(120000);
            }
        }
        public void CreateCase()
        {
            driver.FindElement(By.XPath(PortalSimplify.PlayButton("1"))).Click();
            driverWait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.XPath("//iframe[contains(@src, '/Custom/AccountOpening')]")));
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));
            Thread.Sleep(standardLoadTime);

            //STEP1: Set Card Application Type
            AOSimplify.SetDropdownValue("Card Application type", "CANCEL");
            Thread.Sleep(standardDelay);

            //STEP1: Set Account Lookup Method
            AOSimplify.SetDropdownValue("Account Lookup Method", "CARD NUMBER");
            Thread.Sleep(standardDelay);

            //STEP1: Set Account Lookup Value
            driver.FindElement(By.XPath(AOSimplify.TextField("Account Lookup Value"))).SendKeys("4373930100924270");
            Thread.Sleep(standardDelay);

            //STEP1: Advance to next step
            AOSimplify.ClickNextButton(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP2: Advance to next step
            AOSimplify.ClickNextButton(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP3: Advance to next step
            AOSimplify.ClickNextButton(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP4: Set Cancellation Reason
            AOSimplify.SetDropdownValue("Cancellation Reason", "LOST");
            Thread.Sleep(standardDelay);

            //STEP4: Advance to next step
            AOSimplify.ClickNextButton(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP5: Advance to next step
            AOSimplify.ClickNextButton(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP5: Generate Welcome Pack
            driver.FindElement(By.XPath(AOSimplify.StandardButton("Generate Signature Form"))).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[starts-with(@class, 'docViewerHolder__closeDocViewer docViewerHolder__closeDocViewer')]")));
            Thread.Sleep(standardDelay);
            driver.FindElement(By.XPath("//a[starts-with(@class, 'docViewerHolder__closeDocViewer docViewerHolder__closeDocViewer')]")).Click();
            
            //STEP4: Set Is The Document Signed?
            AOSimplify.SetDropdownValue("Is The Document Signed?", "YES");
            Thread.Sleep(standardDelay);

            //STEP5: Upload files
            AOSimplify.UploadAllFiles("C:\\Users\\EStander\\Desktop\\Things\\dummy.pdf", 1000);

            //STEP6: Submit case
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //Close queue
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[(text() = 'Close Screen' or . = 'Close Screen')]")));
            Thread.Sleep(standardDelay);
            driver.FindElement(By.XPath("//*[(text() = 'Close Screen' or . = 'Close Screen')]")).Click();
            Thread.Sleep(standardDelay);
        }
    }
}