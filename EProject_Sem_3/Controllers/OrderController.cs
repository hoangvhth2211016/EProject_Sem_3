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
    public async Task<IActionResult> CreateOrder(OrderDto dto)
    {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        // save order to db
        var order = await _orderRepo.CreateOrder(dto);
        
        // save orderDetail to db
        await _orderDetailRepo.CreateOrderDetail(order.Id, dto);
        
        
        // create payment url
        var vnPayModel = new VnPaymentOrderRequestModel()
        {
            TotalAmount = dto.TotalAmount,
            OrderId = order.Id,
            Phone = order.Phone
        };
        
        // return url
        return Ok(_vnPayService.CreatePaymentUrlForOrder(HttpContext, vnPayModel));
        
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