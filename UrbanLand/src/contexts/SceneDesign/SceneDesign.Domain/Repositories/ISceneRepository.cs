using Core.Abstractions;
using Core.Contracts;
using SceneDesign.Domain.Entities;
using SceneDesign.Domain.ValueObjects;

namespace SceneDesign.Domain.Repositories;

public interface ISceneRepository : IRepository<Scene, SceneId>
{
    Task<Scene?> GetByProjectIdAsync(ProjectId projectId, CancellationToken ct = default);
}