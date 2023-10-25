using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using Keys = OpenQA.Selenium.Keys;

namespace UnitTestProject1
{
    public class AccountOpeningSimplify
    {
        IWebDriver driver;
        WebDriverWait driverWait;
        public AccountOpeningSimplify()
        {

        }
        public AccountOpeningSimplify(IWebDriver inputDriver, WebDriverWait inputDriverWait)
        {
            driver = inputDriver;
            driverWait = inputDriverWait;
        }
        public void SetDropdownValue(string fieldName, string value)
        {
            try
            {
                driver.FindElement(By.XPath("//*[starts-with(@id, 'select2-" + fieldName + "-') and substring(@id, string-length(@id) - string-length('-container') + 1) = '-container']")).Click();
            }
            catch (ElementClickInterceptedException)
            {
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//*[starts-with(@id, 'select2-" + fieldName + "-') and substring(@id, string-length(@id) - string-length('-container') + 1) = '-container']")).Click();
            }

            driver.FindElement(By.XPath("//input[@type='search']")).SendKeys(value + Keys.Enter);
        }
        public string TextField(string fieldName)
        {
            return "//*[@name = '" + fieldName + "']";
        }
        public string DateField(string fieldName)
        {
            return "//div[./label/span[contains(text(), '" + fieldName + "')]]/span/span/input";
        }
        public string StandardButton(string buttonName)
        {
            return "//button[normalize-space(text())='" + buttonName + "']";
        }
        public void UploadAllFiles(string filePath, int delay)
        {
            Actions actions = new Actions(driver);
            IReadOnlyCollection<IWebElement> listFileUploads = driver.FindElements(By.XPath("//label[starts-with(@for, 'file_')]"));
            foreach (IWebElement fileUpload in listFileUploads)
            {
                try
                {
                    fileUpload.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    Thread.Sleep(4000);
                    fileUpload.Click();
                }
                Thread.Sleep(delay);
                SendKeys.SendWait(filePath);
                SendKeys.SendWait(@"{Enter}");
                Thread.Sleep(delay);
            }
        }
        public void CheckAllReviews()
        {
            IReadOnlyCollection<IWebElement> listReviewCheckboxes = driver.FindElements(By.XPath("//input[substring(@id, string-length(@id) - string-length('_isReviewed') +1) = '_isReviewed']/../label"));
            foreach (IWebElement reviewCheckbox in listReviewCheckboxes)
            {
                reviewCheckbox.Click();
            }
        }
        public void SetAllTextboxesOfType(string fieldName, string textToSet)
        {
            IReadOnlyCollection<IWebElement> listTextboxes = driver.FindElements(By.XPath(TextField(fieldName)));
            foreach (IWebElement textbox in listTextboxes)
            {
                textbox.SendKeys(textToSet);
            }
        }
        public void ClickNextWithRetries(string xpathNextButton, int retrySeconds)
        {
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            for (int i = 0; i <= retrySeconds; i++)
            {
                Thread.Sleep(1000);
                if (IsElementPresent(By.XPath("//*[@id='toast-container']")))
                {
                    driver.FindElement(By.XPath(xpathNextButton)).Click();
                }
                else
                {
                    break;
                }
            }
        }
        public bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
