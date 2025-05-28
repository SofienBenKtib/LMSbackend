using eduflowbackend.Application.Abstractions;
using FluentEmail.Core;

namespace eduflowbackend.Infrastructure.Notification;

public class EmailService : IEmailService
{
    private readonly IFluentEmail _fluentEmail;

    public EmailService(IFluentEmail fluentEmail)
    {
        _fluentEmail = fluentEmail;
    }

    public async Task Send(string to, string subject, string body)
    {
        await _fluentEmail
            .To(to)
            .Subject(subject)
            .Body(body)
            .SendAsync();
    }
}