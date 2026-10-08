using Keeltekool_2.Core.Domain;
using Keeltekool_2.Core.DTO;
using Keeltekool_2.Core.ServiceInterface;
using Keeltekool_2.Data;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Keeltekool_2.ApplicationServices.Services
{
    public class FileServices : IFilesServices
    {
        private readonly Keeltekool_2Context _context;
        private readonly IHostEnvironment _hostEnvironment;
        public FileServices(Keeltekool_2Context context, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        public Task<FileToDatabase> RemoveFileFromDatabase(FileToDatabaseDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<FileToDatabase> RemoveFileFromDatabase(FileToDatabaseDTO[] dtos)
        {
            throw new NotImplementedException();
        }

        public void UploadFiles(LecturerDTO dto, Lecturer domain)
        {
            if (dto.Files !=null && dto.Files.Count > 0)
            {
                foreach (var file in dto.Files)
                {
                    using (var target = new MemoryStream())
                    {
                        FileToDatabase files = new FileToDatabase()
                        {
                            ImageID = Guid.NewGuid(),
                            ImageTitle = file.FileName,
                            LecturerId = domain.Id
                        };
                        file.CopyTo(target);
                        files.ImageData = target.ToArray();
                        _context.Files.Add(files);
                    }
                }
            }
        }
    }
}
