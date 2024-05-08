using EProject_Sem_3.Repositories.Orders;
using EProject_Sem_3.Repositories.OrdersDetail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EProject_Sem_3.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderDetailController : ControllerBase
{
    private readonly IOrderDetailRepo _orderDetail;


    public OrderDetailController(IOrderDetailRepo orderDetail)
    {
        _orderDetail = orderDetail;
    }


    //get all orders detail
    [HttpGet("{orderId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllByOrder(int orderId)
    {
        return Ok(await _orderDetail.GetAllByOrder(orderId));
    }
    

}