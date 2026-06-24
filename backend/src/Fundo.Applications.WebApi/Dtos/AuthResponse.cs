namespace Fundo.Applications.WebApi.Dtos
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public int ExpiresInMinutes { get; set; }
    }
}
