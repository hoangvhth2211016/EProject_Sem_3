using System.ComponentModel.DataAnnotations;

namespace EProject_Sem_3.Models.Recipes {
    public class RecipeCreateDto {

        [Required]
        public required int UserId { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public required string Ingredients { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public required string Description { get; set; }

        public RecipeType Type { get; set; }
    }
}
