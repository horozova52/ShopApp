using System;

namespace ShopApp.Core
{
    public class EmailMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }

        public DateTime DateSent { get; set; } = DateTime.UtcNow;
        public bool IsReceived { get; set; } = false;
    }
}
