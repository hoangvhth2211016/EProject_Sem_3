using EProject_Sem_3.Models;

namespace EProject_Sem_3.Services.FileService;

public interface IFileService
{
    Task<string> Upload(FileModel model);
    
    Task Delete(string name);

}