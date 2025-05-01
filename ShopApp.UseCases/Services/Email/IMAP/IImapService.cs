using ShopApp.Shared.DTO;

namespace ShopApp.UseCases.Services.Email.IMAP
{
    public interface IImapService
    {
        Task<List<EmailMessageDTO>> GetInboxAsync(string email, string password);
    }
}
