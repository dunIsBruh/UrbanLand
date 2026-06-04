namespace SceneDesign.Presentation.Models;

/// <summary>
/// Response returned after placing an object in the scene.
/// </summary>
public sealed record PlaceObjectResponse
{
    /// <summary>
    /// Unique identifier of the placed object.
    /// </summary>
    public Guid ObjectId { get; init; }
}
