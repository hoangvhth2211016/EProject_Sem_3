using AutoMapper;
using EProject_Sem_3.Exceptions;
using EProject_Sem_3.Models;
using EProject_Sem_3.Models.RecipeImages;
using EProject_Sem_3.Models.Recipes;
using EProject_Sem_3.Services.FileService;
using Microsoft.EntityFrameworkCore;

namespace EProject_Sem_3.Repositories.RecipeImages;

public class RecipeImageRepo : IRecipeImageRepo
{

    private readonly IFileService fileService;

    private readonly IMapper mapper;

    private readonly AppDbContext context;

    public RecipeImageRepo(IFileService fileService, IMapper mapper, AppDbContext context) {
        this.fileService = fileService;
        this.mapper = mapper;
        this.context = context;
    }
    
    public async Task Create(Recipe recipe, ICollection<IFormFile> files) {
        foreach (var file in files)
        {
            var name = recipe.Id + "_" + Guid.NewGuid().ToString();

            var model = new FileModel() {
                Path = "recipes",
                File = file,
                Name = name
            };
            var url = await fileService.Upload(model);
            var recipeImageCreateDto = new RecipeImageCreateDto() {
                RecipeId = recipe.Id,
                Image = url,
                Name = name
            };
            await context.RecipeImages.AddAsync(mapper.Map<RecipeImage>(recipeImageCreateDto));
        }

        await context.SaveChangesAsync();
    }

    public async Task Delete(int id) {
        var image = await FindById(id);
        await fileService.Delete(image.Name);
        context.RecipeImages.Remove(image);
        await context.SaveChangesAsync();
    }

    public async Task<RecipeImage> FindById(int id) {
        return await context.RecipeImages.FirstOrDefaultAsync(i => i.Id == id) ??
               throw new NotFoundException("Image not found");
    }
}