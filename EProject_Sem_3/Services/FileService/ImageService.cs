using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EProject_Sem_3.Models;

namespace EProject_Sem_3.Services.FileService;

public class ImageService : IFileService
{

    private Cloudinary cloudinary;

    private readonly IConfiguration config;

    private readonly string rootFolder = "icecream/";

    public ImageService(IConfiguration config) {
        this.config = config;
        var section = config.GetSection("cloudinary");
        cloudinary = new Cloudinary(new Account(
                section.GetSection("cloudName").Value,
                section.GetSection("key").Value,
                section.GetSection("secret").Value
            ));
    }
    
    public async Task<string> Upload(FileModel model) {
        await using var stream = model.File.OpenReadStream();
        var uploadParam = new ImageUploadParams() {
            PublicId = model.Name,
            Folder = rootFolder+model.Path,
            Overwrite = true,
            File = new FileDescription(model.File.Name, stream)
        };
        var result = await cloudinary.UploadAsync(uploadParam);
        return result.Url.ToString();
    }

    public async Task Delete(string name) {
        await cloudinary.DestroyAsync(new DeletionParams(name));
    }


}