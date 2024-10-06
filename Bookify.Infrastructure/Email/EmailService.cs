using Bookify.Application.Abstractions.Email;

namespace Bookify.Infrastructure.Email;

internal sealed class EmailService : IEmailService
{
    public Task SendAsync(Domain.Entities.Users.Email recepient, string subject, string body)
    {
        return Task.CompletedTask;
    }
}