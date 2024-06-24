
using AdsReportingPortal.Model.DTO;
using AdsReportingPortal.Service.Interface;
using MailKit.Net.Smtp;
using MimeKit;

namespace AdsReportingPortal.Service.Implementation
{
    public class EmailService :IEmailServices
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _configuration["EmailConfiguration:UserName"]));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };
            return emailMessage;
        }

        public void Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_configuration["EmailConfiguration:Host"], int.Parse(_configuration["EmailConfiguration:Port"]), true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_configuration["EmailConfiguration:UserName"], _configuration["EmailConfiguration:Password"]);
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}
