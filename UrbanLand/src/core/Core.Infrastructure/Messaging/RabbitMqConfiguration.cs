namespace Core.Infrastructure.Messaging;

public class RabbitMqConfiguration
{
    public const string SectionName = "RabbitMQ";
    
    public string Host { get; set; } = "localhost";
    public ushort Port { get; set; } = 5672;
    public string Username { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public bool UsePostgres { get; set; } = true;
}