
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Keeltekool_2.Core.Domain
{
    
    public class ApplicationUser : IdentityUser
    {
    public string Placeholder { get; set; }
    public string Name { get; set; }
        public RegisterStatus AccountStatus { get; set; }
    }
}
