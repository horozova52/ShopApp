using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Microsoft.Extensions.Configuration;
using ShopApp.Shared.DTO;
using ShopApp.Core;
using ShopApp.Infrastructure;

namespace ShopApp.UseCases.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public EmailService(IConfiguration config, ApplicationDbContext context)
        {
            _config = config;
            _context = context;
        }

        public async Task SendEmailAsync(EmailMessageDTO message)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(message.From));
            email.To.Add(MailboxAddress.Parse(message.To));
            email.Subject = message.Subject;
            email.Body = new TextPart(message.IsHtml ? TextFormat.Html : TextFormat.Plain)
            {
                Text = message.Body
            };

            // Trimite emailul
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config["SmtpSettings:Server"], int.Parse(_config["SmtpSettings:Port"]), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["SmtpSettings:Username"], _config["SmtpSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            // Salvează emailul în DB
            var emailEntity = new EmailMessage
            {
                Id = Guid.NewGuid(),
                From = message.From,
                To = message.To,
                Subject = message.Subject,
                Body = message.Body,
                DateSent = DateTime.UtcNow,
                IsHtml = message.IsHtml
            };

            _context.EmailMessages.Add(emailEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<EmailMessageDTO>> ReceiveEmailsAsync()
        {
            return await Task.FromResult(
                _context.EmailMessages
                    .OrderByDescending(e => e.DateSent)
                    .Select(e => new EmailMessageDTO
                    {
                        From = e.From,
                        To = e.To,
                        Subject = e.Subject,
                        Body = e.Body,
                        IsHtml = e.IsHtml
                    })
                    .ToList()
            );
        }
    }
}
