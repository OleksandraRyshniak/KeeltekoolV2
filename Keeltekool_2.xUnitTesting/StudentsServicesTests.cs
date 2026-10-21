using Keeltekool_2.Core.DTO;
using Keeltekool_2.Core.ServiceInterface;
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
    }
}
