using Keeltekool_2.Core.DTO;
using System;
using System.Collections.Generic;
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
            var result = await Svc<IStudentsService>().Create(dto);

            //kontroll
            Assert.NotNull(result);
        }
    }
}
