using System.ComponentModel.DataAnnotations;

namespace Fundo.Applications.WebApi.Dtos
{
    public class CreateLoanRequest
    {
        [Required]
        [Range(1, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal CurrentBalance { get; set; }

        [Required]
        [StringLength(200)]
        public string ApplicantName { get; set; } = string.Empty;
    }
}
