using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Zarasa.Editorial.Api.Helper
{
    public class AuthMessageSender : IEmailSender
    {
        public AuthMessageSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }


        public EmailSettings _emailSettings { get; }

        public Task SendEmailAsync(string email, string subject, string message)
        {

            Execute (email, subject, message).Wait();
            return Task.FromResult(0);
        }

        public async Task Execute(string toEmail, string subject, string message)
        {
          try
          {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_emailSettings.UsernameEmail, "Zarasa Editorial");
            mailMessage.To.Add(toEmail);
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;

            using (SmtpClient smtp = new SmtpClient(_emailSettings.Host, _emailSettings.Port))
            {
                smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(mailMessage);
            }
          }
          catch(Exception ex)
          {
             //do something here
          }
        }

    }
}