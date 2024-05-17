using EProject_Sem_3.Models.Recipes;
using System.ComponentModel.DataAnnotations;

namespace EProject_Sem_3.Models.RecipeImages {
    public class RecipeImage : BaseEntity {

        [Required]
        public int RecipeId { get; set; }

        [Required]
        public required string Image { get; set; }

        [Required]
        public required string Name { get; set; }

        public Recipe Recipe { get; set; }

    }
}
