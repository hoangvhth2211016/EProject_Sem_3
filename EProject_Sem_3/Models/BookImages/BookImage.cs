using EProject_Sem_3.Models.Books;
using System.ComponentModel.DataAnnotations;

namespace EProject_Sem_3.Models.BookImages {
    public class BookImage : BaseEntity {

        [Required]
        public required int BookId { get; set; }

        [Required]
        public required string Image { get; set; }

        public string? Name { get; set; }

        public required Book Book { get; set; }
    }
}
