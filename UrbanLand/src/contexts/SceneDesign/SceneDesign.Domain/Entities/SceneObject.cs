using SceneDesign.Domain.ValueObjects;
using SharedKernel.Abstractions;

namespace SceneDesign.Domain.Entities;


public class SceneObject : Entity<SceneObjectId>
{
    public AssetId AssetId { get; }
    public Position3D Position { get; private set; }
    public Rotation Rotation { get; private set; }
    public Scale Scale { get; private set; }
    public ObjectLayerId LayerId { get; private set; }
    public BoundingBox BoundingBox { get; private set; }

    private SceneObject()
    {
        AssetId = null!;
        Position = null!;
        Rotation = null!;
        Scale = null!;
        BoundingBox = null!;
        LayerId = null!;
    }
    
    public SceneObject(
        SceneObjectId id,
        AssetId assetId,
        Position3D position,
        Rotation rotation,
        Scale scale,
        ObjectLayerId layerId) : base(id)
    {
        AssetId = assetId;
        Position = position;
        Rotation = rotation;
        Scale = scale;
        LayerId = layerId;
        BoundingBox = new BoundingBox(position, scale);
    }

    public void MoveTo(Position3D newPosition)
    {
        Position = newPosition;
        BoundingBox = new BoundingBox(newPosition, Scale);
    }

    public void ChangeLayer(ObjectLayerId layerId) => LayerId = layerId;
}