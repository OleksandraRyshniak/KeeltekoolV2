
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace Keeltekool_2.Core.Domain
{
    
public class ApplicationUser : IdentityUser
{
    public string Placeholder { get; set; }
    public string Name { get; set; }  // Добавлено свойство Name
}
    }
