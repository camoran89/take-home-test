using Fundo.Applications.WebApi.Dtos;
using Fundo.Applications.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Fundo.Applications.WebApi.Controllers
{
    [ApiController]
    [Route("api/loans")]
    [Authorize]
    public class LoanManagementController : ControllerBase
    {
        private readonly ILoanService _loanService;
        private readonly ILogger<LoanManagementController> _logger;

        public LoanManagementController(ILoanService loanService, ILogger<LoanManagementController> logger)
        {
            _loanService = loanService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all loans");
            var loans = await _loanService.GetAllAsync();
            return Ok(loans);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching loan with id {LoanId}", id);
            var loan = await _loanService.GetByIdAsync(id);
            if (loan is null)
            {
                _logger.LogWarning("Loan with id {LoanId} not found", id);
                return NotFound();
            }

            return Ok(loan);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateLoanRequest request)
        {
            _logger.LogInformation("Creating a new loan for applicant {ApplicantName}", request.ApplicantName);
            var loan = await _loanService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = loan.Id }, loan);
        }

        [HttpPost("{id:int}/payment")]
        public async Task<ActionResult> MakePayment(int id, [FromBody] PaymentRequest request)
        {
            _logger.LogInformation("Processing payment of {Amount} for loan {LoanId}", request.Amount, id);
            var loan = await _loanService.ProcessPaymentAsync(id, request.Amount);
            if (loan is null)
            {
                _logger.LogWarning("Payment requested for missing loan {LoanId}", id);
                return NotFound();
            }

            return Ok(loan);
        }
    }
}
