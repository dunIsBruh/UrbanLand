using SceneDesign.Domain.Entities;
using SceneDesign.Domain.ValueObjects;
using SharedKernel.Abstractions;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace SceneDesign.Domain.Repositories;

public interface ISceneRepository : IRepository<Scene, SceneId>
{
    Task<Scene?> GetByProjectIdAsync(ProjectId projectId, CancellationToken ct = default);
}