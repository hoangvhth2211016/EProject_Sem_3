using EProject_Sem_3.Models.RecipeImages;
using EProject_Sem_3.Models.Recipes;

namespace EProject_Sem_3.Repositories.RecipeImages;

public interface IRecipeImageRepo
{
    Task Create(Recipe recipe, ICollection<IFormFile> files);

    Task Delete(int id);

    Task<RecipeImage> FindById(int id);
}