using System.Threading.Tasks;

namespace Lection_2_BL.Services.SMTPService
{
    public interface ISendingBlueSmtpService
    {
        Task SendMail(MailInfo mailInfo);
    }
}