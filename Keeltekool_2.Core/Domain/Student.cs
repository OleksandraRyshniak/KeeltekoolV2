using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Keeltekool_2.Core.Domain
{
    public class Student
    {
        public Guid Id { get; set; }
        /// <example>Nimi</example>
        [Required]
        public string FirstName { get; set; }
        /// <example>Perenimi</example>
        public string LastName { get; set; }

        /// <example>null</example>

        public string? UserID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
