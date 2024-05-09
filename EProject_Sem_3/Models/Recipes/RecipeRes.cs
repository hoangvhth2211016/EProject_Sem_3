using EProject_Sem_3.Models.RecipeImages;

namespace EProject_Sem_3.Models.Recipes {
    public class RecipeRes {

        public int Id { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; }

        public string Ingredients { get; set; }

        public string Description { get; set; }

        public RecipeType Type { get; set; }

        public string Thumbnail { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<RecipeImageRes> Images { get; set; }

    }
}
