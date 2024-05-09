using System.ComponentModel.DataAnnotations;

namespace EProject_Sem_3.Models.Users;

public class RegisterDto
{
    [Required]
    public required string Name { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }

    [Required]
    public required string Username { get; set; }
}