namespace SceneDesign.Presentation.Models.Responses;

/// <summary>
/// Response returned after a scene is created.
/// </summary>
public sealed record CreateSceneResponse
{
    /// <summary>
    /// Unique identifier of the created scene.
    /// </summary>
    public Guid SceneId { get; init; }

    /// <summary>
    /// Identifier of the project the scene belongs to.
    /// </summary>
    public Guid ProjectId { get; init; }
}
