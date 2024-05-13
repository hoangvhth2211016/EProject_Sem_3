using EProject_Sem_3.Models.Orders;

namespace EProject_Sem_3.Models.OrderDetails;

public class PageOrderRes
{
    public required List<OrderRes> Orders { get; set; } = [];

    public required int Page { get; set; }

    public required int LastPage { get; set; }

    public required int TotalOrders { get; set; }
    
    public required int PerPage { get; set; }
}