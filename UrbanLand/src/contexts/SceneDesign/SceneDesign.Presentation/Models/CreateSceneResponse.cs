namespace SceneDesign.Presentation.Models;

public sealed record CreateSceneResponse
{
    public Guid SceneId { get; init; }
    public Guid ProjectId { get; init; }
}
