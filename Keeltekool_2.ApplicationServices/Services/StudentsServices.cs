using Keeltekool_2.Core.Domain;
using Keeltekool_2.Core.DTO;
using Keeltekool_2.Core.ServiceInterface;
using Keeltekool_2.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Keeltekool_2.ApplicationServices.Services
{
    public class StudentsServices : IStudentsServices
    {
        private readonly Keeltekool_2Context _context;

        public StudentsServices(Keeltekool_2Context context)
        {
            _context = context;
        }

        public async Task<Student> Create(StudentDTO dto)
        {
            if (dto == null) 
            {
                return null;
            }
            Student domain = new Student();
            domain.Id = Guid.NewGuid();

            if (dto.FirstName.Length < 1)
            {
                dto.FirstName = "Alex";
            }
            if (dto.LastName.Length < 1)
            {
                dto.LastName = "Doe";
            }

            domain.FirstName = dto.FirstName;
            domain.LastName = dto.LastName;
            domain.UserID = dto.UserID;

            domain.CreatedAt = DateTime.Now;
            domain.ModifiedAt = DateTime.Now;

            await _context.Students.AddAsync(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<Student> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Student> DetailAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Student> Update(StudentDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
