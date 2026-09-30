using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace Keeltekool_2.Data
{
    public class Keeltekool_2Context : IdentityDbContext<ApplicationUser>
    {
        public Keeltekool_2Context(DbContextOptions<Keeltekool_2Context> options) : base(options)
        {
            //set tables here
        }
    }
}
