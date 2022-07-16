using System.Text;

namespace Lection_2_BL.Services.SMTPService
{
    public class MailInfo
    {
        public string Email { get; set; }
        public string ClientName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}