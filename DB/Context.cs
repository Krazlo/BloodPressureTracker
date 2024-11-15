using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using Helpers.Entities;

namespace DB
{
    public class Context : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Measurement> Measurements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "Server=localhost;Database=mysqlDB;User=sa;Password=S3cr3tP4sSw0rd#123;",
                new MySqlServerVersion(new Version(8, 0, 23))
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasKey(p => p.SSN);

            modelBuilder.Entity<Measurement>()
                .HasKey(m => m.ID);
        }
    }
}
