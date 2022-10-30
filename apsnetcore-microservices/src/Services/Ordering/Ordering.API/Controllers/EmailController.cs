using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Services.Email;

namespace Ordering.API.Controllers
{
    [Route("api/v1/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly ISmtpEmailService _emailService;

        public EmailController(ISmtpEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet("send-email")]
        public async Task<IActionResult> TestEmail()
        {
            var message = new MailRequest
            {
                Body = "hello",
                Subject = "Test",
                ToAddress = "xuanhoang190999@gmail.com"
            };
            await _emailService.SendEmailAsync(message);

            return Ok();
        }
    }
}
