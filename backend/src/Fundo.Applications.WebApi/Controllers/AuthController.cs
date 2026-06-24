using Fundo.Applications.WebApi.Dtos;
using Fundo.Applications.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fundo.Applications.WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ITokenService tokenService, ILogger<AuthController> logger)
        {
            _tokenService = tokenService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<AuthResponse> Login([FromBody] AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request.Username != "testuser" || request.Password != "Password123")
            {
                _logger.LogWarning("Invalid login attempt for user {Username}", request.Username);
                return Unauthorized(new { error = "Invalid credentials" });
            }

            var token = _tokenService.GenerateToken(request.Username);
            _logger.LogInformation("User {Username} successfully authenticated", request.Username);
            return Ok(new AuthResponse { Token = token, ExpiresInMinutes = 525600 });
        }
    }
}
