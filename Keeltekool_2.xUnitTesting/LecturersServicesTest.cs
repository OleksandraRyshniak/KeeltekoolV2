using Keeltekool_2.Core.Domain;
using Keeltekool_2.Core.DTO;
using Keeltekool_2.Core.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Keeltekool_2.xUnitTesting
{
    public class LecturersServicesTest : TestBase
    {
        [Fact]
        public async Task Should_AddNewLecturer_WhenResultISReturned()
        {
            //ülesseade
            LecturerDTO dto = new LecturerDTO();
            dto.FirstName = "Test";
            dto.LastName = "Test";
            dto.Qualification= "Testicles";
            // dto.Image =

            //tegevus
            var result = await Svc<ILecturersServices>().Create(dto);

            //kontroll
            Assert.NotNull(result);
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
            //Assert.Equal(result.Id, createdLecture.Id);
            //Assert.True(result.Id == createdLecture.Id);
            //Assert.Equal(result, createdLecture);
        }

        //update test
        [Fact]
        public async Task Should_UpdateLecturerWithNewData_WhenDataIsDifferentFromDB()
        {
            //ülesseade
            Lecturer createdLecture = await AddLecturerToDB();

            LecturerDTO updateDto = new LecturerDTO();
            updateDto.Id = createdLecture.Id;
            updateDto.FirstName = createdLecture.FirstName;
            updateDto.LastName = createdLecture.LastName;
            updateDto.Qualification = createdLecture.Qualification;

            //tegevus

            var result = await Svc<ILecturersServices>().Update(updateDto);

            //kontroll
            Assert.NotNull(result);
            //Assert.Equal(updateDto.Id, result.Id);
            //Assert.Equal(updateDto.FirstName, result.FirstName);
            //Assert.Equal(updateDto.LastName, result.LastName);
            //Assert.Equal(updateDto.Qualification, result.Qualification);
        }

        //delete test
        [Fact]
        public async Task Should_DeleteLecturer_WhenValidIdIsGiven()
        {
            //ülesseade
            Lecturer created = await AddLecturerToDB();

            //tegevus
            //var deleted = await Svc<ILecturersServices>().Delete(created.Id);
            var result = await Svc<ILecturersServices>().Delete(created.Id);

            //kontroll
            Assert.Null(result);
            //Assert.Equal(created.Id, deleted.Id);
            //Assert.NotEqual(deleted, result);
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
                Qualification = "Testicles"
            };
        }
    }


    
}
