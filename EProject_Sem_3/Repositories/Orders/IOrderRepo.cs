using EProject_Sem_3.Models;
using EProject_Sem_3.Models.OrderDetails;
using EProject_Sem_3.Models.Orders;

namespace EProject_Sem_3.Repositories.Orders;

public interface IOrderRepo
{

    Task<PaginationRes<OrderRes>> GetAllOrders(PaginationReq pageReq);

    Task<OrderRes> GetOrder(int orderId);
    
    Task<OrderRes> CreateOrder(OrderDto dto);

    Task<string> UpdateOrderStatus(int orderId, OrderStatus newStatus);
    
    void DeleteOrder(int orderId);
}