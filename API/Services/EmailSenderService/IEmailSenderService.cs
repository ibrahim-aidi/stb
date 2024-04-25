using API.Dtos;

namespace API.Services.EmailSenderService
{
    public interface IEmailSenderService
    {
        Task SendPasswordByEmailAsync(UserForEmail user);
    }
}
