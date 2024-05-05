using System.ComponentModel.DataAnnotations;

namespace EProject_Sem_3.Models.Users;

public class LoginDto
{
    [Required]
    public required string Username { get; set; }
    
    [Required]
    public required string Password { get; set; }
}