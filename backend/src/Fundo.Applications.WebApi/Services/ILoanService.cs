using Fundo.Applications.WebApi.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fundo.Applications.WebApi.Services
{
    public interface ILoanService
    {
        Task<IEnumerable<LoanDto>> GetAllAsync();
        Task<LoanDto?> GetByIdAsync(int id);
        Task<LoanDto> CreateAsync(CreateLoanRequest request);
        Task<LoanDto?> ProcessPaymentAsync(int id, decimal paymentAmount);
    }
}
