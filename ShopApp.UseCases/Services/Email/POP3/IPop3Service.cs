using ShopApp.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.UseCases.Services.Email.POP3
{
    public interface IPop3Service
    {
        Task<List<EmailMessageDTO>> GetInboxPop3Async(string userEmail, string password);
    }
}
