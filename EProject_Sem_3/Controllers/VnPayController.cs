using EProject_Sem_3.Exceptions;
using EProject_Sem_3.Models;
using EProject_Sem_3.Models.Orders;
using EProject_Sem_3.Repositories.Orders;
using EProject_Sem_3.Repositories.OrdersDetail;
using EProject_Sem_3.Services.VnpayService;
using Microsoft.AspNetCore.Mvc;

namespace EProject_Sem_3.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VnPayController : ControllerBase
{
    private readonly IVnPayService _vnPayService;
    private readonly IOrderRepo _orderRepo;


    public VnPayController(IVnPayService vnPayService,IOrderRepo orderRepo)
    {
        _vnPayService = vnPayService;
        
        _orderRepo = orderRepo;
    }
    

    [HttpGet("PaymentCallBack")]
    public async Task<IActionResult> PaymentCallBack()
    {
        var respronse = _vnPayService.PaymentExecute(Request.Query);
        
        // get orderId from response
        var orderId = respronse.OrderId.Substring(0, respronse.OrderId.Length - 5);
        
        // if VnPayResponseCode != 00 -> payment fail
        if (respronse is not { VnPayResponseCode: "00" })
        {
            // if payment fail -> delete order 
            _orderRepo.DeleteOrder(int.Parse(orderId));
            throw new BadRequestException("Payment Fail");
        }
        
       
        
        return Ok("Payment Success! OrderId: " + orderId);

    }
    
}