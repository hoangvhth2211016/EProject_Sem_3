using EProject_Sem_3.Models.OrderDetails;
using EProject_Sem_3.Models.Orders;

namespace EProject_Sem_3.Repositories.Orders;

public interface IOrderRepo
{

    Task<PageOrderRes> GetAllOrders(int page, int pageSize);

    Task<OrderRes> GetOrder(int orderId);
    
    Task<OrderRes> CreateOrder(OrderDto dto);

    Task<string> UpdateOrderStatus(int orderId, OrderStatus newStatus);
    
    Task<string> DeleteOrder(int orderId);
}