using System.ComponentModel.DataAnnotations;

namespace Keeltekool_2.Models.Students
{
    public class StudentCreateViewModel
    {
        [Required(ErrorMessage = "Eesnimi on kohustuslik")]
        [Display(Name = "Eesnimi")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Perekonnanimi on kohustuslik")]
        [Display(Name = "Perekonnanimi")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Kasutaja ID")]
        public string? UserID { get; set; }
    }
}
