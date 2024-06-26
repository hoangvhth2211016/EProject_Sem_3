using EProject_Sem_3.Models;
using EProject_Sem_3.Models.Orders;
using EProject_Sem_3.Repositories.Books;
using EProject_Sem_3.Repositories.Orders;
using EProject_Sem_3.Repositories.OrdersDetail;
using EProject_Sem_3.Services.VnpayService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EProject_Sem_3.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderRepo _orderRepo;

    private readonly IOrderDetailRepo _orderDetailRepo;
    
    private readonly IVnPayService _vnPayService;

    public OrderController(IOrderRepo orderRepo,
                            IOrderDetailRepo orderDetailRepo,
                            IVnPayService vnPayService)
    {
        _orderDetailRepo = orderDetailRepo;
        _orderRepo = orderRepo;
        _vnPayService = vnPayService;
    }


    //get all orders
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllOrders([FromQuery] PaginationReq pageReq)
    {
        return Ok(await _orderRepo.GetAllOrders(pageReq));
    }
    
    //get order
    [HttpGet("{orderId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetOrder(int orderId)
    {
        return Ok(await _orderRepo.GetOrder(orderId));
    }

    
    
    [HttpPut("{orderId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateOrderStatus(int orderId,[FromBody] OrderStatus newStatus)
    {
        
        return Ok(await _orderRepo.UpdateOrderStatus(orderId,newStatus));
    }
    
    [HttpDelete("{orderId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteOrder(int orderId)
    {
        _orderRepo.DeleteOrder(orderId);
        return Ok("The order had been Deleted");
    }
    
    
    //get all orders detail
    [HttpGet("{orderId}/OrdersDetais")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllByOrder(int orderId)
    {
        return Ok(await _orderDetailRepo.GetAllByOrder(orderId));
    }
}