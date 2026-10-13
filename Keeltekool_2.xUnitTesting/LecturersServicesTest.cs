using Keeltekool_2.Core.Domain;
using Keeltekool_2.Core.DTO;
using Keeltekool_2.Core.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Keeltekool_2.xUnitTesting
{
    public class LecturersServicesTest : TestBase
    {
        [Fact]
        public async Task Should_AddNewLecturer_WhenResultISReturned()
        {
            //ülesseade
            LecturerDTO dto = MockLecturerDTOData();

            //tegevus
            var result = await Svc<ILecturersServices>().Create(dto);

            //kontroll
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(dto.FirstName, result.FirstName);
            Assert.Equal(dto.LastName, result.LastName);
            Assert.Equal(dto.Qualification, result.Qualification);
            Assert.False(string.IsNullOrWhiteSpace(result.UserID));
        }

        //details test
        [Fact]
        public async Task Should_ReturnLecturerDetails_WhenGuidIsNotNull()
        {
            //ülesseade
            LecturerDTO lecture = MockLecturerDTOData();

            //tegevus
            var createdLecture = await Svc<ILecturersServices>().Create(lecture);
            var result = await Svc<ILecturersServices>().DetailAsync(createdLecture.Id);

            //kontroll
            Assert.NotNull(result);
            Assert.Equal(createdLecture.Id, result.Id);
        }

        //update test
        [Fact]
        public async Task Should_UpdateLecturerWithNewData_WhenDataIsDifferentFromDB()
        {
            //ülesseade
            Lecturer createdLecture = await AddLecturerToDB();

            LecturerDTO updateDto = new LecturerDTO();
            updateDto.Id = createdLecture.Id;
            updateDto.FirstName = "UpdatedFirstName";
            updateDto.LastName = "UpdatedLastName";
            updateDto.Qualification = "UpdatedQualification";

            //tegevus
            var result = await Svc<ILecturersServices>().Update(updateDto);

            //kontroll
            Assert.NotNull(result);
            Assert.Equal(updateDto.Id, result.Id);
            Assert.Equal(updateDto.FirstName, result.FirstName);
            Assert.Equal(updateDto.LastName, result.LastName);
            Assert.Equal(updateDto.Qualification, result.Qualification);
        }

        //delete test

        [Fact]
        public async Task Should_DeleteLecturer_WhenValidIdIsGiven()
        {
            // Arrange
            Lecturer created = await AddLecturerToDB();

            // Act
            var result = await Svc<ILecturersServices>().Delete(created.Id);
            var deleted = await Svc<ILecturersServices>().DetailAsync(created.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Null(deleted);
        }


        private async Task<Lecturer> AddLecturerToDB()
        {
            return await Svc<ILecturersServices>().Create(MockLecturerDTOData());
        }


        private LecturerDTO MockLecturerDTOData()
        {
            return new LecturerDTO
            {
                FirstName = "Test",
                LastName = "Test",
                Qualification = "Testicles",
                UserID = "TestUser123"
            };
        }
    }
}