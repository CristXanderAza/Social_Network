using Social_Network.Core.Application.DTOs.Email;
using Social_Network.Core.Domain.Base;

namespace Social_Network.Core.Application.Interfaces.Services.Email
{
    public interface IEmailService
    {
        Task<Result<Unit>> SendAsync(EmailRequestDTO emailRequest);
    }
}
