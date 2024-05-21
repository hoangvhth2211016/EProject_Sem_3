using EProject_Sem_3.Models;

namespace EProject_Sem_3.Services.MailService;

public interface IEmailSender
{
    Task SendEmail(MailTemplate mailTemplate);

    string CreateOrderEmailBody(string name, string phone, string address, decimal total,
        List<(string productName, int quantity)> products);
}