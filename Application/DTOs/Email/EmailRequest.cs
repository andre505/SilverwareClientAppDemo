using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Application.DTOs.Email
{
    public class EmailRequest
    {
        public string To { get; set; }
        public List<string> multipleRecipients { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
        public string Heading { get; set; }
        public string Username { get; set; }
        public string BodyParagraph1 { get; set; }
        public string BodyParagraph2 { get; set; }
        public string ButtonUrl { get; set; }
        public string ButtonLabel { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
