using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EProject_Sem_3.Models.RecipeImages;
using EProject_Sem_3.Models.Users;

namespace EProject_Sem_3.Models.Recipes {
    public class Recipe : BaseEntity {

        [Required]
        public required int UserId { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Ingredients { get; set; }

        [Required]
        public required string Description { get; set; }

        public RecipeType Type { get; set; }

        public string? Thumbnail { get; set; }

        public List<RecipeImage> Images { get; set; } = [];

        public required User User { get; set; }

    }
}
