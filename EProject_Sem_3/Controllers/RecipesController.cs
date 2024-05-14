using System.ComponentModel.DataAnnotations;
using EProject_Sem_3.Models;
using EProject_Sem_3.Models.Recipes;
using EProject_Sem_3.Models.Users;
using EProject_Sem_3.Repositories.Recipes;
using EProject_Sem_3.Repositories.Users;
using EProject_Sem_3.Services.MailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EProject_Sem_3.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase {

        private readonly IRecipeRepo recipeRepo;

        private readonly IEmailSender emailSender;

        public RecipesController(IRecipeRepo recipeRepo,IEmailSender emailSender) {
            this.recipeRepo = recipeRepo;
            this.emailSender = emailSender;
        }


        /// <summary>
        /// get all recipes for public page except the candidate. Candidate recipes should be separated to another route.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllForPublic([FromQuery] PaginationReq pageReq) {
            var query = recipeRepo.ForPublic();
            var totalRecords = await recipeRepo.CountRecord(query);
            var list = await recipeRepo.ProcessPage(pageReq,query);
            return  Ok(new PaginationRes<RecipeCardRes>(pageReq.PageNo, pageReq.PerPage, totalRecords, list));
        }


        /// <summary>
        /// get all recipes, either from "Admin" or "User", otherwise return all recipes from the stores for testing purpose
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("Admin")]
        public async Task<IActionResult> GetAllForAdmin([FromQuery]string? from, [FromQuery] PaginationReq pageReq) {
            IQueryable<Recipe> query;
            if (from == "Admin")
            {
                query = recipeRepo.FromAdmin();
            }
            else if(from == "User")
            {
                query = recipeRepo.FromUser();
            }
            else
            {
                return Ok(await recipeRepo.FindAll());
            }
            var totalRecords = await recipeRepo.CountRecord(query);
            var list = await recipeRepo.ProcessPage(pageReq,query);
            return  Ok(new PaginationRes<RecipeCardRes>(pageReq.PageNo, pageReq.PerPage, totalRecords, list));
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
        /// create recipe as Admin
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAdminRecipe(RecipeCreateDto dto) {
            if (((dto.Type != RecipeType.free)) && (dto.Type !=RecipeType.premium)) { 
                return BadRequest("invalid recipe type");
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

            var recipe = await recipeRepo.FindByIdBase(id);
            
            //send email
            await emailSender.SendEmail(new MailTemplate
            {
                ToAddress = recipe.User.Email,
                Subject = "Ice Scream Parlour",
                Body = "<h1>Congratulations! Your recipe: <span style='color:red'>"+recipe.Title+" <span> won our award<h1>" +
                       ""+
                       "<div>We will send the reward to you as soon as possible!<div>" +
                       "<div>Thank you for contributing your ideas<div>"
            });
                
            return isRewarded ? Ok("Recipe rewarded") : BadRequest("Recipe is unable to reward or already rewarded");
        }

        /// <summary>
        /// delete recipe
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id) {
            await recipeRepo.DeleteRecipe(id);
            return Ok("Recipe deleted");
        }
        
        /// <summary>
        /// delete recipe image
        /// </summary>
        [Authorize]
        [HttpDelete("{recipeId}/Images/{imageId}")]
        public async Task<IActionResult> DeleteRecipe(int recipeId, int imageId) {
            await recipeRepo.DeleteRecipeImageById(recipeId, imageId);
            return Ok("Recipe image deleted");
        }
        
    }
}
