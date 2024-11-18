using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using Helpers.Entities;

namespace DB
{
    public class Context : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Measurement> Measurements { get; set; }

        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasKey(p => p.SSN);

            modelBuilder.Entity<Measurement>()
                .HasKey(m => m.ID);
        }
    }
}
