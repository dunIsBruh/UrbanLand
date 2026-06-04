using SharedKernel.Abstractions;

namespace SceneDesign.Domain.ValueObjects;

public record Scale(double X, double Y, double Z) : ValueObject
{
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
        yield return Z;
    }
    
    public static Scale Default => new(1, 1, 1);
    public static Scale Uniform(double factor) => new(factor, factor, factor);
}