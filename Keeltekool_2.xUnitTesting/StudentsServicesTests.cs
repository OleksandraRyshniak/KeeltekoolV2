using Keeltekool_2.ApplicationServices.Services;
using Keeltekool_2.Core.Domain;
using Keeltekool_2.Core.DTO;
using Keeltekool_2.Core.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Xunit;

namespace Keeltekool_2.xUnitTesting
{
    public class StudentsServicesTests : TestBase
    {

        /// <summary>
        /// Kontrollib, kas uue õpilase loomine õnnestub
        /// ja kas loodud õpilase andmed on õiged.
        /// </summary>
        [Fact]
        public async Task Should_AddNewStudent_WhenResultIsReturned()
        {
            //ülesseade
            StudentDTO dto = new StudentDTO();
            dto.FirstName = "Test";
            dto.LastName = "Test";

            //tegevus
            var result = await Svc<IStudentsServices>().Create(dto);

            //kontroll
            //kontrollime et tagastati midagi
            Assert.NotNull(result);

            //kontrollime et id ei ole tühi
            Assert.NotEqual(Guid.Empty, result.Id);

            //kontrollime et nimi on sama
            Assert.Equal(dto.FirstName, result.FirstName);
            Assert.Equal(dto.LastName, result.LastName);

            //kontrollime kuupäevi
            Assert.True(result.CreatedAt <= DateTime.Now);
            Assert.True(result.ModifiedAt <= DateTime.Now);
        }

        /// <summary>
        /// Kontrollib, kas olemasoleva õpilase andmed
        /// leitakse ja tagastatakse õige ID alusel.
        /// </summary>
        [Fact]
        public async Task Should_ReturnStudentDetails_WhenGuidIsNotNull()
        {
            //ülesseade
            StudentDTO dto = new StudentDTO();
            dto.FirstName = "Test";
            dto.LastName = "Test";

            var student = await Svc<IStudentsServices>().Create(dto);

            //tegevus
            var result = await Svc<IStudentsServices>().DetailAsync(student.Id);

            //kontroll
            //kontrollime et tagastati midagi
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(dto.FirstName, result.FirstName);
            Assert.Equal(dto.LastName, result.LastName);
            Assert.Equal(student.Id, result.Id);
        }


        /// <summary>
        /// Kontrollib, kas õpilase eesnime ja perekonnanime
        /// saab muuta ning kas uued andmed tagastatakse õigesti.
        /// </summary>
        [Fact]
        public async Task Should_UpdateWithNewData_WhenDataIsDiffrentFromDB()
        {
            // ülesseade
            StudentDTO dto = new StudentDTO();
            dto.FirstName = "Test";
            dto.LastName = "Student";

            var student = await Svc<IStudentsServices>().Create(dto);

            StudentDTO updateDto = new StudentDTO();
            updateDto.Id = student.Id;
            updateDto.FirstName = "Anna";
            updateDto.LastName = "Smith";

            // tegevus
            var result = await Svc<IStudentsServices>().Update(updateDto);

            //kontroll
            Assert.NotNull(result);
            Assert.Equal(student.Id, result.Id);
            Assert.Equal("Anna", result.FirstName);
            Assert.Equal("Smith", result.LastName);
            Assert.NotEqual(dto.FirstName, result.FirstName);
            Assert.NotEqual(dto.LastName, result.LastName);
        }

        /// <summary>
        /// Kontrollib, kas olemasoleva õpilase kustutamine õnnestub
        /// ja kas kustutatud õpilast enam andmebaasist ei leita.
        /// </summary>
        [Fact]
        public  async Task Should_DeleteStudent_WhenValidGiven()
        {
            //ülesseade
            StudentDTO dto = new StudentDTO();
            dto.FirstName = "Test";
            dto.LastName = "Student";

            var student = await Svc<IStudentsServices>().Create(dto);

            //tegevus
            var result = await Svc<IStudentsServices>().Delete(student.Id);
            var deletedStudent = await Svc<IStudentsServices>().DetailAsync(student.Id);

            //kontroll
            Assert.NotNull(result);
            Assert.Equal(student.Id, result.Id);
            Assert.Equal(student.FirstName, result.FirstName);
            Assert.Equal(student.LastName, result.LastName);
            Assert.Null(deletedStudent);
            Assert.NotEqual(Guid.Empty, result.Id);
        }

        /// <summary>
        /// Kontrollib, kas olematu õpilase otsimisel
        /// tagastatakse null.
        /// </summary
        /// >
        [Fact]
        public async Task Should_ReturnNull_WhenStudentDoesNotExist()
        {
            //ülesseade
            Guid id = Guid.NewGuid();

            //tegevus
            var result = await Svc<IStudentsServices>().DetailAsync(id);

            //kontroll
            Assert.Null(result);
            Assert.NotEqual(Guid.Empty, id);
            Assert.True(id != Guid.Empty);
            Assert.False(result is Student);
            Assert.IsNotType<Student>(result);
        }


        /// <summary>
        /// Kontrollib, kas olematu õpilase kustutamisel tagastatakse null
        /// ja olemasoleva õpilase andmed jäävad muutmata.
        /// </summary>
        [Fact]
        public async Task Should_ReturnNull_WhenDeletingNonexistenStudent()
        {
            //ülesseade
            StudentDTO dto = new StudentDTO();
            dto.FirstName = "Test";
            dto.LastName = "Student";

            var student = await Svc<IStudentsServices>().Create(dto);
            Guid wrongId = Guid.NewGuid();

            //tegevus
            var result = await Svc<IStudentsServices>().Delete(wrongId);
            var existingStudent = await Svc<IStudentsServices>().DetailAsync(student.Id);

            //kontroll
            Assert.Null(result);
            Assert.NotNull(existingStudent);
            Assert.Equal(student.Id, existingStudent.Id);
            Assert.Equal(dto.FirstName, existingStudent.FirstName);
            Assert.Equal(dto.LastName, existingStudent.LastName);
            Assert.NotEqual(student.Id, wrongId);
        }

        /// <summary>
        /// Kontrollib, kas olematu õpilase muutmisel tagastatakse null
        /// ja olemasoleva õpilase andmed jäävad muutmata.
        /// </summary>
        [Fact]
        public async Task Should_ReturnNull_WhenUpdatingNonexistenStudent()
        {
            //ülesseade
            StudentDTO dto = new StudentDTO();
            dto.FirstName = "Test";
            dto.LastName = "Student";

            var student = await Svc<IStudentsServices>().Create(dto);

            StudentDTO updateDto = new StudentDTO();
            updateDto.Id = Guid.NewGuid();
            updateDto.FirstName = "Anna";
            updateDto.LastName = "Smith";

            //tegevus
            var result = await Svc<IStudentsServices>().Update(updateDto);
            var existingStudent = await Svc<IStudentsServices>().DetailAsync(student.Id);

            //kontroll
            Assert.Null(result);
            Assert.NotNull(existingStudent);
            Assert.Equal(student.Id, existingStudent.Id);
            Assert.Equal(dto.FirstName, existingStudent.FirstName);
            Assert.Equal(dto.LastName, existingStudent.LastName);
            Assert.NotEqual(student.Id, updateDto.Id);
        }

        /// <summary>
        /// Kontrollib, kas tühjade nimeväljade korral
        /// määratakse õpilasele vaikimisi eesnimi ja perekonnanimi.
        /// </summary>
        [Fact]
        public async Task Should_AddNewStudent_WhenFieldsAreEmpty()
        {
            //ülesseade
            StudentDTO dto = new StudentDTO();
            dto.FirstName = "";
            dto.LastName = "";

            //tegevus
            var result = await Svc<IStudentsServices>().Create(dto);

            //kontroll
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal("Alex", result.FirstName);
            Assert.Equal("Doe", result.LastName);
            Assert.NotEqual("", result.FirstName);
            Assert.NotEqual("", result.LastName);
        }

        /// <summary>
        /// Kontrollib, kas õpilase andmete muutmisel
        /// jääb algne loomise kuupäev samaks.
        /// </summary>
        [Fact]
        public async Task Should_KeepCreatedAt_WhenStudentIsUpdated()
        {
            //ülesseade
            StudentDTO dto = new StudentDTO();
            dto.FirstName = "Alex";
            dto.LastName = "Doe";

            var student = await Svc<IStudentsServices>().Create(dto);
            var createdAt = student.CreatedAt;

            StudentDTO updateDto = new StudentDTO();
            updateDto.Id = student.Id;
            updateDto.FirstName = "Anna";
            updateDto.LastName = "Smith";

            //tegevus
            var result = await Svc<IStudentsServices>().Update(updateDto);

            //kontroll
            Assert.NotNull(result);
            Assert.Equal(student.Id, result.Id);
            Assert.Equal(createdAt, result.CreatedAt);
            Assert.Equal("Anna", result.FirstName);
            Assert.Equal("Smith", result.LastName);
            Assert.NotEqual(Guid.Empty, result.Id);
        }

    }
}
