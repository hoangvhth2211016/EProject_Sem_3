using EProject_Sem_3.Models;

namespace EProject_Sem_3.Services.VnpayService;

public interface IVnPayService
{
    string CreatePaymentUrl(HttpContext context,VnPaymentRequestModel model);

    VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
}