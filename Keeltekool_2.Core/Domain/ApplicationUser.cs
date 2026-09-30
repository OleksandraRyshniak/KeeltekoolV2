using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Keeltekool_2.Core.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public ClaimsIdentity UserCredential { get; set; } = null;
    }
}
