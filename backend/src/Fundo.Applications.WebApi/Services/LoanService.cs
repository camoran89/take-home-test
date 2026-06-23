using Fundo.Applications.WebApi.Data;
using Fundo.Applications.WebApi.Dtos;
using Fundo.Applications.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fundo.Applications.WebApi.Services
{
    public class LoanService : ILoanService
    {
        private readonly LoanDbContext _dbContext;

        public LoanService(LoanDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<LoanDto>> GetAllAsync()
        {
            return await _dbContext.Loans
                .AsNoTracking()
                .OrderBy(l => l.Id)
                .Select(l => new LoanDto
                {
                    Id = l.Id,
                    Amount = l.Amount,
                    CurrentBalance = l.CurrentBalance,
                    ApplicantName = l.ApplicantName,
                    Status = l.Status,
                    PaidAt = l.PaidAt.HasValue ? l.PaidAt.Value.ToString("o") : null
                })
                .ToListAsync();
        }

        public async Task<LoanDto?> GetByIdAsync(int id)
        {
            var loan = await _dbContext.Loans.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
            return loan is null ? null : ToDto(loan);
        }

        public async Task<LoanDto> CreateAsync(CreateLoanRequest request)
        {
            var loan = new Loan
            {
                Amount = request.Amount,
                CurrentBalance = request.CurrentBalance,
                ApplicantName = request.ApplicantName,
                Status = request.CurrentBalance <= 0 ? "paid" : "active"
            };

            _dbContext.Loans.Add(loan);
            await _dbContext.SaveChangesAsync();
            return ToDto(loan);
        }

        public async Task<LoanDto?> ProcessPaymentAsync(int id, decimal paymentAmount)
        {
            var loan = await _dbContext.Loans.FirstOrDefaultAsync(l => l.Id == id);
            if (loan is null)
            {
                return null;
            }

            loan.CurrentBalance = loan.CurrentBalance - paymentAmount;
            if (loan.CurrentBalance <= 0)
            {
                loan.CurrentBalance = 0m;
                loan.Status = "paid";
                loan.PaidAt = System.DateTime.UtcNow;
            }

            await _dbContext.SaveChangesAsync();
            return ToDto(loan);
        }

        private static LoanDto ToDto(Loan loan)
        {
            return new LoanDto
            {
                Id = loan.Id,
                Amount = loan.Amount,
                CurrentBalance = loan.CurrentBalance,
                ApplicantName = loan.ApplicantName,
                Status = loan.Status,
                PaidAt = loan.PaidAt?.ToString("o")
            };
        }
    }
}
