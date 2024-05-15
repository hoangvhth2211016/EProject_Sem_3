using EProject_Sem_3.Models;

namespace EProject_Sem_3.Services.MailService;

public interface IEmailSender
{
    Task SendEmail(MailTemplate mailTemplate);
}