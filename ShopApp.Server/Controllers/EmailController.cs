using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Core;
using ShopApp.Shared.DTO;
using ShopApp.UseCases.Services.Email;

namespace ShopApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _service;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmailController(IEmailService service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailMessageDTO message)
        {
            await _service.SendEmailAsync(message);
            return Ok();
        }

        [HttpGet("inbox")]
        public async Task<ActionResult<List<EmailMessageDTO>>> GetInbox()
        {
            var emails = await _service.ReceiveEmailsAsync();
            return Ok(emails);
        }

        [HttpPost("reply")]
        public async Task<IActionResult> ReplyToEmail([FromBody] EmailReplyDTO reply)
        {
            await _service.ReplyToEmailAsync(reply);
            return Ok();
        }

        [Authorize]
        [HttpGet("my-inbox")]
        public async Task<ActionResult<List<EmailMessageDTO>>> GetMyInbox()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || string.IsNullOrEmpty(user.Email))
            {
                return Unauthorized();
            }

            var allEmails = await _service.ReceiveEmailsAsync();
            var myEmails = allEmails.Where(e => e.To == user.Email).ToList();
            return Ok(myEmails);
        }

       
    }
}