using Core.Abstractions;
using SceneDesign.Domain.Enums;

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

