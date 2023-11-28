using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyGuide_WebApp.Models;

namespace StudyGuide_WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<SemesterModel> Semesters { get; set; }
        public DbSet<ModuleModel> Modules { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}