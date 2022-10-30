using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Shared.Services.Email
{
    public class MailRequest
    {
        [EmailAddress]
        public string From { get; set; }

        [EmailAddress]
        public string ToAddress { get; set; }

        public IEnumerable<string> ToAddresses { get; set; } = new List<string>();

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        public IFormFileCollection Attachments { get; set; } = null;
    }
}
