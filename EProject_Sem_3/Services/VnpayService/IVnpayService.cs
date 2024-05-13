using EProject_Sem_3.Models;

namespace EProject_Sem_3.Services.VnpayService;

public interface IVnPayService
{
    string CreatePaymentUrl(HttpContext context, VnPaymentRequest model);

    VnPaymentResponseModel PaymentExecute(IQueryCollection collections);

    string CreatePaymentUrlForOrder(HttpContext context, VnPaymentOrderRequestModel model);

    string CreatePaymentUrlForSubscription(HttpContext context, VnPaymentSubscriptionRequestModel model);
}