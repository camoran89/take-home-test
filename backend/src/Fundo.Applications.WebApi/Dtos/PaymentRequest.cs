using System.ComponentModel.DataAnnotations;

namespace Fundo.Applications.WebApi.Dtos
{
    public class PaymentRequest
    {
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
    }
}
