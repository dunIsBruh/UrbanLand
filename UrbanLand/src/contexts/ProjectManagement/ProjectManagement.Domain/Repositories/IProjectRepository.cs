using Core.Abstractions;
using Core.Identity;
using ProjectManagement.Domain.Entities;
using ProjectId = Core.Contracts.ProjectId;

namespace ProjectManagement.Domain.Repositories;

public interface IProjectRepository : IRepository<Project, ProjectId>
{
    Task<IReadOnlyList<Project>> GetByUserIdAsync(UserId userId, CancellationToken ct = default);
    Task<IReadOnlyList<Project>> GetByObserverIdAsync(UserId observerId, CancellationToken ct = default);
}