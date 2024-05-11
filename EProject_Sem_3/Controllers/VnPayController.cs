using EProject_Sem_3.Exceptions;
using EProject_Sem_3.Models;
using EProject_Sem_3.Models.Orders;
using EProject_Sem_3.Repositories;
using EProject_Sem_3.Repositories.Orders;
using EProject_Sem_3.Repositories.OrdersDetail;
using EProject_Sem_3.Repositories.Users;
using EProject_Sem_3.Services.VnpayService;
using Microsoft.AspNetCore.Mvc;

namespace EProject_Sem_3.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VnPayController : ControllerBase
{
    private readonly IVnPayService _vnPayService;
    private readonly IOrderRepo _orderRepo;
    private readonly IUserRepo _userRepo;
    private readonly ISubscriptionRepo _subscriptionRepo;


    public VnPayController(IVnPayService vnPayService,
                            IOrderRepo orderRepo,
                            IUserRepo userRepo,
                            ISubscriptionRepo subscriptionRepo)
    {
        _vnPayService = vnPayService;
        _userRepo = userRepo;
        _userRepo = userRepo;
        _orderRepo = orderRepo;
        _subscriptionRepo = subscriptionRepo;
    }
    

    [HttpGet("PaymentCallBackForOrder")]
    public async Task<IActionResult> PaymentCallBackForOrder()
    {
        var respronse = _vnPayService.PaymentExecute(Request.Query);
        
        
        // if CheckValid != true -> payment invalid
        if (respronse is not { CheckValid: true  } )
        {
            throw new BadRequestException("Payment invalid");
        }
        
        // if VnPayResponseCode != 00 -> payment fail
        if (respronse is not { VnPayResponseCode: "00" } )
        {
            throw new BadRequestException("Payment Fail");
        }
        
        // get orderId from response
        var orderId = respronse.Respronse.Substring(0, respronse.Respronse.Length - 5);
       
        // if payment success -> change status Paid
        await _orderRepo.UpdateOrderStatus(Convert.ToInt32(orderId), OrderStatus.Paid);
        
        
        return Ok("Payment Success! OrderId: " + orderId);

    }
    
    [HttpGet("PaymentCallBackForSubscription")]
    public async Task<IActionResult> PaymentCallBackForSubscription()
    {
        var respronse = _vnPayService.PaymentExecute(Request.Query);
        
        // if CheckValid != true -> payment invalid
        if (respronse is not { CheckValid: true  } )
        {
            throw new BadRequestException("Payment invalid");
        }
        
        // if VnPayResponseCode != 00 -> payment fail
        if (respronse is not { VnPayResponseCode: "00" })
        {
            throw new BadRequestException("Payment Fail");
        }
        
        // get UserId from response
        var userId = respronse.Respronse.Substring(1, respronse.Respronse.Length - 5);
        var planId = respronse.Respronse[0];
        
        // if payment success -> active user, create subscription
        await _userRepo.ActivateUser(1);
        await _subscriptionRepo.CreateSubscription(Convert.ToInt32(userId), Convert.ToInt32(planId));
        
        
        return Ok("Payment Success! userId: " + userId + " - planId: "+ planId);

    }
    
}
