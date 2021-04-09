using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEmailSender
    {
         Task SendEmailAsync(List<string> toAddresses, string subject, string body);
    }
}