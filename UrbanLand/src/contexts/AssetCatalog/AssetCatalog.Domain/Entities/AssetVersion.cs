using AssetCatalog.Domain.ValueObjects;
using Core.Abstractions;

namespace AssetCatalog.Domain.Entities;

public class AssetVersion : Entity<Guid>
{
    public int VersionNumber { get; }
    public ModelData Model { get; }
    public DateTime CreatedAt { get; }

    // EF Core
    private AssetVersion()
    {
        Model = null!;
    } 

    public AssetVersion(int versionNumber, ModelData model, DateTime createdAt) : base(Guid.NewGuid())
    {
        VersionNumber = versionNumber;
        Model = model;
        CreatedAt = createdAt;
    }
}