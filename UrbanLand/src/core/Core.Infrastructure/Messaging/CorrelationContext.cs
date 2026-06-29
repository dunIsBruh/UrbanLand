namespace Core.Infrastructure.Messaging;

public static class CorrelationContext
{
    private static readonly AsyncLocal<Guid> _correlationId = new();

    public static Guid CorrelationId
    {
        get => _correlationId.Value == Guid.Empty 
            ? Guid.NewGuid() 
            : _correlationId.Value;
        set => _correlationId.Value = value;
    }

    public static void SetCorrelationId(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
}