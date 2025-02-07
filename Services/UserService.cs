using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly AuthService _authService;

    public UserService(IUserRepository userRepository, AuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    public async Task<bool> RegisterAsync(string username, string password, string role)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(username);
        if (existingUser != null)
            throw new ArgumentException("Usuário já existe.");

        string passwordHash = HashPassword(password);
        var user = new User { Username = username, PasswordHash = passwordHash, Role = role };

        await _userRepository.AddAsync(user);
        return true;
    }

    public async Task<string> AuthenticateAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null || !VerifyPassword(password, user.PasswordHash))
            return null;

        return _authService.GenerateJwtToken(user.Username, user.Role);
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        return HashPassword(password) == storedHash;
    }
}
