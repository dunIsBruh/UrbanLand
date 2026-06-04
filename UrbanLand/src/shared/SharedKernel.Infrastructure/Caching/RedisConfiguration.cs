namespace SharedKernel.Infrastructure.Caching;

public class RedisConfiguration
{
    public const string SectionName = "Redis";
    
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 6379;
    public string Password { get; set; } = string.Empty;
    public string InstanceName { get; set; } = "UrbanLandscape";
    
    public string ConnectionString => 
        string.IsNullOrEmpty(Password) 
            ? $"{Host}:{Port}" 
            : $"{Host}:{Port},password={Password}";
}