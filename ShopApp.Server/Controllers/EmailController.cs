using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Core;
using ShopApp.Shared.DTO;
using ShopApp.UseCases.Services.Email;
using ShopApp.UseCases.Services.Email.IMAP;

namespace ShopApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _service;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IImapService _imapService;
        public EmailController(IEmailService service, IImapService imapService, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
            _imapService = imapService;
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

        [HttpGet("inbox-imap")]
        public async Task<ActionResult<List<EmailMessageDTO>>> GetInboxImap([FromQuery] string email, [FromQuery] string password)
        {
            var result = await _imapService.GetInboxAsync(email, password);
            if (result == null || result.Count == 0)
                return NotFound("No messages found or authentication failed.");

            return Ok(result);
        }


    }
}