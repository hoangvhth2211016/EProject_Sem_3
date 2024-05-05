using EProject_Sem_3.Models.Subscriptions;
using System.ComponentModel.DataAnnotations;

namespace EProject_Sem_3.Models.Users;

public class User : BaseEntity {

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Username { get; set; }

    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }

    public Role Role { get; set; }

    [Phone]
    public string? Phone { get; set; }

    public string? Avatar { get; set; }

    public List<Subscription> Subscriptions { get; set; } = [];

}