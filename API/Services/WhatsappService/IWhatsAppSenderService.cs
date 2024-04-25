namespace API.Services.WhatsappService
{
    public interface IWhatsAppSenderService
    {
        Task SendMessageAsync(string to, string message);
    }
}
