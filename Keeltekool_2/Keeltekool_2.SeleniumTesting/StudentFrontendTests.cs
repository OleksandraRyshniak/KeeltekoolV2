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

        /// <summary>
        /// Test kustutab eelnevalt loodud ja muudetud õpilase kustutamise vaate kaudu.
        /// Kontrollib, et kustutamise vaade näitab õige õpilase andmeid ning et pärast
        /// "Kustuta" vajutamist suunatakse nimekirja, kus seda õpilast enam ei ole.
        /// </summary>
        [Fact]
        [Order(4)]
        public void Should_DeleteUniqueStudent_FromDeleteView()
        {
            /* ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            bool isObjectFound = false;
            int rowsBefore = rowsOfTable.Count; //ridade arv enne kustutamist

            /* tegevus */
            foreach (var row in rowsOfTable)
            {
                var firstNameElement = row.FindElement(By.Id("IndexTableStudentFirstName")).Text;
                if (firstNameElement == thisRunGuid)
                {
                    isObjectFound = true;
                    var clickAction = row
                        .FindElement(By.Id("IndexTableStudentActions"))
                        .FindElement(By.Id("IndexTableStudentDelete_BTN"));
                    clickAction.Click();
                    break;
                }
            }
            Thread.Sleep(1000);
            //kontrollime, et kustutamise vaates on õige õpilane
            string delete_firstName = driver.FindElement(By.Id("DD_StudentFirstName")).Text;
            string delete_lastName = driver.FindElement(By.Id("DD_StudentLastName")).Text;
            IWebElement clickDelete = driver.FindElement(By.Id("DeleteDelete_BTN"));
            clickDelete.Click();

            //kontroll
            Thread.Sleep(1000);
            bool isObjectFoundAfterDelete = false;
            tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            int rowsAfter = rowsOfTable.Count; //ridade arv pärast kustutamist
            foreach (var row in rowsOfTable)
            {
                var firstNameElement = row.FindElement(By.Id("IndexTableStudentFirstName")).Text;
                if (firstNameElement == thisRunGuid)
                {
                    isObjectFoundAfterDelete = true;
                    break;
                }
            }
            driver.Quit();

            Assert.True(isObjectFound);
            Assert.Equal(thisRunGuid, delete_firstName); //kustutamise vaates on õige õpilane
            Assert.Equal("Aavik", delete_lastName); //andmed on juba muudetud (Update test)
            Assert.False(isObjectFoundAfterDelete); //õpilast pole enam nimekirjas
            Assert.True(!isObjectFoundAfterDelete);
            Assert.True(rowsAfter < rowsBefore || rowsBefore == 20); //ridu jäi vähemaks (kui nimekiri polnud täis)
        }

        /// <summary>
        /// Test proovib salvestada tühja õpilase vormi.
        /// Kontrollib, et kasutaja jääb loomise vaatesse, eesnime ja perekonnanime juures
        /// kuvatakse eestikeelsed veateated ning nimekirja ei lisandu ühtegi uut rida.
        /// </summary>
        [Fact]
        [Order(5)]
        public void Should_ShowValidationErrors_WhenCreatingStudentWithEmptyFields()
        {
            /* Ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            int rowsBefore = rowsOfTable.Count; //ridade arv enne

            /* tegevus */
            IWebElement indexCreateButton = driver.FindElement(By.Id("IndexCreateNewStudent_BTN"));
            indexCreateButton.Click(); //avame loomise vaate
            IWebElement CreateCreateButton = driver.FindElement(By.Id("CU_Create_BTN"));
            CreateCreateButton.Click(); //vajutame salvesta ilma andmeid sisestamata
            Thread.Sleep(500);

            /* kontroll */
            //kogume andmed
            string urlAfterSubmit = driver.Url;
            string firstNameError = driver.FindElement(By.Id("CU_Student_FirstName_Error")).Text;
            string lastNameError = driver.FindElement(By.Id("CU_Student_LastName_Error")).Text;

            //läheme tagasi nimekirja ja loeme read uuesti üle
            IWebElement goBack = driver.FindElement(By.Id("CU_Cancel_BTN"));
            goBack.Click();
            Thread.Sleep(500);
            tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            int rowsAfter = rowsOfTable.Count; //ridade arv pärast

            //sulgeme akna
            driver.Quit();
            //assertime
            Assert.Contains("/Students/Create", urlAfterSubmit); //jäime loomise vaatesse
            Assert.Equal("Eesnimi on kohustuslik", firstNameError);
            Assert.Equal("Perekonnanimi on kohustuslik", lastNameError);
            Assert.Equal(rowsBefore, rowsAfter); //uut õpilast ei lisatud
        }

        /// <summary>
        /// Test loob uue õpilase, avab tema kustutamise vaate ja vajutab "Tagasi nimekirja" (mitte "Kustuta").
        /// Kontrollib, et kustutamine katkestati: õpilane on endiselt nimekirjas ja tema andmed ei muutunud.
        /// Testi lõpus kustutatakse loodud õpilane, et andmebaasi ei jääks testiandmeid.
        /// </summary>
        [Fact]
        [Order(6)]
        public void Should_KeepStudent_WhenDeleteIsCancelled()
        {
            /* Ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            //loome testi jaoks oma õpilase
            IWebElement indexCreateButton = driver.FindElement(By.Id("IndexCreateNewStudent_BTN"));
            indexCreateButton.Click();
            string uniqueId = DataEntry_Student(driver);
            IWebElement CreateCreateButton = driver.FindElement(By.Id("CU_Create_BTN"));
            CreateCreateButton.Click();
            Thread.Sleep(500);

            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            bool isObjectFound = false;

            /* tegevus */
            foreach (var row in rowsOfTable)
            {
                var firstNameElement = row.FindElement(By.Id("IndexTableStudentFirstName")).Text;
                if (firstNameElement == uniqueId)
                {
                    isObjectFound = true;
                    var clickAction = row
                        .FindElement(By.Id("IndexTableStudentActions"))
                        .FindElement(By.Id("IndexTableStudentDelete_BTN"));
                    clickAction.Click();
                    break;
                }
            }
            Thread.Sleep(1000);
            IWebElement goBack = driver.FindElement(By.Id("DeleteBack_BTN"));
            goBack.Click(); //katkestame kustutamise
            Thread.Sleep(1000);

            /* kontroll */
            //kogume andmed
            string urlAfterCancel = driver.Url;
            bool isObjectFoundAfterCancel = false;
            string lastNameAfterCancel = "";
            tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            foreach (var row in rowsOfTable)
            {
                var firstNameElement = row.FindElement(By.Id("IndexTableStudentFirstName")).Text;
                if (firstNameElement == uniqueId)
                {
                    isObjectFoundAfterCancel = true;
                    lastNameAfterCancel = row.FindElement(By.Id("IndexTableStudentLastName")).Text;
                    //koristame testiandmed - kustutame loodud õpilase
                    row.FindElement(By.Id("IndexTableStudentActions"))
                       .FindElement(By.Id("IndexTableStudentDelete_BTN"))
                       .Click();
                    driver.FindElement(By.Id("DeleteDelete_BTN")).Click();
                    break;
                }
            }

            //sulgeme akna
            Thread.Sleep(500);
            driver.Quit();
            //assertime
            Assert.True(isObjectFound);
            Assert.DoesNotContain("/Students/Delete", urlAfterCancel); //lahkusime kustutamise vaatest
            Assert.True(isObjectFoundAfterCancel); //õpilane on endiselt alles
            Assert.Equal("Automaattest", lastNameAfterCancel); //andmed ei muutunud
        }

        /// <summary>
        /// Test loob uue õpilase, avab muutmise vormi, kirjutab eesnime ja perekonnanime üle
        /// ning vajutab "Tagasi nimekirja" ilma salvestamata.
        /// Kontrollib, et muudatusi ei salvestatud: nimekirjas on vana eesnimi ja perekonnanimi
        /// ning uue eesnimega õpilast ei ole. Testi lõpus kustutatakse loodud õpilane.
        /// </summary>
        [Fact]
        [Order(7)]
        public void Should_NotSaveChanges_WhenUpdateIsCancelled()
        {
            /* Ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            //loome testi jaoks oma õpilase
            IWebElement indexCreateButton = driver.FindElement(By.Id("IndexCreateNewStudent_BTN"));
            indexCreateButton.Click();
            string uniqueId = DataEntry_Student(driver);
            IWebElement CreateCreateButton = driver.FindElement(By.Id("CU_Create_BTN"));
            CreateCreateButton.Click();
            Thread.Sleep(500);

            string changedName = Guid.NewGuid().ToString(); //uus eesnimi, mida ei tohi salvestada
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            bool isObjectFound = false;

            /* tegevus */
            foreach (var row in rowsOfTable)
            {
                var firstNameElement = row.FindElement(By.Id("IndexTableStudentFirstName")).Text;
                if (firstNameElement == uniqueId)
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
            DataClear_CreateUpdate(driver); //puhastame kõik väljad
            driver.FindElement(By.Id("CU_Student_FirstName")).SendKeys(changedName);
            driver.FindElement(By.Id("CU_Student_LastName")).SendKeys("Aavik");
            IWebElement goBack = driver.FindElement(By.Id("CU_Cancel_BTN"));
            goBack.Click(); //lahkume salvestamata
            Thread.Sleep(1000);

            /* kontroll */
            //kogume andmed
            bool isOriginalFound = false;
            bool isChangedFound = false;
            string lastNameAfterCancel = "";
            tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            foreach (var row in rowsOfTable)
            {
                var firstNameElement = row.FindElement(By.Id("IndexTableStudentFirstName")).Text;
                if (firstNameElement == changedName)
                {
                    isChangedFound = true;
                }
                if (firstNameElement == uniqueId)
                {
                    isOriginalFound = true;
                    lastNameAfterCancel = row.FindElement(By.Id("IndexTableStudentLastName")).Text;
                }
            }

            //koristame testiandmed - kustutame loodud õpilase
            tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            foreach (var row in rowsOfTable)
            {
                var firstNameElement = row.FindElement(By.Id("IndexTableStudentFirstName")).Text;
                if (firstNameElement == uniqueId || firstNameElement == changedName)
                {
                    row.FindElement(By.Id("IndexTableStudentActions"))
                       .FindElement(By.Id("IndexTableStudentDelete_BTN"))
                       .Click();
                    driver.FindElement(By.Id("DeleteDelete_BTN")).Click();
                    break;
                }
            }

            //sulgeme akna
            Thread.Sleep(500);
            driver.Quit();
            //assertime
            Assert.True(isObjectFound);
            Assert.True(isOriginalFound); //õpilane on alles vana eesnimega
            Assert.False(isChangedFound); //uue eesnimega õpilast ei ole
            Assert.Equal("Automaattest", lastNameAfterCancel); //perekonnanimi ei muutunud
            Assert.NotEqual("Aavik", lastNameAfterCancel);
        }

        /// <summary>
        /// Test loob uue õpilase, avab muutmise vormi, tühjendab eesnime välja, muudab perekonnanime
        /// ja proovib salvestada. Kontrollib, et kasutaja jääb muutmise vaatesse, eesnime juures
        /// kuvatakse veateade ning õpilase andmed jäävad muutmata (ka perekonnanimi ei salvestu).
        /// Testi lõpus kustutatakse loodud õpilane.
        /// </summary>
        [Fact]
        [Order(8)]
        public void Should_ShowValidationError_WhenUpdatingStudentWithEmptyFirstName()
        {
            /* Ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            //loome testi jaoks oma õpilase
            IWebElement indexCreateButton = driver.FindElement(By.Id("IndexCreateNewStudent_BTN"));
            indexCreateButton.Click();
            string uniqueId = DataEntry_Student(driver);
            IWebElement CreateCreateButton = driver.FindElement(By.Id("CU_Create_BTN"));
            CreateCreateButton.Click();
            Thread.Sleep(500);

            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            bool isObjectFound = false;

            /* tegevus */
            foreach (var row in rowsOfTable)
            {
                var firstNameElement = row.FindElement(By.Id("IndexTableStudentFirstName")).Text;
                if (firstNameElement == uniqueId)
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
            driver.FindElement(By.Id("CU_Student_FirstName")).Clear(); //tühjendame ainult eesnime
            IWebElement entryLastName = driver.FindElement(By.Id("CU_Student_LastName"));
            entryLastName.Clear();
            entryLastName.SendKeys("Aavik"); //perekonnanimi on korrektne
            IWebElement updateClick = driver.FindElement(By.Id("CU_Update_BTN"));
            updateClick.Click();
            Thread.Sleep(500);

            /* kontroll */
            //kogume andmed
            string urlAfterSubmit = driver.Url;
            string firstNameError = driver.FindElement(By.Id("CU_Student_FirstName_Error")).Text;
            string lastNameError = driver.FindElement(By.Id("CU_Student_LastName_Error")).Text;

            //läheme tagasi nimekirja ja vaatame, kas andmed jäid samaks
            IWebElement goBack = driver.FindElement(By.Id("CU_Cancel_BTN"));
            goBack.Click();
            Thread.Sleep(1000);
            bool isStillFound = false;
            string lastNameAfter = "";
            string userIdAfter = "";
            tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            foreach (var row in rowsOfTable)
            {
                var firstNameElement = row.FindElement(By.Id("IndexTableStudentFirstName")).Text;
                if (firstNameElement == uniqueId)
                {
                    isStillFound = true;
                    lastNameAfter = row.FindElement(By.Id("IndexTableStudentLastName")).Text;
                    userIdAfter = row.FindElement(By.Id("IndexTableStudentUserID")).Text;
                    //koristame testiandmed - kustutame loodud õpilase
                    row.FindElement(By.Id("IndexTableStudentActions"))
                       .FindElement(By.Id("IndexTableStudentDelete_BTN"))
                       .Click();
                    driver.FindElement(By.Id("DeleteDelete_BTN")).Click();
                    break;
                }
            }

            //sulgeme akna
            Thread.Sleep(500);
            driver.Quit();
            //assertime
            Assert.True(isObjectFound);
            Assert.Contains("/Students/Update", urlAfterSubmit); //jäime muutmise vaatesse
            Assert.Equal("Eesnimi on kohustuslik", firstNameError);
            Assert.Empty(lastNameError); //perekonnanimi oli täidetud, seal viga pole
            Assert.True(isStillFound); //õpilane on alles vana eesnimega
            Assert.Equal("Automaattest", lastNameAfter); //ka perekonnanimi ei salvestunud
            Assert.Equal("selenium-test-01", userIdAfter);
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
