using System.ComponentModel.DataAnnotations;

namespace EProject_Sem_3.Models.Recipes {
    public class RecipeUpdateDto {
        [Required]
        public required string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public required string Ingredients { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public required string Description { get; set; }
        
        public ICollection<IFormFile>? Files { get; set; } = [];

        public RecipeType Type { get; set; }
    }
}
