using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.ValueObjects;
using SharedKernel.Abstractions;
using SharedKernel.Identity;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Domain.Repositories;

public interface IProjectRepository : IRepository<Project, ProjectId>
{
    Task<IReadOnlyList<Project>> GetByUserIdAsync(UserId userId, CancellationToken ct = default);
    Task<IReadOnlyList<Project>> GetByObserverIdAsync(UserId observerId, CancellationToken ct = default);
}