using MailKit.Net.Pop3;
using MailKit.Security;
using MimeKit;
using ShopApp.Shared.DTO;

namespace ShopApp.UseCases.Services.Email.POP3
{
    public class Pop3Service : IPop3Service
    {
        public async Task<List<EmailMessageDTO>> GetInboxPop3Async(string userEmail, string password)
        {
            var messages = new List<EmailMessageDTO>();
            using var client = new Pop3Client();

            await client.ConnectAsync("pop.mail.ru", 995, SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(userEmail, password);

            for (int i = 0; i < Math.Min(client.Count, 10); i++)
            {
                var msg = await client.GetMessageAsync(i);
                messages.Add(new EmailMessageDTO
                {
                    From = msg.From.Mailboxes.FirstOrDefault()?.Address ?? "",
                    To = msg.To.Mailboxes.FirstOrDefault()?.Address ?? "",
                    Subject = msg.Subject ?? "",
                    Body = msg.TextBody ?? msg.HtmlBody ?? "",
                    IsHtml = !string.IsNullOrEmpty(msg.HtmlBody)
                });
            }

            await client.DisconnectAsync(true);
            return messages;
        }
    }
}
