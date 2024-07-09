using Microsoft.EntityFrameworkCore;
using RMApplication.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace RMApplication.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)

            : base(options)
        {
        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>()
            .HasMany(d => d.SubDepartments)
            .WithOne(d => d.ParentDepartment)
            .HasForeignKey(d => d.ParentDepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
