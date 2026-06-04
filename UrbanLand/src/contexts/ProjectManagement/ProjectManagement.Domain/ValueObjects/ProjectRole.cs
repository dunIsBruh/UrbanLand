using ProjectManagement.Domain.Enums;
using SharedKernel.Abstractions;

namespace ProjectManagement.Domain.ValueObjects;

public record ProjectRole : ValueObject
{
    public AccessLevel Level { get; }
    public string Name { get; }
    
    private ProjectRole(AccessLevel level, string name)
    {
        Level = level;
        Name = name;
    }
    
    public static readonly ProjectRole RoleManager = new(AccessLevel.RoleManager, "Manager");
    public static readonly ProjectRole Builder = new(AccessLevel.Builder, "Builder");
    public static readonly ProjectRole Commentator = new(AccessLevel.Commentator, "Commentator");
    public static readonly ProjectRole Visitor = new(AccessLevel.Visitor, "Visitor");

    public static ProjectRole FromName(string name) => name switch
    {
        "Manager" => RoleManager,
        "Builder" => Builder,
        "Commentator" => Commentator,
        _ => Visitor
    };
    
    public bool CanManageRoles() => this == RoleManager;
    public bool CanEditScene() => this == RoleManager || this == Builder;
    public bool CanComment() => CanEditScene() || this == Commentator;
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Level;
    }
}