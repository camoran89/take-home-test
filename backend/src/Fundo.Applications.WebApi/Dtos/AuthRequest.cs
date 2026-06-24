using System.ComponentModel.DataAnnotations;

namespace Fundo.Applications.WebApi.Dtos
{
    public class AuthRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
