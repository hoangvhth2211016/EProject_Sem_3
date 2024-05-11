using EProject_Sem_3.Models.Recipes;
using EProject_Sem_3.Models.Users;

namespace EProject_Sem_3.Repositories.Recipes {
    public interface IRecipeRepo {
        Task Create(RecipeCreateDto dto);

        Task<ICollection<object>> FindAll(string flag, string? username = null);

        Task<RecipeRes> FindById(int id, string? role = null);

        Task<bool> RewardRecipe(int id);

        Task UpdateRecipe(int id, RecipeUpdateDto dto);

        Task DeleteRecipe(int id);
    }
}
