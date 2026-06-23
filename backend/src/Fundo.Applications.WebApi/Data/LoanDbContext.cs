using Fundo.Applications.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Fundo.Applications.WebApi.Data
{
    public class LoanDbContext : DbContext
    {
        public LoanDbContext(DbContextOptions<LoanDbContext> options)
            : base(options)
        {
        }

        public DbSet<Loan> Loans => Set<Loan>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.Property(e => e.ApplicantName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasData(
                    new Loan
                    {
                        Id = 1,
                        Amount = 1500.00m,
                        CurrentBalance = 500.00m,
                        ApplicantName = "Maria Silva",
                        Status = "active",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Loan
                    {
                        Id = 2,
                        Amount = 1200.00m,
                        CurrentBalance = 0m,
                        ApplicantName = "Ricardo Gomes",
                        Status = "paid",
                        CreatedAt = DateTime.UtcNow.AddDays(-45),
                        PaidAt = DateTime.UtcNow.AddDays(-5)
                    },
                    new Loan
                    {
                        Id = 3,
                        Amount = 20000.00m,
                        CurrentBalance = 15800.00m,
                        ApplicantName = "Ana Costa",
                        Status = "active",
                        CreatedAt = DateTime.UtcNow.AddDays(-10)
                    });
            });
        }
    }
}
