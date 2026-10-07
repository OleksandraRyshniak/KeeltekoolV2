using Keeltekool_2.Core.DTO;
using Keeltekool_2.Core.ServiceInterface;
using Keeltekool_2.Core.Domain;
using Keeltekool_2.xUnitTesting;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Xunit;

namespace Keeltekool_2.xUnitTesting
{
    public class LanguageCoursesServicesTests : TestBase
    {
        //[Fact] //Käsusõna, mida testrunner tunneb, et aru saada mis on test, ja mis ei ole
        // 1 - Kirjeldatakse ära, kas test on tavaline (peaks/ei tohi teha), või negatiivne (ei tohi/peaks tegema)
        // 2 - Mida parasjagu testiga testitakse.
        // 3 - Mis tingimustel tulemust kontrollitakse, peale tegevust
        //                  1           2           3
        //                  \/          \/          \/
        //public async Task Should_AddNewCourse_WhenResultIsReturned()
        //{
        //    //ülesseade
        //    LanguageCourseDTO newCourseDTO = new LanguageCourseDTO();
        //    newCourseDTO.Nimetus = "TestKursus";
        //    newCourseDTO.Keel = "Eesti keel";
        //    newCourseDTO.Tase = "Algtase";
        //    newCourseDTO.Kirjeldus = "A0 tasemel eesti keele \"õpe\", tule ja raiska aega";

        //    //tegevus
        //    var result = await Svc<ILanguageCoursesServices>().Create(newCourseDTO);

        //    //kontroll
        //    Assert.NotNull(result);
        //    /*
        //     Assert on klass mille abil saab kontrollita andmete eri tingimusi, kujusid, olekuid jne.
        //    Antud juhul kontrollitakse eelnevat objekti ühe kontrolliga - et ei oleks tühi.
        //    Aga, kui meie meetod pärast selle sisu arendamist hakkab juba tagastama mingisugust objekti, 
        //    tuleks testi täiendada, täpsemate tingimustega, mis kontrollib näiteks, kas on samasugune, 
        //    sisaldab kindlal kujul andmeid, andmed on mingit kindlat tüüpi jne. Võimalusi mida kontrollida on palju,
        //    ning viise kuidas teste kirjutada veelgi rohkem.
        //     */
        //}

        [Fact]
        public async Task ShouldNot_AddNewCourse_WhenFieldsEmpty()
        {
            //ülesseade
            LanguageCourseDTO newCourse = MockLanguageCourseDTOData();
            newCourse.Keel = string.Empty;
            newCourse.Nimetus = string.Empty;
            //tegevus
            var result = await Svc<ILanguageCoursesServices>().Create(newCourse);
            //kontroll
            Assert.Null(result); //kontrollime et teenus lükkaks objekti lisamise tagasi
            if (result != null)
            {
                //kontrollime et keel oleks juurde lisatud, ja mitte tühi
                Assert.NotNull(result.Keel);
                Assert.NotNull(result.Nimetus);
                //kontrollime et muutujates oleks midagi lisatud
                Assert.True(result.Keel.Length > 0);
                Assert.False(result.Nimetus.Length < 1);
                //kontrollime et teenus ei kaota ära vahepeal andmeid mis me sisestasime
                Assert.Equal(newCourse.Keel, result.Keel);
                Assert.Equal(newCourse.Nimetus, result.Nimetus);
            }
        }
        //Detailstest
        [Fact]
        public async Task Should_ReturnCourseDetails_WhenGuidIsNotNull()//??
        {
            //ülesseade
            //tekitame uue objekti
            LanguageCourseDTO course = MockLanguageCourseDTOData();
            //lisame andmebaasi
            var createdCourse = await Svc<ILanguageCoursesServices>().Create(course);

            //tegevus
            //kasutame objektis asuvat id et see objekt tagasi lugeda andmebaasist DetailsAsync meetodiga
            var result = await Svc<ILanguageCoursesServices>().DetailsAsync(createdCourse.Id);

            //kontroll
            //kontrollime et tagastati midagi
            Assert.NotNull(result);
            //kontrollime et tagastatud objekti id on sama nagu see mis andmebaasi lisatud sai
            Assert.Equal(result.Id, createdCourse.Id); //on sama kontroll nagu alumine
            Assert.True(result.Id == createdCourse.Id); //on sama kontroll nagu ülemine, kirjapilt erineb
            ////võrdleme kas objekt on sama nagu see mis me genereerisime, va. id-ga
            Assert.Equal(result, createdCourse);
        }
        // 1 - Kirjeldatakse ära, kas test on tavaline (peaks/ei tohi teha), või negatiivne (ei tohi/peaks tegema)
        // 2 - Mida parasjagu testiga testitakse.
        // 3 - Mis tingimustel tulemust kontrollitakse, peale tegevust
        //                  1           2           3
        //                  \/          \/          \/
        public async Task Should_UpdateWithNewData_WhenDataIsDiffrentFromDB(LanguageCourseDTO result)
        {
            // ülesseade
            Core.Domain.LanguageCourse createdCourse = await AddObjectToDB();

            LanguageCourseDTO updatedCourse = new LanguageCourseDTO();
            updatedCourse.Id = createdCourse.Id;
            updatedCourse.Keel = createdCourse.Keel;
            updatedCourse.Kirjeldus = createdCourse.Kirjeldus;
            updatedCourse.Nimetus = "Uusnimetus";
            updatedCourse.ModifiedAt = DateTime.UtcNow;

            // tegevus
            LanguageCourse languageCourse = await Svc<ILanguageCoursesServices>().Update(updatedCourse);

            //kontroll
            Assert.NotNull(result);
            Assert.Equal(updatedCourse.Id, result.Id);
            Assert.Equal(updatedCourse.Keel, result.Keel);
            Assert.Equal(updatedCourse.Kirjeldus, result.Kirjeldus);
            Assert.Equal(updatedCourse.Tase, result.Tase);
            Assert.Equal(updatedCourse.Nimetus, result.Nimetus);
        }

        private async Task<Core.Domain.LanguageCourse> AddObjectToDB()
        {
            LanguageCourseDTO course = MockLanguageCourseDTOData();
            var createdCourse = await Svc<ILanguageCoursesServices>().Create(course);
            return createdCourse;
        }

        [Fact]
        public async Task Should_DeleteDataFromDB_WhenValidIDIsGiven()
        {
            //ülesseade
            Core.Domain.LanguageCourse createdCourse = await AddObjectToDB();

            //tegevus
            var deleteCourse = await Svc<ILanguageCoursesServices>().Delete(createdCourse.Id);
            var result = await Svc<ILanguageCoursesServices>().DetailsAsync(createdCourse.Id);

            //Kontroll
            Assert.Null(result);
            Assert.Equal(createdCourse, deleteCourse);
            Assert.Equal(createdCourse.Id, deleteCourse.Id);
            Assert.NotEqual(deleteCourse, result);
        }

        private LanguageCourseDTO MockLanguageCourseDTOData()
        {
            return new LanguageCourseDTO
            {
                Nimetus = "TestKursus",
                Keel = "Eesti keel",
                Tase = "Algtase",
                Kirjeldus = "A0 tasemel eesti keele \"õpe\", tule ja raiska aega"
            };
        }
    }
}