using System.ComponentModel.DataAnnotations;

namespace EProject_Sem_3.Models.RecipeImages;

public class RecipeImageCreateDto
{
    [Required]
    public int RecipeId { get; set; }

    [Required]
    public required string Image { get; set; }

    [Required]
    public required string Name { get; set; }
    
}