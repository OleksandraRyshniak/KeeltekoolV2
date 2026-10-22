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
    }
}
