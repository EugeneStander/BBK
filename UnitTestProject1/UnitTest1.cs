using System;
using System.Threading;
using System.Data;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace UnitTestProject1
{
    [TestFixture]
    public class UnitTest1 
    {
        //constants, to be changed if issues are found in delays
        private const string xpathNextButton = "//*[contains(@id, '_x_content')]/div[2]/div/div/button";
        private const int standardDelay = 750;
        private const int standardLoadTime = 2000;
        private const int standardTabSwitchTime = 2000;

        string caseNumber;
        IWebDriver driver;
        WebDriverWait driverWait;
        AccountOpeningSimplify AOSimplify;
        PortalSimplify PortalSimplify;
        DataHandler DataHandler;
        DataTable dataTable;
        string allocatedTo;

        [OneTimeSetUp]
        public void Init()
        {
            driver = new ChromeDriver("C:\\ChromeDriver");
            driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://jnb-dev-bbk:9014/portal");
            AOSimplify = new AccountOpeningSimplify(driver, driverWait);
            PortalSimplify = new PortalSimplify(driver, driverWait);
            DataHandler = new DataHandler("SybBBG");
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
        public void CreateCase()
        {
            SESLogin("Branch1_FrontOfficeMaker1", "Sybr!n123");
            Thread.Sleep(500);
            driver.FindElement(By.XPath("//div[@id='processFlowList']/ul/li")).Click();
            Thread.Sleep(100);
            driverWait.Until(ExpectedConditions.ElementIsVisible(By.XPath(PortalSimplify.PlayButton("1"))));
            Thread.Sleep(500);
            driver.FindElement(By.XPath(PortalSimplify.PlayButton("1"))).Click();
            driverWait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.XPath("//iframe[contains(@src, '/Custom/AccountOpening')]")));
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));
            Thread.Sleep(standardLoadTime);

            //STEP1: Get Case Number
            caseNumber = driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='- Front Office Maker Form'])[1]/following::p[1]")).Text;

            //STEP1: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP2: Set Customer Type
            AOSimplify.SetDropdownValue("Customer Type", "NEW");
            Thread.Sleep(standardDelay);

            //STEP2: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP3: Set Business Banking Segment
            AOSimplify.SetDropdownValue("Business Banking Segment", "SME PORTFOLIO");
            Thread.Sleep(standardDelay);

            //STEP3: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP4: Set Product Type
            AOSimplify.SetDropdownValue("Product Type", "CREDIT");
            Thread.Sleep(standardDelay);

            //STEP4: Set Customer Name
            driver.FindElement(By.XPath(AOSimplify.TextField("Customer Name"))).SendKeys("Customer Name");
            Thread.Sleep(standardDelay);

            //STEP4: Set Customer Signature Date
            driver.FindElement(By.XPath(AOSimplify.DateField("Customer Signature Date"))).Click();
            driver.FindElement(By.XPath(AOSimplify.DateField("Customer Signature Date"))).SendKeys("19990101");
            Thread.Sleep(standardDelay);

            //STEP4: Set Date of Incorporation
            driver.FindElement(By.XPath(AOSimplify.DateField("Date of Incorporation"))).Click();
            driver.FindElement(By.XPath(AOSimplify.DateField("Date of Incorporation"))).SendKeys("19990101");
            Thread.Sleep(standardDelay);

            //STEP4: Set Originating Branch
            AOSimplify.SetDropdownValue("Originating Branch", "12");
            Thread.Sleep(standardDelay);

            //STEP4: Set RM Code
            AOSimplify.SetDropdownValue("RM Code", "7060");
            Thread.Sleep(standardDelay);

            //STEP4: Set RM Name
            AOSimplify.SetDropdownValue("RM Name", "SETH");
            Thread.Sleep(standardDelay);

            //STEP4: Set BIC Code
            AOSimplify.SetDropdownValue("BIC Code", "9901");
            Thread.Sleep(standardDelay);

            //STEP4: Set Initial Scoring Results
            AOSimplify.SetDropdownValue("Initial Scoring Results", "LOW");
            Thread.Sleep(standardDelay);

            //STEP4: Set Domicile Branch
            AOSimplify.SetDropdownValue("Domicile Branch", "21");
            Thread.Sleep(standardDelay);

            //STEP4: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP5: Upload files
            AOSimplify.UploadAllFiles("C:\\Users\\EStander\\Desktop\\Things\\dummy.pdf", 1000);

            //STEP5: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));
            Thread.Sleep(standardLoadTime);

            //STEP6: Set Stakeholder Type
            AOSimplify.SetDropdownValue("Stakeholder Type", "Director");
            Thread.Sleep(standardDelay);

            //STEP6: Click on stakeholder documents tab
            driver.FindElement(By.XPath("//div[contains(@aria-controls, 'mat-tab-content')]/div[normalize-space(text())='Stakeholder Documents']")).Click();
            Thread.Sleep(standardTabSwitchTime);

            //STEP6: Upload files
            AOSimplify.UploadAllFiles("C:\\Users\\EStander\\Desktop\\Things\\dummy.pdf", 1000);

            //STEP6: Set all Director First Name
            AOSimplify.SetAllTextboxesOfType("Director First Name", "DirectorFirstName");

            //STEP6: Set all Director Last Name
            AOSimplify.SetAllTextboxesOfType("Director Last Name", "DirectorLastName");

            //STEP6: Submit case
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //Close queue
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[(text() = 'Close Screen' or . = 'Close Screen')]")));
            Thread.Sleep(standardDelay);
            driver.FindElement(By.XPath("//*[(text() = 'Close Screen' or . = 'Close Screen')]")).Click();
            Thread.Sleep(standardDelay);

            //Sign out
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(PortalSimplify.PlayButton("1"))));
            driver.FindElement(By.XPath("//*[@id='bs-example-navbar-collapse-1']/ul[3]/li[2]/a")).Click();
            driver.FindElement(By.XPath("//*[@id='bs-example-navbar-collapse-1']/ul[3]/li[2]/ul/li[6]/a")).Click();
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[text() = 'Sign In']")));
            Thread.Sleep(standardDelay);
            driver.FindElement(By.XPath("//button[text() = 'Sign In']")).Click();
        }

        [Test, Order(2)]
        public void ProcessCaseFOChecker()
        {
            dataTable = DataHandler.ExecuteQuery("SELECT FOCheckerAssignedTo FROM BBO_Case_Document WHERE CaseNumber = '" + caseNumber + "'");
            allocatedTo = dataTable.Rows[0][0].ToString();
            SESLogin(allocatedTo, "Sybr!n123");
            Thread.Sleep(500);
            driver.FindElement(By.XPath("//div[@id='processFlowList']/ul/li")).Click();
            Thread.Sleep(100);
            driverWait.Until(ExpectedConditions.ElementIsVisible(By.XPath(PortalSimplify.PlayButton("1"))));
            Thread.Sleep(standardLoadTime);
            driver.FindElement(By.XPath(PortalSimplify.CherryPicker("1"))).Click();
            Thread.Sleep(standardLoadTime);
            PortalSimplify.GoIntoCase("Case_Create_Date", caseNumber);
            driverWait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.XPath("//iframe[contains(@src, '/Custom/AccountOpening')]")));
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));
            Thread.Sleep(standardLoadTime);

            //STEP1: Check all checkboxes
            AOSimplify.CheckAllReviews();

            //STEP1: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP2: Check all checkboxes
            AOSimplify.CheckAllReviews();

            //STEP2: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP3: Check all checkboxes
            AOSimplify.CheckAllReviews();

            //STEP3: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP4: Advance to next step
            AOSimplify.ClickNextWithRetries(xpathNextButton, 10);

            //STEP5: Check all checkboxes
            AOSimplify.CheckAllReviews();

            //STEP5: Submit case
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //Close queue
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[(text() = 'Close Screen' or . = 'Close Screen')]")));
            Thread.Sleep(standardDelay);
            driver.FindElement(By.XPath("//*[(text() = 'Close Screen' or . = 'Close Screen')]")).Click();
            Thread.Sleep(standardDelay);

            //Sign out
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(PortalSimplify.PlayButton("1"))));
            driver.FindElement(By.XPath("//*[@id='bs-example-navbar-collapse-1']/ul[3]/li[2]/a")).Click();
            driver.FindElement(By.XPath("//*[@id='bs-example-navbar-collapse-1']/ul[3]/li[2]/ul/li[6]/a")).Click();
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[text() = 'Sign In']")));
            Thread.Sleep(standardDelay);
            driver.FindElement(By.XPath("//button[text() = 'Sign In']")).Click();
        }

        [Test, Order(3)]
        public void ProcessCaseCOMaker()
        {
            dataTable = DataHandler.ExecuteQuery("SELECT CentralMakerAssignedTo FROM BBO_Case_Document WHERE CaseNumber = '" + caseNumber + "'");
            allocatedTo = dataTable.Rows[0][0].ToString();
            SESLogin(allocatedTo, "Sybr!n123");
            Thread.Sleep(500);
            driver.FindElement(By.XPath("//div[@id='processFlowList']/ul/li")).Click();
            Thread.Sleep(100);
            driverWait.Until(ExpectedConditions.ElementIsVisible(By.XPath(PortalSimplify.PlayButton("1"))));
            Thread.Sleep(standardLoadTime);
            driver.FindElement(By.XPath(PortalSimplify.CherryPicker("1"))).Click();
            Thread.Sleep(standardLoadTime);
            PortalSimplify.GoIntoCase("Case_Create_Date", caseNumber);
            driverWait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.XPath("//iframe[contains(@src, '/Custom/AccountOpening')]")));
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));
            Thread.Sleep(standardLoadTime);

            //STEP1: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP2: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP3: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP4: Set CID/CIF
            driver.FindElement(By.XPath(AOSimplify.TextField("CID/CIF"))).SendKeys("54321");
            Thread.Sleep(standardDelay);

            //STEP4: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP5: Add product
            driver.FindElement(By.XPath(AOSimplify.StandardButton("Add Product"))).Click();
            Thread.Sleep(standardLoadTime);

            //STEP5: Set Product Name
            driver.FindElement(By.XPath(AOSimplify.TextField("Product Name"))).SendKeys("Product Name");
            Thread.Sleep(standardDelay);

            //STEP5: Set Account Type
            AOSimplify.SetDropdownValue("Account Type", "CREDIT");
            Thread.Sleep(standardDelay);

            //STEP5: Set Currency
            AOSimplify.SetDropdownValue("Currency", "SOUTH");
            Thread.Sleep(standardDelay);

            //STEP5: Set Account Number
            driver.FindElement(By.XPath(AOSimplify.TextField("Account Number"))).SendKeys("12345");
            Thread.Sleep(standardDelay);

            //STEP5: Set Channel Name
            driver.FindElement(By.XPath(AOSimplify.TextField("Channel Name"))).SendKeys("Channel Name");
            Thread.Sleep(standardDelay);

            //STEP5: Set Internal Branch Code
            driver.FindElement(By.XPath(AOSimplify.TextField("Internal Branch Code"))).SendKeys("Internal Branch");
            Thread.Sleep(standardDelay);

            //STEP5: Set External Branch Code
            driver.FindElement(By.XPath(AOSimplify.TextField("External Branch Code"))).SendKeys("External Branch");
            Thread.Sleep(standardDelay);

            //STEP5: Set SWIFT Code
            driver.FindElement(By.XPath(AOSimplify.TextField("SWIFT Code"))).SendKeys("SWIFT Code");
            Thread.Sleep(standardDelay);

            //STEP5: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP6: Advance to next step
            AOSimplify.ClickNextWithRetries(xpathNextButton, 10);

            //STEP7: Advance to next step
            AOSimplify.ClickNextWithRetries(xpathNextButton, 10);

            //STEP8: Set Date Application Signed By Customer
            driver.FindElement(By.XPath(AOSimplify.DateField("Date Application Signed By Customer"))).Click();
            driver.FindElement(By.XPath(AOSimplify.DateField("Date Application Signed By Customer"))).SendKeys("19990101");
            Thread.Sleep(standardDelay);

            //STEP8: Set Date Application Signed By Customer
            driver.FindElement(By.XPath(AOSimplify.DateField("Date Application Collected By RM"))).Click();
            driver.FindElement(By.XPath(AOSimplify.DateField("Date Application Collected By RM"))).SendKeys("19990101");
            Thread.Sleep(standardDelay);

            //STEP8: Click to let validation for the last date trigger
            driver.FindElement(By.XPath(AOSimplify.DateField("Date Application Signed By Customer"))).Click();
            Thread.Sleep(standardDelay);

            //STEP8: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP9: Generate Welcome Pack
            driver.FindElement(By.XPath(AOSimplify.StandardButton("Generate Welcome Pack"))).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[starts-with(@class, 'docViewerHolder__closeDocViewer docViewerHolder__closeDocViewer')]")));
            Thread.Sleep(standardDelay);
            driver.FindElement(By.XPath("//a[starts-with(@class, 'docViewerHolder__closeDocViewer docViewerHolder__closeDocViewer')]")).Click();

            //STEP9: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP10: Generate Config Report
            driver.FindElement(By.XPath(AOSimplify.StandardButton("Generate Config Report"))).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[starts-with(@class, 'docViewerHolder__closeDocViewer docViewerHolder__closeDocViewer')]")));
            Thread.Sleep(standardDelay);
            driver.FindElement(By.XPath("//a[starts-with(@class, 'docViewerHolder__closeDocViewer docViewerHolder__closeDocViewer')]")).Click();

            //STEP10: Submit case
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //Close queue
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[(text() = 'Close Screen' or . = 'Close Screen')]")));
            Thread.Sleep(standardDelay);
            driver.FindElement(By.XPath("//*[(text() = 'Close Screen' or . = 'Close Screen')]")).Click();
            Thread.Sleep(standardDelay);

            //Sign out
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(PortalSimplify.PlayButton("1"))));
            driver.FindElement(By.XPath("//*[@id='bs-example-navbar-collapse-1']/ul[3]/li[2]/a")).Click();
            driver.FindElement(By.XPath("//*[@id='bs-example-navbar-collapse-1']/ul[3]/li[2]/ul/li[6]/a")).Click();
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[text() = 'Sign In']")));
            Thread.Sleep(standardDelay);
            driver.FindElement(By.XPath("//button[text() = 'Sign In']")).Click();
        }

        [Test, Order(4)]
        public void ProcessCaseCOChecker()
        {
            dataTable = DataHandler.ExecuteQuery("SELECT CentralCheckerAssignedTo FROM BBO_Case_Document WHERE CaseNumber = '" + caseNumber + "'");
            allocatedTo = dataTable.Rows[0][0].ToString();
            SESLogin(allocatedTo, "Sybr!n123");
            Thread.Sleep(500);
            driver.FindElement(By.XPath("//div[@id='processFlowList']/ul/li")).Click();
            Thread.Sleep(100);
            driverWait.Until(ExpectedConditions.ElementIsVisible(By.XPath(PortalSimplify.PlayButton("1"))));
            Thread.Sleep(standardLoadTime);
            driver.FindElement(By.XPath(PortalSimplify.CherryPicker("1"))).Click();
            Thread.Sleep(standardLoadTime);
            PortalSimplify.GoIntoCase("Case_Create_Date", caseNumber);
            driverWait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.XPath("//iframe[contains(@src, '/Custom/AccountOpening')]")));
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));
            Thread.Sleep(standardLoadTime);

            //STEP1: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP2: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP3: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP4: Advance to next step
            AOSimplify.ClickNextWithRetries(xpathNextButton, 10);

            //STEP5: Advance to next step
            AOSimplify.ClickNextWithRetries(xpathNextButton, 10);

            //STEP6: Check all checkboxes
            AOSimplify.CheckAllReviews();

            //STEP6: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP7: Check all checkboxes
            AOSimplify.CheckAllReviews();

            //STEP7: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP8: Check all checkboxes
            AOSimplify.CheckAllReviews();

            //STEP8: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP9: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP10: Submit case
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //Close queue
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[(text() = 'Close Screen' or . = 'Close Screen')]")));
            Thread.Sleep(standardDelay);
            driver.FindElement(By.XPath("//*[(text() = 'Close Screen' or . = 'Close Screen')]")).Click();
            Thread.Sleep(standardDelay);

            //Sign out
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(PortalSimplify.PlayButton("1"))));
            driver.FindElement(By.XPath("//*[@id='bs-example-navbar-collapse-1']/ul[3]/li[2]/a")).Click();
            driver.FindElement(By.XPath("//*[@id='bs-example-navbar-collapse-1']/ul[3]/li[2]/ul/li[6]/a")).Click();
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[text() = 'Sign In']")));
            Thread.Sleep(standardDelay);
            driver.FindElement(By.XPath("//button[text() = 'Sign In']")).Click();
        }

        [Test, Order(5)]
        public void ProcessCaseFOComplete()
        {
            dataTable = DataHandler.ExecuteQuery("SELECT FOMakerAssignedTo FROM BBO_Case_Document WHERE CaseNumber = '" + caseNumber + "'");
            allocatedTo = dataTable.Rows[0][0].ToString();
            SESLogin(allocatedTo, "Sybr!n123");
            Thread.Sleep(500);
            driver.FindElement(By.XPath("//div[@id='processFlowList']/ul/li")).Click();
            Thread.Sleep(100);
            driverWait.Until(ExpectedConditions.ElementIsVisible(By.XPath(PortalSimplify.PlayButton("1"))));
            Thread.Sleep(standardLoadTime);
            driver.FindElement(By.XPath(PortalSimplify.CherryPicker("5"))).Click();
            Thread.Sleep(standardLoadTime);
            PortalSimplify.GoIntoCase("Case_Create_Date", caseNumber);
            driverWait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.XPath("//iframe[contains(@src, '/Custom/AccountOpening')]")));
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));
            Thread.Sleep(standardLoadTime);

            //STEP1: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP2: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP3: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP4: Advance to next step
            AOSimplify.ClickNextWithRetries(xpathNextButton, 10);

            //STEP5: Advance to next step
            AOSimplify.ClickNextWithRetries(xpathNextButton, 10);

            //STEP6: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP7: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP8: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP9: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP10: Advance to next step
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //STEP11: Submit case
            driver.FindElement(By.XPath(xpathNextButton)).Click();
            Thread.Sleep(standardLoadTime);
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpathNextButton)));

            //Close queue
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[(text() = 'Close Screen' or . = 'Close Screen')]")));
            Thread.Sleep(standardDelay);
            driver.FindElement(By.XPath("//*[(text() = 'Close Screen' or . = 'Close Screen')]")).Click();
            Thread.Sleep(standardDelay);

            //Sign out
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(PortalSimplify.PlayButton("1"))));
            driver.FindElement(By.XPath("//*[@id='bs-example-navbar-collapse-1']/ul[3]/li[2]/a")).Click();
            driver.FindElement(By.XPath("//*[@id='bs-example-navbar-collapse-1']/ul[3]/li[2]/ul/li[6]/a")).Click();
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[text() = 'Sign In']")));
            Thread.Sleep(standardDelay);
            driver.FindElement(By.XPath("//button[text() = 'Sign In']")).Click();
        }
    }
}