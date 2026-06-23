using FluentAssertions;
using Fundo.Applications.WebApi.Data;
using Fundo.Applications.WebApi.Dtos;
using Fundo.Applications.WebApi.Models;
using Fundo.Applications.WebApi.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace Fundo.Services.Tests.Unit
{
    public class LoanServiceTests
    {
        private LoanDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<LoanDbContext>()
                .UseInMemoryDatabase(databaseName: "LoanServiceTests")
                .Options;

            var context = new LoanDbContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task CreateAsync_ShouldPersistLoanWithActiveStatus_WhenBalanceIsPositive()
        {
            var context = CreateContext();
            var service = new LoanService(context);

            var result = await service.CreateAsync(new CreateLoanRequest
            {
                Amount = 1200m,
                CurrentBalance = 1200m,
                ApplicantName = "Unit Test"
            });

            result.Should().NotBeNull();
            result.Status.Should().Be("active");
            result.CurrentBalance.Should().Be(1200m);
            result.ApplicantName.Should().Be("Unit Test");
        }

        [Fact]
        public async Task ProcessPaymentAsync_ShouldMarkLoanPaid_WhenBalanceReachesZero()
        {
            var context = CreateContext();
            var loan = new Loan
            {
                Amount = 500m,
                CurrentBalance = 100m,
                ApplicantName = "Payment User",
                Status = "active"
            };
            context.Loans.Add(loan);
            await context.SaveChangesAsync();

            var service = new LoanService(context);
            var result = await service.ProcessPaymentAsync(loan.Id, 100m);

            result.Should().NotBeNull();
            result!.Status.Should().Be("paid");
            result.CurrentBalance.Should().Be(0m);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnLoan_WhenLoanExists()
        {
            var context = CreateContext();
            var loan = new Loan
            {
                Amount = 800m,
                CurrentBalance = 800m,
                ApplicantName = "Find Me",
                Status = "active"
            };
            context.Loans.Add(loan);
            await context.SaveChangesAsync();

            var service = new LoanService(context);
            var result = await service.GetByIdAsync(loan.Id);

            result.Should().NotBeNull();
            result!.ApplicantName.Should().Be("Find Me");
        }
    }
}
