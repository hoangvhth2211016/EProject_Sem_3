using EProject_Sem_3.Models;
using Microsoft.AspNetCore.Components.Sections;
using Microsoft.Identity.Client;
using VNPAY_CS_ASPX;

namespace EProject_Sem_3.Services.VnpayService;

public class VnPayService : IVnPayService
{
    private readonly IConfiguration _config;

    public VnPayService(IConfiguration configuration)
    {
        _config = configuration;
    }

    public string CreatePaymentUrlForOrder(HttpContext context, VnPaymentOrderRequestModel model)
    {
        var vnPaymentRequest = new VnPaymentRequest();
        vnPaymentRequest.Amount = (model.TotalAmount * 100).ToString();
        vnPaymentRequest.ReturnUrl = "http://localhost:5180/api/VnPay/PaymentCallBackForOrder";
        vnPaymentRequest.OrderInfo = "Thanh toan don hang:" + model.OrderId + ". So dien thoai: " + model.Phone;
        vnPaymentRequest.TxnRef = model.OrderId.ToString() + new Random().Next(10000, 99999);


        return CreatePaymentUrl(context, vnPaymentRequest);
    }
    
    public string CreatePaymentUrlForSubscription(HttpContext context, VnPaymentSubscriptionRequestModel model)
    {
        var vnPaymentRequest = new VnPaymentRequest();
        vnPaymentRequest.Amount = (model.TotalAmount * 100).ToString();
        vnPaymentRequest.ReturnUrl = "http://localhost:5180/api/VnPay/PaymentCallBackForSubscription";
        vnPaymentRequest.OrderInfo = "Đăng ký thành viên: " + model.UserId + " - " + model.PlanId ;
        vnPaymentRequest.TxnRef = model.UserId + model.PlanId.ToString() + new Random().Next(10000, 99999);


        return CreatePaymentUrl(context, vnPaymentRequest);
    }
    
    public string CreatePaymentUrl(HttpContext context, VnPaymentRequest model)
    {
        
       
            //Get Config Info
            string vnp_Returnurl = model.ReturnUrl; //URL nhan ket qua tra ve 
            string vnp_Url = _config["VnPay:BaseUrl"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = "8CJY7ZHO"; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = _config["VnPay:HashSecret"]; //Secret Key
        

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", model.Amount); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", model.OrderInfo);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef",  model.TxnRef); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
           
            return paymentUrl;
        }

    public VnPaymentResponseModel PaymentExecute(IQueryCollection collections)
    {
        var vnpay = new VnPayLibrary();

        foreach (var (key, value) in collections)
        {
            if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
            {
                vnpay.AddResponseData(key, value.ToString());
            }
        }
        
        
        var respronse = vnpay.GetResponseData("vnp_TxnRef");
        var vnp_SecureHash = collections
            .FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
        var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
        

        bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VnPay:HashSecret"]);
        if (!checkSignature)
        {
            return new VnPaymentResponseModel()
            {
                CheckValid = false
            };
        }

        return new VnPaymentResponseModel
        {
            CheckValid = true,
            Respronse = respronse,
            VnPayResponseCode = vnp_ResponseCode
        };
    }

}