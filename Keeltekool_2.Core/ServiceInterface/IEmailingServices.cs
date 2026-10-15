using Keeltekool_2.Core.DTO;

namespace Keeltekool_2.Core.ServiceInterface
{
    public interface IEmailingServices
    {
        void SendEmail(EmailDTO request);
        void SendEmailToken(EmailTokenDTO request, string token);
    }
}