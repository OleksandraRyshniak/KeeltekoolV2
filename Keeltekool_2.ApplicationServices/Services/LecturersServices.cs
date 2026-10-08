using Keeltekool_2.Core.Domain;
using Keeltekool_2.Core.DTO;
using Keeltekool_2.Core.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Keeltekool_2.ApplicationServices.Services
{
    public class LecturersServices : ILecturersServices
    {
        public Task<Lecturer> Create(LecturerDTO dto)
        {
          
        }

        public Task<Lecturer> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Lecturer> DetailAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Lecturer> Update(LecturerDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
