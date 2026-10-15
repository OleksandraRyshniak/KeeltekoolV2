using Keeltekool_2.Core.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Keeltekool_2.Data
{
    public class Keeltekool_2Context : IdentityDbContext<ApplicationUser>
    {
        public Keeltekool_2Context(DbContextOptions<Keeltekool_2Context> options) : base(options)
        {

        }
        //set tables here
        public DbSet<LanguageCourse> LanguageCourses { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<FileToDatabase> Files { get; set; }
    }
}