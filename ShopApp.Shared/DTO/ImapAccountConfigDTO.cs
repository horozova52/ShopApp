using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ShopApp.Shared.DTO
{
    public class ImapAccountConfigDTO
    {
        public string Provider { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
    }
}
