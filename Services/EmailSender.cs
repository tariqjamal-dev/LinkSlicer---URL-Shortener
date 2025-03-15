using LinkSlicer.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;

namespace LinkSlicer.Services
{
    public class EmailSender(IOptions<EmailSettings> emailSettings) : IEmailSender
    {
        private readonly EmailSettings _emailSettings = emailSettings.Value;

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                using var client = new SmtpClient(_emailSettings.SMTPServer, _emailSettings.Port)
                {
                    Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                    EnableSsl = _emailSettings.EnableSSL,  // ✅ Zoho requires this for TLS
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false
                };

                using var message = new MailMessage
                {
                    From = new MailAddress(_emailSettings.Username),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                message.To.Add(toEmail);

                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Email sending failed: {ex.Message}");
                throw;
            }
        }
    }
}
