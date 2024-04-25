using API.Services.WhatsappService;
using System;
using System.Collections.Generic;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace API.Services.WhatsappService
{
    public class WhatsAppSenderService : IWhatsAppSenderService
    {
        private readonly IConfiguration _configuration;

        public WhatsAppSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMessageAsync(string to, string message)
        {
            var accountSid = _configuration["Twilio:AccountSid"];
            var authToken = _configuration["Twilio:AuthToken"];
            var whatsAppNumber = _configuration["Twilio:WhatsAppNumber"];

            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
                new PhoneNumber(to)); // Use the provided 'to' parameter
            messageOptions.From = new PhoneNumber("whatsapp:+14155238886");
            messageOptions.Body = message; // Use the provided 'message' parameter

            var sentMessage = await MessageResource.CreateAsync(messageOptions);
         
        }
    }
}