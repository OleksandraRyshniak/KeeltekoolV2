using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using System.Threading;
using Xunit.Sdk;
using Xunit.Abstractions;

namespace Keeltekool_2.SeleniumTesting
{
    public class OrderAttribute: Attribute
    {
        public int Value { get; } //värtus
        public OrderAttribute(int value) //omistuskonstruktor
        {
            Value = value;
        }
    }

    public class PriorityOrderer : ITestCaseOrderer
    {
        //method võtab sisse kõik test cased
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            //toimub sorteerimistegevus kus järjestatakse arendaja kirjutatud järjesti järgi.
            var result = testCases.OrderBy(test =>
            {
                //otsitakse järjesti omadusena
                var method = test.TestMethod.Method.GetCustomAttributes(typeof(OrderAttribute)).FirstOrDefault();
                //kui ei leita
                if (method == null)
                {
                    //antakse tagasi maksimaalne täisarv, ehk test teostatakse kohe, ega ei panda teiste potensiaalselt ÕIGES
                    //järjekorras ovelate testide vahele
                    return int.MaxValue;
                }
                //kui leitakse
                else
                {
                    return method.GetNamedArgument<int>(nameof(OrderAttribute.Value));
                }
            }).ToList();
            return result;
        }
    }
    [TestCaseOrderer("Keeltekool_2.SeleniumTesting.PriorityOrderer", "Keeltekool_2.SeleniumTesting")]
    public class LanguageCourseFrontendTests
    {
        [Fact]
        [Order(1)]
        public void Should_NavigateToIndex_AddLanguageWithCorrectData_ReturnToIndex()
        {
            //Ülesseade
            IWebDriver driver = TestSetup_NavigateToIndex();

            //tegevus
            IWebElement indexCreateButton = driver.FindElement(By.Id("IndexCreateNewLanguageCourse_BTN"));
            indexCreateButton.Click();
            DataEntry_LanguageCourse(driver);
            IWebElement CreateCreateButton = driver.FindElement(By.Id("CU_Create_BTN"));
            CreateCreateButton.Click();

            //kontroll
            //kogume andmed
            var nimetused = driver.FindElements(By.Id("IndexTableCourseNimetus"));
            var keeled = driver.FindElements(By.Id("IndexTableCourseKeel"));
            string LanguageCourseNameData = nimetused[nimetused.Count - 1].Text;
            string LanguageCourseKeelData = keeled[keeled.Count - 1].Text;
            // string LanguageCourseTaseData = driver.FindElement(By.Id("IndexTableCourseTase")).Text;
            Thread.Sleep(3000);
            driver.Quit();
            //assertime
            Assert.Equal("Automaattestimise kursus", LanguageCourseNameData);
            Assert.Equal("C# + Selenium & xUnit", LanguageCourseKeelData);
            Assert.True(LanguageCourseNameData == "Automaattestimise kursus" && LanguageCourseKeelData == "C# + Selenium & xUnit");
        }
        //siia vaja details (READ) test
        [Fact]
        [Order(2)]
        public void Should_NavigateToDetailsOfLanguageCourse_WithPreviouslyCorrectData()
        {
            //ülesseade
            IWebDriver driver = TestSetup_NavigateToIndex();
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            bool isObjectFound = false;
            string details_id = "";
            string details_name = "";
            string details_keel = "";
            string details_kirjeldus = "";
            string details_tase = "";

            //tegevus
            foreach (var row in rowsOfTable)
            {
                var nameElement = row.FindElement(By.Id("IndexTableCourseNimetus")).Text;
                var keelElement = row.FindElement(By.Id("IndexTableCourseKeel")).Text;
                if((nameElement == "Automaattestimise kursus") && (keelElement=="C# + Selenium & xUnit"))
                {
                    isObjectFound = true;
                    var clickAction = row
                        .FindElement(By.Id("IndexTableCourseActions"))
                        .FindElement(By.Id("IndexTableCourseDetails_BTN"));
                    clickAction.Click();
                    break;
                }
            }

            // Ainult siis, kui leidsime kursuse, püüame laadida detaile
            if (isObjectFound)
            {
                Thread.Sleep(500);
                //find data
                details_id = driver.FindElement(By.Id("DD_LanguageCourseGuid")).Text;
                details_name = driver.FindElement(By.Id("DD_LanguageCourseNimetus")).Text;
                details_keel = driver.FindElement(By.Id("DD_LanguageCourseKeel")).Text;
                details_kirjeldus = driver.FindElement(By.Id("DD_LanguageCourseKirjeldus")).Text;
                details_tase = driver.FindElement(By.Id("DD_LanguageCourseTase")).Text;
                IWebElement goBack = driver.FindElement(By.Id("DetailsBack_BTN"));
            }

            driver.Quit();

            //kontroll
            Assert.True(isObjectFound, "Kursust 'Automaattestimise kursus' ei leitud. Kontrolli, et test andmed on loodud.");
            Assert.NotEmpty(details_id);
            Assert.NotEmpty(details_name);
            Assert.NotEmpty(details_keel);
            Assert.NotEmpty(details_kirjeldus);
            Assert.NotEmpty(details_tase);
            Assert.Equal("Automaattestimise kursus", details_name);
            Assert.Equal("C# + Selenium & xUnit", details_keel);
        }
        //vaja teha uuenduse test (UPDATE)
        [Fact]
        [Order(3)]
        public void Should_UpdateDetailsOfLanguageCourse_WithNewData()
        {
            /* Ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            bool isObjectFound = false;

            /* tegevus */
            foreach (var row in rowsOfTable)
            {
                var nameElement = row.FindElement(By.Id("IndexTableCourseNimetus")).Text;
                var keelElement = row.FindElement(By.Id("IndexTableCourseKeel")).Text;
                if ((nameElement == "Automaattestimise kursus") && (keelElement == "C# + Selenium & xUnit"))
                {
                    isObjectFound = true;
                   
                    row.FindElement(By.Id("IndexTableCourseActions"))
                       .FindElement(By.Id("IndexTableCourseUpdate_BTN"))
                       .Click();
                    break;
                }
            }
            Assert.True(isObjectFound, "Kursust 'Automaattestimise kursus' ei leitud.");

            DataClear_CreateUpdate(driver);
            DataEntry_LanguageCourse(driver, true);
            driver.FindElement(By.Id("CU_Update_BTN")).Click();

            driver.FindElement(By.LinkText("Keelekursused")).Click();

            /* kontrolle */
           
            tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            isObjectFound = false;
            foreach (var row in rowsOfTable)
            {
                var nameElement = row.FindElement(By.Id("IndexTableCourseNimetus")).Text;
                var keelElement = row.FindElement(By.Id("IndexTableCourseKeel")).Text;
                if ((nameElement == "Manuaaltestimise kursus") && (keelElement == "Mantis, Excel"))
                {
                    isObjectFound = true;
                    row.FindElement(By.Id("IndexTableCourseActions"))
                       .FindElement(By.Id("IndexTableCourseDetails_BTN"))
                       .Click();
                    break;
                }
            }
            Assert.True(isObjectFound, "Uuendatud kursust ei leitud.");

            string details_id = driver.FindElement(By.Id("DD_LanguageCourseGuid")).Text;
            string details_name = driver.FindElement(By.Id("DD_LanguageCourseNimetus")).Text;
            string details_keel = driver.FindElement(By.Id("DD_LanguageCourseKeel")).Text;
            string details_kirjeldus = driver.FindElement(By.Id("DD_LanguageCourseKirjeldus")).Text;
            string details_tase = driver.FindElement(By.Id("DD_LanguageCourseTase")).Text;
            driver.Quit();

            Assert.NotEmpty(details_id);
            Assert.Equal("Manuaaltestimise kursus", details_name);
            Assert.Equal("Mantis, Excel", details_keel);
            Assert.Equal("2", details_tase);
            Assert.Equal("Kursusel õpime mis on manuaaltestimine.", details_kirjeldus);
        }

        private void DataClear_CreateUpdate(IWebDriver driver)
        {
            IWebElement createEntryNimetus = driver.FindElement(By.Id("CU_LanguageCourse_Nimetus"));
            createEntryNimetus.Clear();
            IWebElement createEntryKeel = driver.FindElement(By.Id("CU_LanguageCourse_Keel"));
            createEntryKeel.Clear();
            IWebElement createEntryTase = driver.FindElement(By.Id("CU_LanguageCourse_Tase"));
            createEntryTase.Clear();
            IWebElement createEntryKirjeldus = driver.FindElement(By.Id("CU_LanguageCourse_Kirjeldus"));
            createEntryKirjeldus.Clear();
        }

        //Delete
        [Fact]
        [Order(4)]
        public void Should_DeleteLanguageCourse_FromDeleteView()
        {
            /* Ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            bool isObjectFound = false;

            /* tegevus */
            foreach (var row in rowsOfTable)
            {
                var nameElement = row.FindElement(By.Id("IndexTableCourseNimetus")).Text;
                var keelElement = row.FindElement(By.Id("IndexTableCourseKeel")).Text;
                if ((nameElement == "Manuualtestimise kursus") && (keelElement == "Mantis, Excel"))
                {
                    isObjectFound = true;
                    var clickAction = row;
                    row.FindElement(By.Id("IndexTableCourseActions"))
                       .FindElement(By.Id("IndexTableCourseDelete_BTN"))
                       .Click();
                    break;
                }
            }
            Thread.Sleep(1000);
            IWebElement clickDelete = driver.FindElement(By.Id("DeleteDelete_BTN"));
            clickDelete.Click();

            //kontroll
            isObjectFound = false;
            tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            foreach (var row in rowsOfTable)
            {
                var nameElement = row.FindElement(By.Id("IndexTableCourseNimetus")).Text;
                var keelElement = row.FindElement(By.Id("IndexTableCourseKeel")).Text;
                if ((nameElement == "Manuualtestimise kursus") && (keelElement == "Mantis, Excel"))
                {
                    isObjectFound = true;
                    break;
                }
            }
            Assert.False(isObjectFound);
            Assert.True(!isObjectFound);
        }


        public void DataEntry_LanguageCourse(IWebDriver driver, bool isOneOrTwo = false)
        {
            if (!isOneOrTwo)
            {
                IWebElement createEntryNimetus = driver.FindElement(By.Id("CU_LanguageCourse_Nimetus"));
                createEntryNimetus.SendKeys("Automaattestimise kursus");
                IWebElement createEntryKeel = driver.FindElement(By.Id("CU_LanguageCourse_Keel"));
                createEntryKeel.SendKeys("C# + Selenium & xUnit");
                IWebElement createEntryTase = driver.FindElement(By.Id("CU_LanguageCourse_Tase"));
                createEntryTase.SendKeys("3");
                IWebElement createEntryKirjeldus = driver.FindElement(By.Id("CU_LanguageCourse_Kirjeldus"));
                createEntryKirjeldus.SendKeys("Ksusel õpime mis on automaattestimine, kuidas erineb manuaaltestimisest ning kuidas kirjutada backendi ja frontendi automaatteste.");
            }
            else
            {
                IWebElement createEntryNimetus = driver.FindElement(By.Id("CU_LanguageCourse_Nimetus"));
                createEntryNimetus.SendKeys("Manuaaltestimise kursus");
                IWebElement createEntryKeel = driver.FindElement(By.Id("CU_LanguageCourse_Keel"));
                createEntryKeel.SendKeys("Mantis, Excel");
                IWebElement createEntryTase = driver.FindElement(By.Id("CU_LanguageCourse_Tase"));
                createEntryTase.SendKeys("2");
                IWebElement createEntryKirjeldus = driver.FindElement(By.Id("CU_LanguageCourse_Kirjeldus"));
                createEntryKirjeldus.SendKeys("Kursusel õpime mis on manuaaltestimine.");
            }
        }

        private static IWebDriver TestSetup_NavigateToIndex()
        {
            var options = new ChromeOptions { AcceptInsecureCertificates = true };
            IWebDriver driver = new ChromeDriver(options); //(ChromeDriver) brauserit juhtiv ja seal tegutsev draiver
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Url = "https://localhost:7153/"; //navigieeritav aadress
            IWebElement navigateToLanguageCourse = driver.FindElement(By.LinkText("Keelekursused"));//otsime tesitava osa lingi
            navigateToLanguageCourse.Click();//simuleeritud tegevus elemendiga, antud juhul klikk
            return driver;
        }
    }
}