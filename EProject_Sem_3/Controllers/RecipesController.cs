using EProject_Sem_3.Models.Recipes;
using EProject_Sem_3.Models.Users;
using EProject_Sem_3.Repositories.Recipes;
using EProject_Sem_3.Repositories.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EProject_Sem_3.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase {

        private readonly IRecipeRepo recipeRepo;

        public RecipesController(IRecipeRepo recipeRepo) {
            this.recipeRepo = recipeRepo;
        }


        /// <summary>
        /// get all recipes for public page except the candidate. Candidate recipes should be separated to another route.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllForPublic() {
            return Ok(await recipeRepo.FindAll("public"));
        }


        /// <summary>
        /// get all recipes from admin, which mean, only recipes with type of free or premium
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("Admin")]
        public async Task<IActionResult> GetAllFromAdmin(string from) {
            return Ok(await recipeRepo.FindAll(from));
        }


        /// <summary>
        /// get recipe by id, accept access for all user but with conditions to view
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) {
            if (!User.Identity.IsAuthenticated) {
                return Ok(await recipeRepo.FindById(id, "Anonymous"));
            }
            var role = User.FindFirst("Role")?.Value;
            return Ok(await recipeRepo.FindById(id, role));
        }


        /// <summary>
        /// create recipe
        /// </summary>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAsUser(RecipeCreateDto dto) {
            var role = User.FindFirst("Role")?.Value;
            switch (role) {
                case "Admin":
                    string type = dto.Type.ToString();
                    if ((type != "free") && (type != "premium")) {
                        return BadRequest("invalid recipe type");
                    }
                    break;
                case "User":
                    dto.Type = RecipeType.candidate;
                    break;
                default:
                    return Unauthorized();
            }
            var id = Convert.ToInt16(User.FindFirst("Id")?.Value);
            dto.UserId = id;
            await recipeRepo.Create(dto);
            return Ok("Recipe created");
        }


        /// <summary>
        /// update recipe
        /// </summary>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, RecipeUpdateDto dto) {
            await recipeRepo.UpdateRecipe(id, dto);
            return Ok("Recipe updated");
        }


        /// <summary>
        /// reward user's recipe, change from candidate to user
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> RewardRecipe(int id) {
            var isRewarded = await recipeRepo.RewardRecipe(id);
            return isRewarded ? Ok("Recipe rewarded") : BadRequest("Recipe is unable to reward or already rewarded");
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id) {
            await recipeRepo.DeleteRecipe(id);
            return Ok("Recipe deleted");
        }
    }
}
