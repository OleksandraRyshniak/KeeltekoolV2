using Keeltekool_2.Core.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Keeltekool_2.Core.DTO
{
    public class LecturerDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //Fullname TBA
        public string Qualification { get; set; }
        public string? UserID { get; set; }
        // public ICollection<LanguageSubject> LanguageSubject { get; set; }

        public List<IFormFile>? Files { get; set; }
        public IEnumerable<FileToDatabaseDTO>? Image { get; set; } = new List<FileToDatabaseDTO>();
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
