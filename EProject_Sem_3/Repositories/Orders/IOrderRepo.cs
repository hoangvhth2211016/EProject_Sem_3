using EProject_Sem_3.Models.OrderDetails;
using EProject_Sem_3.Models.Orders;

namespace EProject_Sem_3.Repositories.Orders;

public interface IOrderRepo
{
    
    Task<List<OrderRes>> GetAllOrders();

    Task<OrderRes> GetOrder(int orderId);
    
    Task<OrderRes> CreateOrder(OrderDto dto);

    Task<OrderRes> UpdateOrder(int orderId, UpdateOrderDto orderDto);
    
    Task<string> DeleteOrder(int orderId);
}