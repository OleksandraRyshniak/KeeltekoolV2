using Keeltekool_2.Core.Domain;
using Keeltekool_2.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Keeltekool_2.Core.ServiceInterface
{
    public interface ILecturersServices
    {
        Task<Lecturer> Create(LecturerDTO dto);
        Task<Lecturer> Update(LecturerDTO dto);
        Task<Lecturer> DetailAsync(Guid id);
        Task<Lecturer> Delete(Guid id);
    }
}
