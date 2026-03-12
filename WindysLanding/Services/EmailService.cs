using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using WindysLanding.Models;

namespace WindysLanding.Services
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public void SendContactEmail(ContactFormViewModel model)
        {
            var subject = string.IsNullOrWhiteSpace(model.Subject)
                ? "New Contact Form Submission"
                : model.Subject;

            var body = $@"New contact form message

                Name: {model.Name}
                Phone: {model.Phone}
                Email: {model.Email}

                Message:
                {model.Message}";

            using var smtp = new SmtpClient(_settings.SmtpServer)
            {
                Port = _settings.Port,
                Credentials = new NetworkCredential(_settings.Username, _settings.Password),
                EnableSsl = true
            };

            using var mail = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
                Subject = subject,
                Body = body
            };

            mail.To.Add(_settings.AdminEmail);

            smtp.Send(mail);
        }

        public void SendNewsletterSignupEmail(NewsletterSignupViewModel model)
        {
            var subject = "New Newsletter Signup";

            var body = $@"A new user signed up for the newsletter.

Subscriber Email: {model.Email}";

            using var smtp = new SmtpClient(_settings.SmtpServer)
            {
                Port = _settings.Port,
                Credentials = new NetworkCredential(_settings.Username, _settings.Password),
                EnableSsl = true
            };

            using var mail = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
                Subject = subject,
                Body = body
            };

            mail.To.Add(_settings.AdminEmail);

            smtp.Send(mail);
        }
    }
}