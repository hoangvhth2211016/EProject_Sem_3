using EProject_Sem_3.Models;
using EProject_Sem_3.Models.Recipes;
using EProject_Sem_3.Models.Users;

namespace EProject_Sem_3.Repositories.Recipes {
    public interface IRecipeRepo {
        Task Create(RecipeCreateDto dto);

        Task<ICollection<Recipe>> FindAll();

        Task<RecipeRes> FindById(int id, string? role = null);

        Task<Recipe> FindByIdBase(int id);

        Task<bool> RewardRecipe(int id);

        Task UpdateRecipe(int id, RecipeUpdateDto dto);

        Task DeleteRecipe(int id);

        Task<ICollection<RecipeCardRes>> ProcessPage(PaginationReq pageReq, IQueryable<Recipe> query);

        Task<int> CountRecord(IQueryable<Recipe> query);

        IQueryable<Recipe> FromAdmin();

        IQueryable<Recipe> FromUser();

        IQueryable<Recipe> ForPublic();

        IQueryable<Recipe> ForPublicWithSort(RecipeType type);

        IQueryable<Recipe> FromSelf(string username);

        Task DeleteRecipeImageById(int recipeId, int imageId);
    }
}
