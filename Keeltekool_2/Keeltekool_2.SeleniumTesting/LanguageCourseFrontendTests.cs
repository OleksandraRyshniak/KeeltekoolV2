using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Keeltekool_2.SeleniumTesting
{
    public class LanguageCourseFrontendTests
    {
        [Fact]
        public void Should_NavigateToIndex_AddSpaceshipWithCorrectData_ReturnToIndex()
        {
            //Ülesseade
            var options = new FirefoxOptions { AcceptInsecureCertificates = true };
            IWebDriver driver = new FirefoxDriver(options); //(ChromeDriver) brauserit juhtiv ja seal tegutsev draiver
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Url = "https://localhost:7153/"; //navigieeritav aadress
            IWebElement navigateToLanguageCourse = driver.FindElement(By.LinkText("Keelekursused"));//otsime tesitava osa lingi
            navigateToLanguageCourse.Click();//simuleeritud tegevus elemendiga, antud juhul klikk

            //tegevus
            IWebElement indexCreateButton = driver.FindElement(By.Id("IndexCreateNewLanguageCourse_BTN"));
            indexCreateButton.Click();
            IWebElement createEntryNimetus = driver.FindElement(By.Id("CU_LanguageCourse_Nimetus"));
            createEntryNimetus.SendKeys("Automaattestimise kursus");
            IWebElement createEntryKeel = driver.FindElement(By.Id("CU_LanguageCourse_Keel"));
            createEntryKeel.SendKeys("C# + Selenium & xUnit");
            IWebElement createEntryTase = driver.FindElement(By.Id("CU_LanguageCourse_Tase"));
            createEntryTase.SendKeys("3");
            IWebElement createEntryKirjeldus = driver.FindElement(By.Id("CU_LanguageCourse_Kirjeldus"));
            createEntryKirjeldus.SendKeys("Kursusel õpime mis on automaattestimine, kuidas erineb manuaaltestimisest ning kuidas kirjutada backendi ja frontendi automaatteste.");
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
    }
}