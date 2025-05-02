using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Microsoft.Extensions.Configuration;
using ShopApp.Shared.DTO;
using ShopApp.Core;
using ShopApp.Infrastructure;
using ShopApp.UseCases.Services.Email.IMAP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ShopApp.UseCases.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;
        private readonly IImapService _imapService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContext;

        public EmailService(IImapService imapService, IConfiguration config, ApplicationDbContext context,
                      UserManager<ApplicationUser> userManager,
                      IHttpContextAccessor httpContext)
        {
            _imapService = imapService;
            _config = config;
            _context = context;
            _userManager = userManager;
            _httpContext = httpContext;
        }

        public async Task SendEmailAsync(EmailMessageDTO message)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(message.From));
            email.To.Add(MailboxAddress.Parse(message.To));
            if (!string.IsNullOrEmpty(message.ReplyTo))
            {
                email.ReplyTo.Add(new MailboxAddress("", message.ReplyTo));
            }

            email.Subject = message.Subject;

            var builder = new BodyBuilder
            {
                HtmlBody = message.IsHtml ? message.Body : null,
                TextBody = !message.IsHtml ? message.Body : null
            };

            // ATASAMENTE
            if (message.AttachmentsBase64 != null && message.AttachmentFileNames != null)
            {
                for (int i = 0; i < message.AttachmentsBase64.Count; i++)
                {
                    var bytes = Convert.FromBase64String(message.AttachmentsBase64[i]);
                    var name = message.AttachmentFileNames[i];
                    builder.Attachments.Add(name, bytes);
                }
            }

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config["SmtpSettings:Server"], int.Parse(_config["SmtpSettings:Port"]), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["SmtpSettings:Username"], _config["SmtpSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            var emailEntity = new EmailMessage
            {
                Id = Guid.NewGuid(),
                From = message.From,
                To = message.To,
                Subject = message.Subject,
                Body = message.Body,
                DateSent = DateTime.UtcNow,
                IsHtml = message.IsHtml,
                IsReceived = false
            };

            _context.EmailMessages.Add(emailEntity);
            await _context.SaveChangesAsync();
        }
        public async Task ReplyToEmailAsync(EmailReplyDTO reply)
        {
            var message = new EmailMessageDTO
            {
                From = reply.From,
                To = reply.To,
                Subject = reply.Subject,
                Body = reply.Body,
                IsHtml = reply.IsHtml,
                AttachmentsBase64 = reply.AttachmentsBase64,
                AttachmentFileNames = reply.AttachmentFileNames
            };

            await SendEmailAsync(message);
        }

        public async Task<List<EmailMessageDTO>> ReceiveEmailsAsync()
        {
            var user = _httpContext.HttpContext?.User;
            if (user == null)
                return new List<EmailMessageDTO>();

            var appUser = await _userManager.GetUserAsync(user);
            if (appUser == null || string.IsNullOrEmpty(appUser.Email))
                return new List<EmailMessageDTO>();

            var password = _config["ImapSettings:Password"];

            return await _imapService.GetInboxAsync(appUser.Email, password);
        }


    }
}
