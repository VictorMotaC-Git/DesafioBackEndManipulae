using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string PasswordHash { get; set; } // 🔐 Armazena a senha criptografada

    [Required]
    public string Role { get; set; } // Admin ou User
}
