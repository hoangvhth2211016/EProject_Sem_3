using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EProject_Sem_3.Models.BookImages;

namespace EProject_Sem_3.Models.Books;

public class BookDto
{
    [Required]
    public required string ISBN { get; set; }

    [Required]
    public required string Title { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public required decimal Price { get; set; }

    [Required]
    public int Stock { get; set; } = 0;

    [Required]
    public required string Description { get; set; }
    
}