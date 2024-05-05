using EProject_Sem_3.Models.Books;
using EProject_Sem_3.Models.Orders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EProject_Sem_3.Models.OrderDetails {
    public class OrderDetail : BaseEntity {
        [Required]
        public required int OrderId { get; set; }

        [Required]
        public required int BookId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public required decimal PurchasePrice { get; set; }

        [Required]
        public required int Quantity { get; set; }

        public required Order Order { get; set; }

        public required Book Book { get; set; }

    }
}
