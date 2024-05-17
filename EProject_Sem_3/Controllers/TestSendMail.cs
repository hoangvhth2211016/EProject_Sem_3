using EProject_Sem_3.Models;
using EProject_Sem_3.Repositories.Recipes;
using EProject_Sem_3.Services.MailService;
using Microsoft.AspNetCore.Mvc;

namespace EProject_Sem_3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestSendMailController : ControllerBase
    {

        private readonly IEmailSender _emailSender;

        public TestSendMailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpPost]
        public async Task<IActionResult> Sendmail(MailTemplate mailTemplate)
        {
            await _emailSender.SendEmail(mailTemplate);
            return Ok("send mail ok");
        }
    }
}



