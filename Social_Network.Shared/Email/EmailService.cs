using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Social_Network.Core.Application.DTOs.Email;
using Social_Network.Core.Application.Interfaces.Services.Email;
using Social_Network.Core.Domain.Base;
using Social_Network.Core.Domain.Settings;


namespace Social_Network.Infraestructure.Shared.Email
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<MailSettings> opt, ILogger<EmailService> logger)
        {
            _mailSettings = opt.Value;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<Unit>> SendAsync(EmailRequestDTO emailRequest)
        {
            try
            {
                emailRequest.ToRange.Add(emailRequest.To ?? "");
                var email = new MimeMessage()
                {
                    Sender = MailboxAddress.Parse(_mailSettings.EmailFrom),
                    Subject = emailRequest.Subject,
                };
                email.To.AddRange(emailRequest.ToRange.Select(to => MailboxAddress.Parse(to)).ToList());
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = emailRequest.BodyHtml,
                };
                email.Body = bodyBuilder.ToMessageBody();
                using SmtpClient client = new SmtpClient();
                await client.ConnectAsync(_mailSettings.SmtpHost, _mailSettings.SmtpPort
                    , MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
                await client.SendAsync(email);
                await client.DisconnectAsync(true);

                return Result<Unit>.Ok(Unit.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "An exception ocurred {Exception}", ex);
                return Result<Unit>.Fail("Something were wrong while sending the email");

            }
        }
    }
}
