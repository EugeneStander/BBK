using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Data;
using System.Threading;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace MainSpace
{
    class PortalSimplify
    {
        IWebDriver driver;
        WebDriverWait driverWait;
        public PortalSimplify()
        {

        }
        public PortalSimplify(IWebDriver inputDriver, WebDriverWait inputDriverWait)
        {
            driver = inputDriver;
            driverWait = inputDriverWait;
        }
        public string ProcessFlow(string flowNumber)
        {
            return "//div[@id='processFlowList']/ul/li[" + flowNumber + "]";
        }
        public string PlayButton(string queueNumber)
        {
            return "//sybrin-basic-card[" + queueNumber + "]/div/div/div/div/div/div/div[2]/div/button/i[@class='fa fa-play']";
        }
        public string CherryPicker(string queueNumber)
        {
            return "//sybrin-basic-card[" + queueNumber + "]/div/div/div/div/div/div/div[2]/div[@class='btn-action-container']/button/i[@class='fa fa-search-plus']";
        }
        public string ColumnHeader(string headerName)
        {
            return "//sync-fusion-grid/div[2]/ej-grid/div[2]/div/table/thead/tr/th/div/span[contains(text(), '" + headerName + "')]";
        }
        public string CellValue(string columnHeader, string rowNumber)
        {
            return "//sync-fusion-grid/div[2]/ej-grid/div[3]/div/table/tbody/tr[" + rowNumber + "]/td[substring(@aria-label, string-length(@aria-label) - string-length('" + columnHeader + "') +1) = '" + columnHeader + "']";
        }
        public void WaitForLoadingCases()
        {
            driverWait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//sync-fusion-grid/div[2]/ej-grid/div[3]/div/table/tbody/tr[1]")));
            driverWait.Until((d) =>
            {
                IWebElement element = d.FindElement(By.XPath("//sync-fusion-grid/div[2]/ej-grid/div[3]/div"));
                if (element.GetAttribute("aria-busy") == "false")
                {
                    return element;
                }

                return null;
            });
        }
        public void GoIntoCase(string orderBy, string caseNumber)
        {
            WaitForLoadingCases();
            Actions actions = new Actions(driver);
            actions.MoveToElement(driver.FindElement(By.XPath(ColumnHeader(orderBy))));
            actions.Perform();
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(ColumnHeader(orderBy))));
            driver.FindElement(By.XPath(ColumnHeader(orderBy))).Click();
            WaitForLoadingCases();
            driver.FindElement(By.XPath(ColumnHeader(orderBy))).Click();
            WaitForLoadingCases();
            for (int i = 1; i <= 10; i++)
            {
                if (caseNumber == driver.FindElement(By.XPath(CellValue("Case Number", i.ToString()))).Text)
                {
                    new Actions(driver).DoubleClick(driver.FindElement(By.XPath(CellValue("Case Number", i.ToString())))).Perform();
                    break;
                }
            }
        }
        public void WaitForCaseCreation(string caseNumber, int delay, DataHandler dataHandler)
        {
            DataTable dataTable = dataHandler.ExecuteQuery("Select * From Credit_Vetting_Case Where CaseNumber = '" + caseNumber + "'");
            driverWait.Until((d) =>
            {
                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    Thread.Sleep(delay);
                    dataTable = dataHandler.ExecuteQuery("Select * From Credit_Vetting_Case Where CaseNumber = '" + caseNumber + "'");
                    return null;
                }
                return dataTable;
            });
        }
    }
}