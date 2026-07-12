namespace Core.Infrastructure.Messaging;

public static class MessageHeaders
{
    public const string CorrelationId = "x-correlation-id";
    public const string CausationId = "x-causation-id";
    public const string TenantId = "x-tenant-id";
    public const string UserId = "x-user-id";
    public const string SourceContext = "x-source-context";
}