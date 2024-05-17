namespace EProject_Sem_3.Models.Recipes;

public class RecipeCardRes
{
    public int Id { get; set; }

    public int UserId { get; set; }
    
    public  string ShortDescription {get; set;}
    
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    
    public string Title { get; set; }
    
    public RecipeType Type { get; set; }
    
    public string Thumbnail { get; set; }
    
    public string AuthorName { get; set; }
    
}