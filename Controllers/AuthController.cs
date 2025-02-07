using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Simulando usuários com roles diferentes
        if (request.Username == "admin" && request.Password == "123456")
        {
            var token = _authService.GenerateJwtToken(request.Username, "Admin");
            return Ok(new { Token = token });
        }
        else if (request.Username == "user" && request.Password == "123456")
        {
            var token = _authService.GenerateJwtToken(request.Username, "User");
            return Ok(new { Token = token });
        }

        return Unauthorized("Usuário ou senha inválidos.");
    }
}
