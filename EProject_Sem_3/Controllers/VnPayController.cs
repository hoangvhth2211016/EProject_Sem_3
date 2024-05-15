using EProject_Sem_3.Exceptions;
using EProject_Sem_3.Models;
using EProject_Sem_3.Models.Orders;
using EProject_Sem_3.Models.Users;
using EProject_Sem_3.Repositories;
using EProject_Sem_3.Repositories.Orders;
using EProject_Sem_3.Repositories.OrdersDetail;
using EProject_Sem_3.Repositories.Users;
using EProject_Sem_3.Services.MailService;
using EProject_Sem_3.Services.VnpayService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EProject_Sem_3.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VnPayController : ControllerBase
{
    private readonly IVnPayService _vnPayService;
    private readonly IOrderRepo _orderRepo;
    private readonly IUserRepo _userRepo;
    private readonly ISubscriptionRepo _subscriptionRepo;
    private readonly IEmailSender _emailSender;
    private readonly AppDbContext _dbContext;


    public VnPayController(IVnPayService vnPayService,
                            IOrderRepo orderRepo,
                            IUserRepo userRepo,
                            ISubscriptionRepo subscriptionRepo,
                            IEmailSender emailSender,
                            AppDbContext dbContext)
    {
        _vnPayService = vnPayService;
        _userRepo = userRepo;
        _userRepo = userRepo;
        _orderRepo = orderRepo;
        _subscriptionRepo = subscriptionRepo;
        _emailSender = emailSender;
        _dbContext = dbContext;
    }
    

    [HttpGet("PaymentCallBackForOrder")]
    public async Task<IActionResult> PaymentCallBackForOrder()
    {
        var respronse = _vnPayService.PaymentExecute(Request.Query);

        var status = 1;
        
        // if CheckValid != true -> payment invalid
        if (respronse is not { CheckValid: true  } )
        {
            status = 0;
            
        }
        
        // if VnPayResponseCode != 00 -> payment fail
        if (respronse is not { VnPayResponseCode: "00" } )
        {
            status = 0;
        }
        
        // get orderId from response
        var orderId = respronse.Respronse.Substring(0, respronse.Respronse.Length - 5);
       
        // if payment success -> change status Paid
        await _orderRepo.UpdateOrderStatus(Convert.ToInt32(orderId), OrderStatus.Paid);
        
        
        // send mail to user
        var order = await _orderRepo.GetOrder(Convert.ToInt32(orderId));

        await _emailSender.SendEmail(new MailTemplate
        {
            ToAddress = order.Email,
            Subject = "Ice Scream Parlour",
            Body = "<h1 style='color:red'>Ordered books successfully!\nYou will receive your book in the next few days!<h1>"
        });
        
        return Redirect("http://localhost:8080/callback?status="+status);

    }
    
    [HttpGet("PaymentCallBackForSubscription")]
    public async Task<IActionResult> PaymentCallBackForSubscription()
    {
        var respronse = _vnPayService.PaymentExecute(Request.Query);
        
        // if CheckValid != true -> payment invalid
        if (respronse is not { CheckValid: true  } )
        {
            return Redirect("http://localhost:8080/callback?status=0");
            throw new BadRequestException("Payment invalid");
        }
        
        // if VnPayResponseCode != 00 -> payment fail
        if (respronse is not { VnPayResponseCode: "00" })
        {
            return Redirect("http://localhost:8080/callback?status=0");
            throw new BadRequestException("Payment Fail");
        }
        
        // get UserId and PlanId from response
        var planId = respronse.Respronse[0];
        var userId = respronse.Respronse.Substring(1, respronse.Respronse.Length - 6);
        
        
        // if payment success -> active user, create subscription
        await _userRepo.ActivateUser(Convert.ToInt32(userId));
        await _subscriptionRepo.CreateSubscription(Convert.ToInt32(userId), int.Parse(planId.ToString()));

        var user = await _dbContext.Users.FindAsync(Convert.ToInt32(userId))?? throw new NotFoundException("User not found");
        
        //send Email
        await _emailSender.SendEmail(new MailTemplate
        {
            ToAddress = user.Email,
            Subject = "Ice Scream Parlour",
            Body = "<h1 style='color:red'>Subscribe successfully!<h1>"
        });
        
        
        // return Ok(userId);
        return Redirect("http://localhost:8080/callback?status=1");

    }
    
}
