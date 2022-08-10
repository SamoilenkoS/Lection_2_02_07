using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lection_2_BL.Services.SMTPService
{
    public class MockSMTPService : ISendingBlueSmtpService
    {
        public Task SendMail(MailInfo mailInfo)
        {
            return Task.CompletedTask;
        }
    }
}
