using EProject_Sem_3.Models.BookImages;

namespace EProject_Sem_3.Models.Books;

public class BookRes
{
    public required int Id { get; set; }
    
    public required string ISBN { get; set; }
    
    public required string Title { get; set; }
    
    public decimal Price { get; set; }
    
    public int Stock { get; set; } 
    
    public required string Description { get; set; }
    
    public List<BookImageRes> BookImages { get; set; } = [];
}