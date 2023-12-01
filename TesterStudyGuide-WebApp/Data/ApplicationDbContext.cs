using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TesterStudyGuide_WebApp.Models;

namespace TesterStudyGuide_WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<SemesterModel> Semesters { get; set; }
        public DbSet<ModuleModel> Modules { get; set; }
        public DbSet<StudyRecordModel> Records { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //makes userId a foreign key in semester table
            modelBuilder.Entity<SemesterModel>()
            .HasOne(s => s.User)
            .WithMany(u => u.Semesters)
            .HasForeignKey(s => s.Id)
            .IsRequired();

            //makes semesterId a foreign key in module table
            modelBuilder.Entity<ModuleModel>()
                .HasOne(m => m.Semester)
                .WithMany(s => s.Modules)
                .HasForeignKey(m => m.semesterId)
                .OnDelete(DeleteBehavior.Restrict);

            //makes userId a foreign key in module table
            modelBuilder.Entity<ModuleModel>()
                .HasOne(m => m.User)
                .WithMany(u => u.Modules)
                .HasForeignKey(m => m.Id)
                .IsRequired();

            //makes code a foreign key in record table
            modelBuilder.Entity<StudyRecordModel>()
                .HasOne(s => s.Module)
                .WithMany()
                .HasForeignKey(s => s.code);

            //makes userId a foreign key in record table
            modelBuilder.Entity<StudyRecordModel>()
                .HasOne(s => s.User)
                .WithMany(u => u.Records)
                .HasForeignKey(s => s.Id)
                .OnDelete(DeleteBehavior.Restrict) //prevents cascading issues
                .IsRequired();
        }
    }
}