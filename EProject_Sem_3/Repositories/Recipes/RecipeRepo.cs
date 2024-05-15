using AutoMapper;
using EProject_Sem_3.Exceptions;
using EProject_Sem_3.Models;
using EProject_Sem_3.Models.Recipes;
using EProject_Sem_3.Models.Users;
using EProject_Sem_3.Repositories.RecipeImages;
using EProject_Sem_3.Services.FileService;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EProject_Sem_3.Repositories.Recipes {
    public class RecipeRepo : IRecipeRepo {

        private readonly AppDbContext context;

        private readonly IMapper mapper;

        private readonly IRecipeImageRepo recipeImageRepo;

        private readonly IFileService imageService;

        public RecipeRepo(AppDbContext context, IMapper mapper, IRecipeImageRepo recipeImageRepo, IFileService imageService) {
            this.context = context;
            this.mapper = mapper;
            this.recipeImageRepo = recipeImageRepo;
            this.imageService = imageService;
        }

        // create Recipe
        public async Task Create(RecipeCreateDto dto) {
            Recipe recipe = mapper.Map<Recipe>(dto);
            var entityEntry = await context.Recipes.AddAsync(recipe);
            await context.SaveChangesAsync();
            var newRecipe = entityEntry.Entity;
            if (!dto.Files.IsNullOrEmpty())
            {
                await recipeImageRepo.Create(newRecipe, dto.Files);
            }
            if (dto.Thumbnail != null)
            {
                string name = recipe.Id + "_thumbnail";
                string image = await imageService.Upload(new FileModel() { 
                    File = dto.Thumbnail,
                    Name = name,
                    Path = "recipes"
                });
                newRecipe.Thumbnail = image;
                await context.SaveChangesAsync();
            }
        }


        // find all recipes
        public async Task<ICollection<Recipe>> FindAll() {
            return await context.Recipes.ToListAsync();
        }

        // find by recipe id with scenario
        public async Task<RecipeRes> FindById(int id, string? role = null) {

            var query = context.Recipes
                .Include(r => r.Images)
                .Where(r => id == r.Id);

            if (role == "Anomymous") {
                // If role is not specified (free user) then user can only view free recipes
                query = query.Where(r => r.Type == RecipeType.free);
            }
            else if (role == "User") {
                // If role is User, then user can view every types of recipes, except the candidate one
                query = query.Where(r => r.Type != RecipeType.candidate);
            }

            var recipe = await query.FirstOrDefaultAsync();

            return recipe == null ? throw new NotFoundException("Recipe not found") : mapper.Map<RecipeRes>(recipe);
        }

        public async Task<Recipe> FindByIdBase(int id) {
            return await context.Recipes.FirstOrDefaultAsync(r => r.Id == id) ?? throw new NotFoundException("Recipe Not Found");
        }

        public async Task<bool> RewardRecipe(int id) {
            var recipe = await FindByIdBase(id);
            if (recipe.Type != RecipeType.candidate) {
                return false;
            }
            recipe.Type = RecipeType.winner;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateRecipe(int id, RecipeUpdateDto dto) {
            var recipe = await FindByIdBase(id);
            if (recipe.Type.Equals(RecipeType.winner)) {
                return;
            }
            mapper.Map(dto, recipe);
            if (dto.Thumbnail != null)
            {
                string name = recipe.Id + "_thumbnail";
                string image = await imageService.Upload(new FileModel() { 
                    File = dto.Thumbnail,
                    Name = name,
                    Path = "recipes"
                });
                recipe.Thumbnail = image;
            }
            await context.SaveChangesAsync();
            if(!dto.Files.IsNullOrEmpty())
                await recipeImageRepo.Create(recipe, dto.Files);
        }


        public async Task DeleteRecipe(int id) {
            var recipe = await FindByIdBase(id);
            context.Remove(recipe);
            await context.SaveChangesAsync();
        }



        public async Task<ICollection<RecipeCardRes>> ProcessPage(PaginationReq pageReq, IQueryable<Recipe> query) {
            var list = await query
                .OrderBy(r => r.Id)
                .Skip((pageReq.PageNo - 1) * pageReq.PerPage)
                .Take(pageReq.PerPage)
                .Select(r => new RecipeCardRes {
                    Id = r.Id,
                    UserId = r.UserId,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt,
                    Title = r.Title,
                    Type = r.Type,
                    Thumbnail = r.Thumbnail
                })
                .ToListAsync();

            return list;
        }
        
        public async Task DeleteRecipeImageById(int recipeId, int imageId) {
            var isImageExist = await context.RecipeImages
                .AnyAsync(r => r.RecipeId == recipeId && r.Id == imageId);
            if (isImageExist)
            {
                await recipeImageRepo.Delete(imageId);
            }
        }

        public async Task DeleteRecipeThumbnail(int recipeId) {
            var recipe = await FindByIdBase(recipeId);
            await imageService.Delete("recipes/"+recipeId+"_thumbnail");
            recipe.Thumbnail = null;
            await context.SaveChangesAsync();
        }
        

        public IQueryable<Recipe> FromAdmin() {
            return context.Recipes.Include(r => r.User).Where(r => r.User.Role.Equals(Role.Admin));
        }

        public IQueryable<Recipe> FromUser() {
            return context.Recipes.Include(r => r.User).Where(r => r.User.Role.Equals(Role.User));
        }

        public IQueryable<Recipe> ForPublic() {
            return context.Recipes.Where(r => !r.Type.Equals(RecipeType.candidate));
        }

        public IQueryable<Recipe> ForPublicWithSort(RecipeType type) {
            return context.Recipes.Where(r => r.Type.Equals(type));
        }

        public IQueryable<Recipe> FromSelf(string username) {
            return context.Recipes.Include(r => r.User).Where(r => r.User.Username == username);
        }


        public async Task<int> CountRecord(IQueryable<Recipe> query) {
            return await query.CountAsync();
        }
        
      
    }
}
