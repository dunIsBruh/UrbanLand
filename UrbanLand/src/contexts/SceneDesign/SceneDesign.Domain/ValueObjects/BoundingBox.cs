using Core.Abstractions;

namespace SceneDesign.Domain.ValueObjects;

public record BoundingBox : ValueObject
{
    public Position3D Min { get; }
    public Position3D Max { get; }

    private BoundingBox() { }

    public BoundingBox(Position3D center, Scale scale)
    {
        var halfX = scale.X / 2;
        var halfY = scale.Y / 2;
        var halfZ = scale.Z / 2;

        Min = new Position3D(center.X - halfX, center.Y - halfY, center.Z - halfZ);
        Max = new Position3D(center.X + halfX, center.Y + halfY, center.Z + halfZ);
    }

    public bool Contains(Position3D point)
        => point.X >= Min.X && point.X <= Max.X &&
           point.Y >= Min.Y && point.Y <= Max.Y &&
           point.Z >= Min.Z && point.Z <= Max.Z;

    public bool Intersects(BoundingBox other)
        => !(Max.X < other.Min.X || Min.X > other.Max.X ||
             Max.Y < other.Min.Y || Min.Y > other.Max.Y ||
             Max.Z < other.Min.Z || Min.Z > other.Max.Z);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Min;
        yield return Max;
    }
}