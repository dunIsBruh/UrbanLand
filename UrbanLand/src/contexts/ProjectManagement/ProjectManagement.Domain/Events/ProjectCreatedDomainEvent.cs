using Core.Abstractions;
using Core.Identity;
using ProjectId = Core.Contracts.ProjectId;

namespace ProjectManagement.Domain.Events;

public sealed record ProjectCreatedDomainEvent(
    ProjectId ProjectId,
    string Name,
    UserId OwnerId) : DomainEvent;