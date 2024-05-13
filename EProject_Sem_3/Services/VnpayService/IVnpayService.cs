using EProject_Sem_3.Models;

namespace EProject_Sem_3.Services.VnpayService;

public interface IVnPayService
{
    string CreatePaymentUrl( VnPaymentRequest model);

    VnPaymentResponseModel PaymentExecute(IQueryCollection collections);

    string CreatePaymentUrlForOrder( VnPaymentOrderRequestModel model);

    string CreatePaymentUrlForSubscription( VnPaymentSubscriptionRequestModel model);
}