using System.Net;
using System.Net.Mail;
using EProject_Sem_3.Exceptions;
using EProject_Sem_3.Models;
using EProject_Sem_3.Services.MailService;

namespace EProject_Sem_3.Services.EMailService;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmail(MailTemplate mailTemplate)
    {
        try
        {
            // create MailMessage
            var mail = new MailMessage();
            mail.From = new MailAddress(_configuration["MailSetting:From"]);
            mail.To.Add(mailTemplate.ToAddress);
            mail.Subject = mailTemplate.Subject;
            mail.Body = mailTemplate.Body;
            mail.IsBodyHtml = true;

            // SMTP client
            SmtpClient smtp = new SmtpClient();
            smtp.Host = _configuration["MailSetting:Host"];
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(_configuration["MailSetting:From"], _configuration["MailSetting:Password"]);

            // send email
                smtp.Send(mail);
            Console.WriteLine("Email sent successfully to "+ mailTemplate.ToAddress);
        }
        catch (Exception ex)
        {
            throw new BadRequestException("Fail to send email: "+ex.Message);
        }
    }
    
   
}