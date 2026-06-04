namespace SharedKernel.Contracts;

public class DateTimeProvider
{
    public DateTime UtcNow { get; private set; } = DateTime.UtcNow;

    public void Set(DateTime dateTime)
    {
        UtcNow = dateTime;
    }
}
