using Cross;
using System.Collections.Generic;

namespace Entities.Entities
{
    public class EmailMessage
    {
        public string From { get; set; }
        public string FromName { get; set; }
        public string To { get; set; }
        public string ToName { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
        public IList<EmailAttachment> Attachements { get; set; }

        public EmailMessage(string from = null, string fromName = null, string toName = null)
        {
            From = !string.IsNullOrEmpty(from) ? from : Constants.SenderEmail;
            FromName = !string.IsNullOrEmpty(fromName) ? fromName : Constants.SenderName;
            ToName = !string.IsNullOrEmpty(toName) ? toName : Constants.RecipientName;
            Attachements = new List<EmailAttachment>();
        }

    }

    public class EmailAttachment
    {
        public byte[] Content { get; set; }
        public string ContentName { get; set; }
        public EmailAttachment(byte[] content, string contentName)
        {
            Content = content;
            ContentName = contentName;
        }
    }
}
