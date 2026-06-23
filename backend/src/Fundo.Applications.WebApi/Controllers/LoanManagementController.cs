using Fundo.Applications.WebApi.Dtos;
using Fundo.Applications.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fundo.Applications.WebApi.Controllers
{
    [ApiController]
    [Route("api/loans")]
    public class LoanManagementController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoanManagementController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var loans = await _loanService.GetAllAsync();
            return Ok(loans);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            var loan = await _loanService.GetByIdAsync(id);
            if (loan is null)
            {
                return NotFound();
            }

            return Ok(loan);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateLoanRequest request)
        {
            var loan = await _loanService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = loan.Id }, loan);
        }

        [HttpPost("{id:int}/payment")]
        public async Task<ActionResult> MakePayment(int id, [FromBody] PaymentRequest request)
        {
            var loan = await _loanService.ProcessPaymentAsync(id, request.Amount);
            if (loan is null)
            {
                return NotFound();
            }

            return Ok(loan);
        }
    }
}
