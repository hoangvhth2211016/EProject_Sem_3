using EProject_Sem_3.Models.Recipes;
using EProject_Sem_3.Models.Users;
using EProject_Sem_3.Repositories.Recipes;
using EProject_Sem_3.Repositories.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EProject_Sem_3.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {

        private readonly IUserRepo userRepo;

        private readonly IRecipeRepo recipeRepo;

        public UsersController(IUserRepo userRepo, IRecipeRepo recipeRepo) {
            this.userRepo = userRepo;
            this.recipeRepo = recipeRepo;
        }


        /// <summary>
        /// get current user information
        /// </summary>
        [Authorize]
        [HttpGet("Current")]
        public async Task<IActionResult> GetCurrentUser() {
            var name = User.Identity.Name;
            return Ok(await userRepo.FindByUsername(name));
        }


        /// <summary>
        /// get all users, only for admin
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            return Ok(await userRepo.FindAll());
        }


        /// <summary>
        /// get user by id, only for admin
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) {
            return Ok(await userRepo.FindById(id));
        }


        /// <summary>
        /// update user information
        /// </summary>
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserUpdateDto dto) {
            var user = await userRepo.FindByUsername(User.Identity.Name);
            var updatedUser = await userRepo.UpdateUser(user, dto);
            return Ok(updatedUser);
        }

        /// <summary>
        /// Change user Password
        /// </summary>
        [Authorize]
        [HttpPatch("Password/Change")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto) {
            var user = await userRepo.FindByUsername(User.Identity.Name);
            await userRepo.ChangePassword(user, dto);
            return Ok("password changed");
        }

        /// <summary>
        /// Get Current User Recipes
        /// </summary>
        [Authorize]
        [HttpGet("Recipes")]
        public async Task<IActionResult> GetRecipesFromCurrentUser() {
            var username = User.Identity.Name;
            return Ok(await recipeRepo.FindAll("self", username));
        }

        /// <summary>
        /// Update User Recipe, can only be change when recipe's type is still candidate
        /// </summary>
        [Authorize(Roles = "User")]
        [HttpPut("Recipes/{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, RecipeUpdateDto dto) {
            await recipeRepo.UpdateRecipe(id, dto);
            return Ok("recipe updated");
        }
    }
}
