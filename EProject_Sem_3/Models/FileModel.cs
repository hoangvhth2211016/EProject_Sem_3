namespace EProject_Sem_3.Models;

public class FileModel
{
    public required string Path { get; set; }

    public required IFormFile File { get; set; }

    public required string Name { get; set; }
}