using Keeltekool_2.Core.Domain;
using Keeltekool_2.Core.DTO;
using Keeltekool_2.Core.ServiceInterface;
using Keeltekool_2.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Keeltekool_2.ApplicationServices.Services
{
    public class LecturersServices : ILecturersServices
    {
        private readonly Keeltekool_2Context _context;
        private readonly IFilesServices _filesServices;

        public LecturersServices(Keeltekool_2Context context, IFilesServices filesServices)
        {
            _context = context;
            _filesServices = filesServices;
        }
        public async Task<Lecturer> Create(LecturerDTO dto)
        {
            if (dto == null)
            {
                return null;
            }

            Lecturer domain = new Lecturer();
            domain.Id = Guid.NewGuid();
            if (string.IsNullOrWhiteSpace(dto.FirstName))
            {
                dto.FirstName = "Jane";
            }
            if (string.IsNullOrWhiteSpace(dto.LastName))
            {
                dto.LastName = "Doe";
            }
            domain.FirstName = dto.FirstName;
            domain.LastName = dto.LastName;
            domain.Qualification = dto.Qualification;
            domain.UserID = dto.UserID ?? "TestUser123";
            domain.CreatedAt = DateTime.Now;
            domain.ModifiedAt = DateTime.Now;

            await _context.Lecturers.AddAsync(domain);
            await _context.SaveChangesAsync();
            return domain;
        }

        public async Task<Lecturer> DetailAsync(Guid id)
        {
            return await _context.Lecturers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Lecturer> Update(LecturerDTO dto)
        {
            var domain = await _context.Lecturers.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (domain == null)
            {
                return null;
            }

            domain.FirstName = dto.FirstName;
            domain.LastName = dto.LastName;
            domain.Qualification = dto.Qualification;

            if (dto.Files != null)
            {
                _filesServices.UploadFiles(dto, domain);
            }

            _context.Lecturers.Update(domain);
            await _context.SaveChangesAsync();
            return domain;
        }

        public async Task<Lecturer> Delete(Guid id)
        {
            var domain = await _context.Lecturers.FirstOrDefaultAsync(x => x.Id == id);
            if (domain == null)
            {
                return null;
            }

            _context.Lecturers.Remove(domain);
            await _context.SaveChangesAsync();
            return domain;
        }
    }
}
