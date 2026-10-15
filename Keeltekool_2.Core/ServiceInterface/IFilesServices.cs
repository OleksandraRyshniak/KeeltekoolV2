using Keeltekool_2.Core.Domain;
using Keeltekool_2.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Keeltekool_2.Core.ServiceInterface
{
    public interface IFilesServices
    {
        void UploadFiles(LecturerDTO dto, Lecturer domain);
        Task<FileToDatabase> RemoveFileFromDatabase(FileToDatabaseDTO dto);
        Task<FileToDatabase> RemoveFileFromDatabase(FileToDatabaseDTO[] dtos);
    }
}
