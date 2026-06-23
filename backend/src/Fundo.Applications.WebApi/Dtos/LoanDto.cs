namespace Fundo.Applications.WebApi.Dtos
{
    public class LoanDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal CurrentBalance { get; set; }
        public string ApplicantName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? PaidAt { get; set; }
    }
}
