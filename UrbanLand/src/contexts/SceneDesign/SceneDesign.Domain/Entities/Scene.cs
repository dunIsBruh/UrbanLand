using Core.Abstractions;
using Core.Contracts;
using Core.Primitives;
using SceneDesign.Domain.Enums;
using SceneDesign.Domain.Events;
using SceneDesign.Domain.ValueObjects;

namespace SceneDesign.Domain.Entities;

public class Scene : AggregateRoot<SceneId>
{
    private readonly List<SceneObject> _objects = [];
    private readonly List<ObjectLayer> _layers = [];

    public ProjectId ProjectId { get; private set; }
    public SceneSettings Settings { get; private set; }
    public ViewMode CurrentViewMode { get; private set; }
    public int Version { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsLocked { get; private set; }

    public IReadOnlyCollection<SceneObject> Objects => _objects.AsReadOnly();
    public IReadOnlyCollection<ObjectLayer> Layers => _layers.AsReadOnly();

    private Scene()
    {
        ProjectId = null!;
        Settings = null!;
    }

    private Scene(SceneId id, ProjectId projectId)
        : base(id)
    {
        ProjectId = projectId;
        Settings = SceneSettings.Default(SceneType.Park);
        CurrentViewMode = ViewMode.TopDown2D;
        CreatedAt = DateTime.UtcNow;
        Version = 1;

        _layers.Add(ObjectLayer.Create("Terrain", 0));
        _layers.Add(ObjectLayer.Create("Buildings", 1));
        _layers.Add(ObjectLayer.Create("Vegetation", 2));
        _layers.Add(ObjectLayer.Create("Infrastructure", 3));
    }

    public static Result<Scene> CreateForProject(ProjectId projectId)
    {
        var scene = new Scene(SceneId.New(), projectId);

        scene.AddDomainEvent(new SceneCreatedDomainEvent(scene.Id, projectId));

        return Result<Scene>.Success(scene);
    }

    public Result<SceneObject> PlaceObject(
        AssetId assetId,
        Position3D position,
        Rotation rotation,
        Scale scale,
        string? layerName = null)
    {
        if (_objects.Count >= Settings.MaxObjectLimit)
        {
            return Result<SceneObject>.Failure(
                Error.Validation($"Maximum {Settings.MaxObjectLimit} objects reached"));
        }

        if (IsLocked)
        {
            return Result<SceneObject>.Failure(Error.Conflict("Scene is locked"));
        }

        var targetLayer = GetOrCreateLayer(layerName ?? "Default");

        var sceneObject = new SceneObject(
            SceneObjectId.New(),
            assetId,
            position,
            rotation,
            scale,
            targetLayer.Id);

        _objects.Add(sceneObject);
        // Version++;

        AddDomainEvent(new ObjectPlacedDomainEvent(Id, sceneObject.Id, assetId, position));

        return Result<SceneObject>.Success(sceneObject);
    }

    public Result MoveObject(SceneObjectId objectId, Position3D newPosition)
    {
        var obj = _objects.FirstOrDefault(o => o.Id == objectId);
        if (obj == null)
        {
            return Result.Failure(Error.NotFound("SceneObject", objectId));
        }

        if (IsLocked)
        {
            return Result.Failure(Error.Conflict("Scene is locked"));
        }

        obj.MoveTo(newPosition);
        // Version++;

        AddDomainEvent(new ObjectMovedDomainEvent(Id, objectId, obj.Position, newPosition));

        return Result.Success();
    }

    public Result RemoveObject(SceneObjectId objectId)
    {
        var obj = _objects.FirstOrDefault(o => o.Id == objectId);
        if (obj == null)
        {
            return Result.Failure(Error.NotFound("SceneObject", objectId));
        }

        _objects.Remove(obj);
        // Version++;

        AddDomainEvent(new ObjectRemovedDomainEvent(Id, objectId, obj.AssetId));

        return Result.Success();
    }

    public Result SwitchViewMode(ViewMode newMode)
    {
        if (CurrentViewMode == newMode)
            return Result.Success();

        var oldMode = CurrentViewMode;
        CurrentViewMode = newMode;

        AddDomainEvent(new ViewModeChangedDomainEvent(Id, oldMode, newMode));

        return Result.Success();
    }

    public Result Lock()
    {
        if (IsLocked)
        {
            return Result.Failure(Error.Conflict("Scene is already locked"));
        }

        IsLocked = true;
        return Result.Success();
    }

    public Result Unlock()
    {
        if (!IsLocked)
        {
            return Result.Failure(Error.Conflict("Scene is not locked"));
        }

        IsLocked = false;
        return Result.Success();
    }

    private ObjectLayer GetOrCreateLayer(string layerName)
    {
        var layer = _layers.FirstOrDefault(l =>
            l.Name.Equals(layerName, StringComparison.OrdinalIgnoreCase));

        if (layer != null)
        {
            return layer;
        }

        layer = ObjectLayer.Create(layerName, _layers.Count);
        _layers.Add(layer);

        return layer;
    }
}
