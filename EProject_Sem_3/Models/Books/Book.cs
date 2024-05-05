using EProject_Sem_3.Models.BookImages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EProject_Sem_3.Models.Books {
    public class Book : BaseEntity {
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

        public List<BookImage> BookImages { get; set; } = [];
    }
}
