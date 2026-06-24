namespace Fundo.Applications.WebApi.Services
{
    public interface ITokenService
    {
        string GenerateToken(string username);
    }
}
