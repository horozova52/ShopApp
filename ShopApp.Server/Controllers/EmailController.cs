using Microsoft.AspNetCore.Mvc;
using ShopApp.Shared.DTO;
using ShopApp.UseCases.Services.Email;

namespace ShopApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _service;

        public EmailController(IEmailService service)
        {
            _service = service;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail(EmailMessageDTO message)
        {
            await _service.SendEmailAsync(message);
            return Ok();
        }

        [HttpGet("inbox")]
        public async Task<List<EmailMessageDTO>> GetInbox()
        {
            return await _service.ReceiveEmailsAsync();
        }
    }

}
