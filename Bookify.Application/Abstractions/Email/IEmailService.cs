namespace Bookify.Application.Abstractions.Email;

public interface IEmailService
{
    Task SendAsync(Domain.Entities.Users.Email recepient, string subject, string body);
}
