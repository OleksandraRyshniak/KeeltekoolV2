using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Globalization;

namespace Keeltekool_2.SeleniumTesting
{
    [TestCaseOrderer("Keeltekool_2.SeleniumTesting.PriorityOrderer", "Keeltekool_2.SeleniumTesting")]
    public class StudentFrontendTests
    {
        public static string thisRunGuid { get; set; }

        /// <summary>
        /// Test loob uue õpilase, kelle eesnimes on unikaalne Guid.
        /// Kontrollib, et pärast salvestamist suunatakse tagasi nimekirja ning seal on
        /// õpilane, kelle eesnimi, perekonnanimi ja kasutaja ID vastavad sisestatud andmetele.
        /// </summary>
        [Fact]
        [Order(1)]
        public void Should_NavigateToIndex_AddStudentWithUniqueData_ReturnToIndex()
        {
            /* Ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();

            /* tegevus */
            IWebElement indexCreateButton = driver.FindElement(By.Id("IndexCreateNewStudent_BTN"));
            indexCreateButton.Click(); //vajutame loomise nupule

            string uniqueId = DataEntry_Student(driver); //sisestame andmed vormi
            thisRunGuid = uniqueId;

            IWebElement CreateCreateButton = driver.FindElement(By.Id("CU_Create_BTN"));
            CreateCreateButton.Click(); //vajutame loomise vaates salvesta nupule

            /* kontroll */

            //kogume andmed
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0); //eemaldame päise rea
            bool isObjectFound = false;
            string StudentFirstNameData = "";
            string StudentLastNameData = "";
            string StudentUserIdData = "";
            foreach (var row in rowsOfTable)
            {
                if (row.FindElement(By.Id("IndexTableStudentFirstName")).Text == thisRunGuid)
                {
                    isObjectFound = true;
                    StudentFirstNameData = thisRunGuid;
                    StudentLastNameData = row.FindElement(By.Id("IndexTableStudentLastName")).Text;
                    StudentUserIdData = row.FindElement(By.Id("IndexTableStudentUserID")).Text;
                }
            }

            //sulgeme akna
            Thread.Sleep(500);
            driver.Quit();
            //assertime
            Assert.True(isObjectFound);
            Assert.Equal(uniqueId, StudentFirstNameData);
            Assert.Equal("Automaattest", StudentLastNameData);
            Assert.Equal("selenium-test-01", StudentUserIdData);
        }

        /// <summary>
        /// Test avab nimekirjast eelmises testis loodud õpilase detailvaate ("Vaata").
        /// Kontrollib, et detailvaates on täpselt need andmed, mis loomisel sisestati,
        /// et loomise ja muutmise kuupäev on kujul pp.kk.aaaa tt:mm
        /// ning et "Tagasi nimekirja" nupp viib tagasi õpilaste nimekirja.
        /// </summary>
        [Fact]
        [Order(2)]
        public void Should_NavigateToDetailsOfAStudent_WithPreviouslyUniqueData()
        {
            /* ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            bool isObjectFound = false;

            /* tegevus */
            foreach (var row in rowsOfTable)
            {
                var firstNameElement = row.FindElement(By.Id("IndexTableStudentFirstName")).Text;
                if (firstNameElement == thisRunGuid)
                {
                    isObjectFound = true;
                    var clickAction = row
                        .FindElement(By.Id("IndexTableStudentActions"))
                        .FindElement(By.Id("IndexTableStudentDetails_BTN"));
                    clickAction.Click();
                    break;
                }
            }
            Thread.Sleep(1000);
            //leiame andmed
            string details_firstName = driver.FindElement(By.Id("DD_StudentFirstName")).Text; //unikaalne Guid
            string details_lastName = driver.FindElement(By.Id("DD_StudentLastName")).Text;
            string details_userId = driver.FindElement(By.Id("DD_StudentUserID")).Text;
            string details_createdAt = driver.FindElement(By.Id("DD_StudentCreatedAt")).Text;
            string details_modifiedAt = driver.FindElement(By.Id("DD_StudentModifiedAt")).Text;
            IWebElement goBack = driver.FindElement(By.Id("DetailsBack_BTN"));
            goBack.Click();
            Thread.Sleep(1000);
            bool isBackOnIndex = driver.FindElements(By.Id("IndexTable")).Count == 1; //kas oleme tagasi nimekirjas
            driver.Quit();

            /* Kontroll */
            Assert.True(isObjectFound);
            Assert.NotEmpty(details_firstName);
            Assert.NotEmpty(details_lastName);
            Assert.NotEmpty(details_userId);
            Assert.Equal(thisRunGuid, details_firstName);
            Assert.Equal("Automaattest", details_lastName);
            Assert.Equal("selenium-test-01", details_userId);
            //kuupäevad peavad olema kujul pp.kk.aaaa tt:mm
            Assert.True(DateTime.TryParseExact(details_createdAt, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
            Assert.True(DateTime.TryParseExact(details_modifiedAt, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
            Assert.True(isBackOnIndex);
        }

        /// <summary>
        /// Test muudab eelnevalt loodud õpilase perekonnanime ja kasutaja ID-d (eesnimi ehk Guid jääb samaks).
        /// Kontrollib, et nimekirjas ja detailvaates on uued andmed, vanu andmeid enam ei ole
        /// ning et muutmise kuupäev ei ole loomise kuupäevast varasem.
        /// </summary>
        [Fact]
        [Order(3)]
        public void Should_UpdateDetailsOfUniqueStudent_WithNewData()
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
                var firstNameElement = row.FindElement(By.Id("IndexTableStudentFirstName")).Text;
                if (firstNameElement == thisRunGuid)
                {
                    isObjectFound = true;
                    var clickAction = row
                        .FindElement(By.Id("IndexTableStudentActions"))
                        .FindElement(By.Id("IndexTableStudentUpdate_BTN"));
                    clickAction.Click();
                    break;
                }
            }
            Thread.Sleep(500);
            DataClear_CreateUpdate_SaveName(driver); //puhastame väljad, eesnimi (Guid) jääb alles
            DataEntry_Student(driver, true, false); //sisestame andmekomplekti 2
            IWebElement updateClick = driver.FindElement(By.Id("CU_Update_BTN"));
            updateClick.Click(); //salvestamise järel suunatakse tagasi nimekirja

            /* kontroll */
            //otsime muudetud objekti nimekirjast üles
            Thread.Sleep(1000);
            tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            bool isUpdatedObjectFound = false;
            string index_lastName = "";
            string index_userId = "";
            foreach (var row in rowsOfTable)
            {
                var firstNameElement = row.FindElement(By.Id("IndexTableStudentFirstName")).Text;
                if (firstNameElement == thisRunGuid)
                {
                    isUpdatedObjectFound = true;
                    index_lastName = row.FindElement(By.Id("IndexTableStudentLastName")).Text;
                    index_userId = row.FindElement(By.Id("IndexTableStudentUserID")).Text;
                    var clickAction = row
                        .FindElement(By.Id("IndexTableStudentActions"))
                        .FindElement(By.Id("IndexTableStudentDetails_BTN"));
                    clickAction.Click();
                    break;
                }
            }
            Thread.Sleep(1000);
            //leiame andmed detailvaatest
            string details_firstName = driver.FindElement(By.Id("DD_StudentFirstName")).Text; //unikaalne Guid
            string details_lastName = driver.FindElement(By.Id("DD_StudentLastName")).Text;
            string details_userId = driver.FindElement(By.Id("DD_StudentUserID")).Text;
            string details_createdAt = driver.FindElement(By.Id("DD_StudentCreatedAt")).Text;
            string details_modifiedAt = driver.FindElement(By.Id("DD_StudentModifiedAt")).Text;
            Thread.Sleep(1000);
            driver.Quit();

            Assert.True(isObjectFound);
            Assert.True(isUpdatedObjectFound);
            //nimekirjas on uued andmed
            Assert.Equal("Aavik", index_lastName);
            Assert.Equal("selenium-test-02", index_userId);
            //detailvaates on uued andmed
            Assert.Equal(thisRunGuid, details_firstName);
            Assert.Equal("Aavik", details_lastName);
            Assert.Equal("selenium-test-02", details_userId);
            //vanu andmeid enam ei ole
            Assert.NotEqual("Automaattest", details_lastName);
            Assert.NotEqual("selenium-test-01", details_userId);
            //muutmise kuupäev ei ole loomise kuupäevast varasem
            DateTime createdAt = DateTime.ParseExact(details_createdAt, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
            DateTime modifiedAt = DateTime.ParseExact(details_modifiedAt, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
            Assert.True(modifiedAt >= createdAt);
        }

        //eof

        ///<summary>
        ///Puhastab õpilase vormi kõik andmeväljad meetodile kaasaantud draiveri sessioonis
        ///</summary>
        ///<param name="driver">Hetkel käsitletav draiver</param>
        private void DataClear_CreateUpdate(IWebDriver driver)
        {
            IWebElement entryFirstName = driver.FindElement(By.Id("CU_Student_FirstName"));
            entryFirstName.Clear();
            IWebElement entryLastName = driver.FindElement(By.Id("CU_Student_LastName"));
            entryLastName.Clear();
            IWebElement entryUserId = driver.FindElement(By.Id("CU_Student_UserID"));
            entryUserId.Clear();
        }

        ///<summary>
        ///Puhastab õpilase vormi andmeväljad meetodile kaasaantud draiveri sessioonis
        ///välja arvatud eesnime väli (seal hoitakse testi unikaalset Guidi)
        ///</summary>
        ///<param name="=driver">Hetkel käsitletav draiver</param>
        private void DataClear_CreateUpdate_SaveName(IWebDriver driver)
        {
            IWebElement entryLastName = driver.FindElement(By.Id("CU_Student_LastName"));
            entryLastName.Clear();
            IWebElement entryUserId = driver.FindElement(By.Id("CU_Student_UserID"));
            entryUserId.Clear();
        }

        /// <summary>
        /// Meetod mis testimise eesmärgil sisestab õpilase vormi andmeid, on võimalik konfigureerida.
        /// </summary>
        /// <param name="driver">WebDriver mida hetkel test rakendab</param>
        /// <param name="isOneOrTwo">Muutuja mis valib kahe erineva andmekomplekti vahel</param>
        /// <param name="updateName">Muutuja mis ütleb kas eesnime on vaja muuta (eesnimes hoitakse testimiseks vajalikku unikaalset Guidi)</param>
        /// <returns>Tagastab lisatud Guidi stringi kujul mis pandi eesnime välja (Kui on konfigureeritud nii)</returns>

        public string DataEntry_Student(IWebDriver driver, bool isOneOrTwo = false, bool updateName = true)
        {
            Guid generatedId = Guid.NewGuid();
            if (updateName)
            {
                IWebElement entryFirstName = driver.FindElement(By.Id("CU_Student_FirstName"));
                entryFirstName.SendKeys(generatedId.ToString());
            }

            if (!isOneOrTwo)
            {
                //andmekomplekt 1 - perekonnanimi algab A-tähega, et õpilane oleks nimekirja alguses
                IWebElement entryLastName = driver.FindElement(By.Id("CU_Student_LastName"));
                entryLastName.SendKeys("Automaattest");
                IWebElement entryUserId = driver.FindElement(By.Id("CU_Student_UserID"));
                entryUserId.SendKeys("selenium-test-01");
            }
            else
            {
                //andmekomplekt 2 - kasutatakse muutmisel
                IWebElement entryLastName = driver.FindElement(By.Id("CU_Student_LastName"));
                entryLastName.SendKeys("Aavik");
                IWebElement entryUserId = driver.FindElement(By.Id("CU_Student_UserID"));
                entryUserId.SendKeys("selenium-test-02");
            }
            return generatedId.ToString();
        }

        /// <summary>
        /// Ülesseade meetod mis tekitab draiveri, navigeerib aadressile ja vajutab Õpilaste lingile.
        /// </summary>
        /// <returns>Tagastab testisiseselt kasutatava draiveri</returns>
        private static IWebDriver TestSetup_NavigateToIndex()
        {
            var options = new ChromeOptions { AcceptInsecureCertificates = true };
            IWebDriver driver = new ChromeDriver(options); //(ChromeDriver) brauserit juhtiv ja seal tegutsev draiver
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Url = "https://localhost:7153/"; //navigeeritav aadress
            IWebElement navigateToStudents = driver.FindElement(By.LinkText("Õpilased")); //otsime testitava osa lingi
            navigateToStudents.Click(); //simuleeritud tegevus elemendiga, antud juhul klikk
            return driver;
        }
    }
}
