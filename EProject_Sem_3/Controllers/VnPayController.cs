using EProject_Sem_3.Exceptions;
using EProject_Sem_3.Models;
using EProject_Sem_3.Models.Orders;
using EProject_Sem_3.Repositories.OrdersDetail;
using EProject_Sem_3.Services.VnpayService;
using Microsoft.AspNetCore.Mvc;

namespace EProject_Sem_3.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VnPayController : ControllerBase
{
    private readonly IVnPayService _vnPayService;


    public VnPayController(IVnPayService vnPayService)
    {
        _vnPayService = vnPayService;
    }

    [HttpGet]
    public async Task<IActionResult> CreateUrlVnpay(OrderDto dto)
    {
        
        // check validation dto
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        
        var vnPayModel = new VnPaymentRequestModel
        {
            TotalAmount = dto.TotalAmount,
            CreatedDate = DateTime.Now,
            OrderId = new Random().Next(1000, 100000)
        };

        // create payment url
        return Ok(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));

    }

    [HttpGet("CheckStatus")]
    public async Task<IActionResult> PaymentCallBack()
    {
        var respronse = _vnPayService.PaymentExecute(Request.Query);

        // if VnPayResponseCode != 00 -> payment fail
        if (respronse is not { VnPayResponseCode: "00" })
        {
            throw new BadRequestException("Payment Fail");
        }

        return Ok("Payment Success");

    }
    
}
