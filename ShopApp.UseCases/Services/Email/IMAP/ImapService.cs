using MailKit;
using MailKit.Net.Imap;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using ShopApp.Shared.DTO;

namespace ShopApp.UseCases.Services.Email.IMAP
{
    public class ImapService : IImapService
    {
        private readonly IConfiguration _config;

        public ImapService(IConfiguration config)
        {
            _config = config;
        }

        private (string email, string password, string server, int port) GetImapCredentials(string userEmail)
        {
            var accounts = _config.GetSection("ImapAccounts").Get<List<ImapAccountConfigDTO>>();
            var account = accounts.FirstOrDefault(a => a.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));
            if (account == null)
                throw new NotSupportedException("Nu există un cont IMAP configurat pentru acest email.");

            return (account.Email, account.Password, account.Server, account.Port);
        }

        public async Task<List<EmailMessageDTO>> GetInboxAsync(string userEmail, string password)
        {
            var messages = new List<EmailMessageDTO>();
            var (email, pwd, server, port) = GetImapCredentials(userEmail);

            using var client = new ImapClient();
            await client.ConnectAsync(server, port, SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(email, pwd);

            var inbox = client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadOnly);

            int maxToLoad = 20;
            int startIndex = Math.Max(0, inbox.Count - maxToLoad);

            for (int i = startIndex; i < inbox.Count; i++)
            {
                var msg = await inbox.GetMessageAsync(i);

                var attachmentsBase64 = new List<string>();
                var attachmentFileNames = new List<string>();

                foreach (var attachment in msg.Attachments)
                {
                    if (attachment is MimePart part)
                    {
                        using var memory = new MemoryStream();
                        await part.Content.DecodeToAsync(memory);
                        string base64 = Convert.ToBase64String(memory.ToArray());
                        attachmentsBase64.Add(base64);
                        attachmentFileNames.Add(part.FileName);
                    }
                }

                messages.Add(new EmailMessageDTO
                {
                    From = msg.From.Mailboxes.FirstOrDefault()?.Address ?? "unknown",
                    To = msg.To.Mailboxes.FirstOrDefault()?.Address ?? userEmail,
                    Subject = msg.Subject ?? "",
                    Body = msg.TextBody ?? msg.HtmlBody ?? "",
                    IsHtml = !string.IsNullOrEmpty(msg.HtmlBody),
                    AttachmentsBase64 = attachmentsBase64,
                    AttachmentFileNames = attachmentFileNames
                });
            }

            await client.DisconnectAsync(true);
            return messages;
        }
    }


}
