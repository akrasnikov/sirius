using System.Threading.Tasks;
using ProjectName.ServiceName.Application.DTOs.Email;

namespace ProjectName.ServiceName.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}