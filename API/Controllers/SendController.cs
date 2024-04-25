using API.Dtos;
using API.Services.AuthService;
using API.Services.WhatsappService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendController : ControllerBase
    {
        private readonly IWhatsAppSenderService _WhatsAppSenderService;
        public SendController(IWhatsAppSenderService WhatsAppSenderService)
        {
            _WhatsAppSenderService =WhatsAppSenderService;
        }

        [HttpPost("whatsapp")]

        public async Task<IActionResult> SendMessage([FromBody]  WhatsappMessage msg)
        {
            if (msg== null)
                return BadRequest("Message data is required.");

            if (string.IsNullOrEmpty(msg.To) || string.IsNullOrEmpty(msg.Message))
                return BadRequest("Recipient number and message body are required.");

            await _WhatsAppSenderService.SendMessageAsync(msg.To, msg.Message);

            return Ok("Message sent successfully.");
        }
    }





}
