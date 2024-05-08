using EProject_Sem_3.Models.Orders;
using EProject_Sem_3.Repositories.Books;
using EProject_Sem_3.Repositories.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EProject_Sem_3.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderRepo _orderRepo;


    public OrderController(IOrderRepo orderRepo)
    {
        _orderRepo = orderRepo;
    }


    //get all orders
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllOrders(int page = 1, int pageSize = 10)
    {
        return Ok(await _orderRepo.GetAllOrders(page,pageSize));
    }
    
    //get order
    [HttpGet("{orderId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetOrder(int orderId)
    {
        return Ok(await _orderRepo.GetOrder(orderId));
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(OrderDto dto)
    {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        
        return Ok(await _orderRepo.CreateOrder(dto));
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
        return Ok(await _orderRepo.DeleteOrder(orderId));
    }
}