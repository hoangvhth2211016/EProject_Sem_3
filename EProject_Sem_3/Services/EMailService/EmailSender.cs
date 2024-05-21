using System.Net;
using System.Net.Mail;
using System.Text;
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
    
    public string CreateOrderEmailBody(string name, string phone, string address, decimal totalAmount, List<(string productName, int quantity)> products)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<html><body>");
        sb.Append("<h1>Ordered books successfully!<h1>");
        sb.Append("You will receive your book in the next few days!<h1>");
        sb.Append("<h2>Order Info</h2>");
        sb.AppendFormat("<p><strong>Your name:</strong> {0}</p>", name);
        sb.AppendFormat("<p><strong>Your telephone number:</strong> {0}</p>", phone);
        sb.AppendFormat("<p><strong>Your address:</strong> {0}</p>", address);
        sb.AppendFormat("<p><strong>Total Amount:</strong> ${0}</p>", totalAmount);

        sb.Append("<h3>List books</h3>");
        sb.Append("<table border='1' style='border-collapse:collapse; width: 100%;'>");
        sb.Append("<tr><th>Book name</th><th>Quantity</th></tr>");

        foreach (var product in products)
        {
            sb.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", product.productName, product.quantity);
        }

        sb.Append("</table>");
        sb.Append("</body></html>");

        return sb.ToString();
    }
    
   
}