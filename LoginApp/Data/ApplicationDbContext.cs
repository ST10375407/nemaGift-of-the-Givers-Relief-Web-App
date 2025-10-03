using System.Reflection;
using LoginApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<IncidentReport> IncidentReports { get; set; }
        public DbSet<ResourceDonation> ResourceDonations { get; set; }
        public DbSet<VolunteerTask> VolunteerTasks { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Ignore(u => u.Password)
                .Ignore(u => u.ConfirmPassword);
        }
    }
}
