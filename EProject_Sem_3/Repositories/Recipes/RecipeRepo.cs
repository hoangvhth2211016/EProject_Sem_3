using AutoMapper;
using EProject_Sem_3.Exceptions;
using EProject_Sem_3.Models;
using EProject_Sem_3.Models.Recipes;
using EProject_Sem_3.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace EProject_Sem_3.Repositories.Recipes {
    public class RecipeRepo : IRecipeRepo {

        private readonly AppDbContext context;

        private readonly IMapper mapper;

        public RecipeRepo(AppDbContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        // create Recipe
        public async Task Create(RecipeCreateDto dto) {
            Recipe recipe = mapper.Map<Recipe>(dto);
            await context.Recipes.AddAsync(recipe);
            await context.SaveChangesAsync();
        }


        // find recipes with scenarios
        public async Task<ICollection<object>> FindAll(string flag, string username = null) {
            var query = context.Recipes.AsQueryable();

            if (flag == "public") {
                query = query.Where(r => r.Type != RecipeType.candidate);
            }
            else if (flag == "admin") {
                query = query.Where(r => r.UserId == 1);
            }
            else if (flag == "user") {
                query = query.Where(r => r.Type == RecipeType.winner || r.Type == RecipeType.candidate);
            }
            else if (username != null && flag == "self") {
                query = query.Where(r => r.User.Username == username);

            }

            return await query
                .Select(r => new {
                    r.Id,
                    r.UserId,
                    r.CreatedAt,
                    r.UpdatedAt,
                    r.Title,
                    r.Type,
                    r.Thumbnail
                })
                .ToListAsync<object>();
        }

        // find by recipe id with scenario
        public async Task<RecipeRes> FindById(int id, string? role = null) {

            var query = context.Recipes.Where(r => id == r.Id);

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

        public async Task<bool> RewardRecipe(int id) {
            var recipe = await FindById(id);
            if (!recipe.Type.Equals(RecipeType.candidate)) {
                return false;
            }
            recipe.Type = RecipeType.winner;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateRecipe(int id, RecipeUpdateDto dto) {
            var recipe = await FindById(id);
            if (recipe.Type.Equals(RecipeType.winner)) {
                return;
            }
            mapper.Map(dto, recipe);
            await context.SaveChangesAsync();
        }


        public async Task DeleteRecipe(int id) {
            var recipe = await FindById(id);
            context.Remove(recipe);
            await context.SaveChangesAsync();
        }
    }
}
