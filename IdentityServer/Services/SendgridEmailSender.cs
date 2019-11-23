using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Configurations;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace IdentityServer.Services
{
    public class SendgridEmailSender : IEmailSender
    {
        
        private readonly SendgridConfiguration _configuration;
        private readonly ILogger<SendgridEmailSender> _logger;

        public SendgridEmailSender(ILogger<SendgridEmailSender> logger,   SendgridConfiguration configuration)
        {
            _logger = logger; 
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
             
            var client = new SendGridClient(_configuration.ApiKey);
            var from = new EmailAddress(_configuration.SourceEmail, _configuration.SourceName);

            var to = new EmailAddress(email); 
            var msg = MailHelper.CreateSingleEmail(from, to, subject,null,htmlMessage);
            var response = await client.SendEmailAsync(msg); 
             
            if ((response.StatusCode == System.Net.HttpStatusCode.OK) || (response.StatusCode == System.Net.HttpStatusCode.Created))
            {
                _logger.LogInformation($"Email: {email}, subject: {subject}, message: {htmlMessage} successfully sent");
            }
            else
            {
                var errorMessage = response.Body.ReadAsStringAsync();
                _logger.LogError($"Response with code {response.StatusCode} and body {errorMessage} after sending email: {email}, subject: {subject}");
            }
        }
        
    }
}
