namespace SharedKernel.Identity;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendTemplatedEmailAsync(string to, string templateId, object model);
}