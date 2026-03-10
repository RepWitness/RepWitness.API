using RepWitness.Infrastructure.Models;

namespace RepWitness.Infrastructure.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(EmailDto emailInfo);
}
