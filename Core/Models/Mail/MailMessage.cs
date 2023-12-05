using System.Collections.Generic;

namespace RamDam.BackEnd.Core.Models.Mail
{
    public class MailMessage
    {
        public string Subject { get; set; }
        public MailAddress From { get; set; }
        public ICollection<MailAddress> To { get; set; }
        public string ReplyTo { get; set; }
        public string HtmlContent { get; set; }
        public string TextContent { get; set; }
        public string Language { get; set; }
    }

    public class MailAddress
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
