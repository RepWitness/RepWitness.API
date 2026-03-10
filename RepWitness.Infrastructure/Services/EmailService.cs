using Microsoft.Extensions.Options;
using RepWitness.Infrastructure.Interfaces;
using RepWitness.Infrastructure.Models;
using System.Net;
using System.Net.Mail;

namespace RepWitness.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> smtpOptions) => _smtpSettings = smtpOptions.Value;

    public async Task SendEmailAsync(EmailDto emailInfo)
    {
        var mail = new MailMessage
        {
            From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
            Subject = emailInfo.Subject,
            Body = emailInfo.Body,
            IsBodyHtml = true
        };

        mail.To.Add(emailInfo.To);

        using var smtp = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
        {
            Credentials = new NetworkCredential(
                _smtpSettings.Username,
                _smtpSettings.Password
            ),
            EnableSsl = _smtpSettings.UseSSL
        };

        await smtp.SendMailAsync(mail);
    }
}