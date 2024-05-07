using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EProject_Sem_3.Models.Books;
using EProject_Sem_3.Models.Orders;

namespace EProject_Sem_3.Models.OrderDetails;

public class OrderDetailDto
{

    [Required]
    public required int BookId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public required decimal PurchasePrice { get; set; }

    [Required]
    public required int Quantity { get; set; }
    
}