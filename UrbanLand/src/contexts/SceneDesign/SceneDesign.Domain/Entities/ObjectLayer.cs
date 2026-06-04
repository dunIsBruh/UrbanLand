using SceneDesign.Domain.ValueObjects;
using SharedKernel.Abstractions;

namespace SceneDesign.Domain.Entities;

public class ObjectLayer : Entity<ObjectLayerId>
{
    public string Name { get; }
    public int Order { get; }
    public bool IsVisible { get; private set; }
    public bool IsLocked { get; private set; }

    // EF Core
    private ObjectLayer()
    {
        Name = null!;
    } 

    public ObjectLayer(ObjectLayerId id, string name, int order) : base(id)
    {
        Name = name;
        Order = order;
        IsVisible = true;
        IsLocked = false;
    }
    
    public static ObjectLayer Create(string name, int order) => new(ObjectLayerId.New(), name, order);
}