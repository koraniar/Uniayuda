using Entities.Entities;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    public interface IEmailRepository
    {
        Task SendByRestApiAsync(EmailMessage emailMessage);
        Task SendBySmtpAsync(EmailMessage emailMessage);
    }
}
