using EProject_Sem_3.Models.OrderDetails;
using EProject_Sem_3.Models.Orders;


namespace EProject_Sem_3.Repositories.OrdersDetail;

public interface IOrderDetailRepo
{

    Task<List<OrderDetailRes>> GetAllByOrder(int orderId);

    Task CreateOrderDetail(int orderId, OrderDto dto);

    // void UpdateOrderDetail(int orderId, UpdateOrderDto dto);
}
