using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Shared.DTO
{
    public class EmailMessageDTO
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public List<string>? AttachmentsBase64 { get; set; }
        public List<string>? AttachmentFileNames { get; set; }

    }

}
