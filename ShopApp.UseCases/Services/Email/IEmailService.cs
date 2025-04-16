using ShopApp.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.UseCases.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessageDTO message);
        Task<List<EmailMessageDTO>> ReceiveEmailsAsync();
    }

}
