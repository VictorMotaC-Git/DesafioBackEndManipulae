using Microsoft.AspNetCore.Mvc;
using DesafioBackEndManipulae.Models;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        await _userService.RegisterAsync(request.Username, request.Password, request.Role);
        return Ok("Usuário cadastrado com sucesso.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _userService.AuthenticateAsync(request.Username, request.Password);
        if (token == null)
            return Unauthorized("Usuário ou senha inválidos.");

        return Ok(new { Token = token });
    }
}

public class RegisterRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } // Admin ou User
}
