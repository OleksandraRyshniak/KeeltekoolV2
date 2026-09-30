using System.ComponentModel.DataAnnotations;

namespace Keeltekool_2.Models.Accounts
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}