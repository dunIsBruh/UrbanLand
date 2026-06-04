using SceneDesign.Domain.Enums;
using SharedKernel.Abstractions;

namespace SceneDesign.Domain.ValueObjects;

public record SceneSettings(int MaxObjectLimit, bool DetectCollisions) : ValueObject
{
    // TODO: вопрос: добавлять ли сетку и ее размер?
    public static SceneSettings Default(SceneType type) => type switch
    {
        SceneType.Park
            => new SceneSettings(2000, false),
        SceneType.CottageVillage
            => new SceneSettings(1000, true),
        _ => new SceneSettings(3000, true)
    };

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return MaxObjectLimit;
        yield return DetectCollisions;
    }
}

// public record ProjectSettings : ValueObject
// {
//     public double DefaultGridSize { get; }
//     public bool ShowGrid { get; }
//     public int MaxObjectsLimit { get; }
//
//     private ProjectSettings(
//         double defaultGridSize,
//         bool showGrid,
//         int maxObjectsLimit)
//     {
//         DefaultGridSize = defaultGridSize;
//         ShowGrid = showGrid;
//         MaxObjectsLimit = maxObjectsLimit;
//     }
//
//     public static ProjectSettings CreateDefault(ProjectType type) => type switch
//     {
//         ProjectType.Park 
//             => new ProjectSettings(
//             1.0,
//             true,
//             5000),
//         ProjectType.ResidentialComplex 
//             => new ProjectSettings(
//             0.5, 
//             true, 
//             10000),
//         ProjectType.CottageVillage 
//             => new ProjectSettings(
//                 2.0,
//                 true,
//                 3000
//                 ),
//         _ 
//             => new ProjectSettings(
//                 1.0,
//                 true,
//                 5000
//                 )
//     };
// }