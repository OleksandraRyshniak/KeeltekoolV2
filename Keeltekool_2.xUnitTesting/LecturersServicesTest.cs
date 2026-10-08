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
        //update test
        //delete test
    }

    
}
