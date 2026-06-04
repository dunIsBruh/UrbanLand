using SharedKernel.Abstractions;

namespace SceneDesign.Domain.ValueObjects;

public record Position3D(double X, double Y, double Z) : ValueObject
{
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
        yield return Z;
    }
    
    public double DistanceTo(Position3D other)
        => Math.Sqrt(Math.Pow(X - other.X, 2) + 
                     Math.Pow(Y - other.Y, 2) + 
                     Math.Pow(Z - other.Z, 2));
}