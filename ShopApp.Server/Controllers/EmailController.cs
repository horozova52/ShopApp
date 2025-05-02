using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Core;
using ShopApp.Shared.DTO;
using ShopApp.UseCases.Services.Email;
using ShopApp.UseCases.Services.Email.IMAP;
using ShopApp.UseCases.Services.Email.POP3;

namespace ShopApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _service;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IImapService _imapService;
        private readonly IPop3Service _pop3Service;
        public EmailController(IEmailService service, IPop3Service pop3Service, IImapService imapService, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
            _imapService = imapService;
            _pop3Service = pop3Service;
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
        [HttpGet("inbox-pop3")]
        public async Task<ActionResult<List<EmailMessageDTO>>> GetInboxPop3Async(string userEmail, string password)
        {
            try
            {
                var emails = await _pop3Service.GetInboxPop3Async(userEmail, password);
                return Ok(emails);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


    }
}