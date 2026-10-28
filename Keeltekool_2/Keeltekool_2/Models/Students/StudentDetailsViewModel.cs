
using System;
using System.ComponentModel.DataAnnotations;

namespace Keeltekool_2.Models.Students
{
    public class StudentDetailsViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Eesnimi")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Perekonnanimi")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Kasutaja ID")]
        public string? UserID { get; set; }

        [Display(Name = "Loomise kuupäev")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Muutmise kuupäev")]
        public DateTime ModifiedAt { get; set; }
    }
}
