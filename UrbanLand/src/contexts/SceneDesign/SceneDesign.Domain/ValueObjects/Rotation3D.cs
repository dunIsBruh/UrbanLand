using SharedKernel.Abstractions;

namespace SceneDesign.Domain.ValueObjects;

public record Rotation(double Yaw, double Pitch, double Roll) : ValueObject
{
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Yaw;
        yield return Pitch;
        yield return Roll;
    }
    
    public static Rotation Default => new(0, 0, 0);
    public static Rotation North => new(0, 0, 0);
    public static Rotation East => new(90, 0, 0);
    public static Rotation South => new(180, 0, 0);
    public static Rotation West => new(-90, 0, 0);
}