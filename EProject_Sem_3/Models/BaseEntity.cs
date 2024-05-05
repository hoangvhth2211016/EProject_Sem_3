using System.ComponentModel.DataAnnotations;

namespace EProject_Sem_3.Models;

public abstract class BaseEntity
{
    [Key]
    public required int Id { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }
    
}