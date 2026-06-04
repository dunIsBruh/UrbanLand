using ProjectManagement.Domain.ValueObjects;
using SharedKernel.Abstractions;
using SharedKernel.Identity;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Domain.Events;

public sealed record ProjectCreatedDomainEvent(
    ProjectId ProjectId,
    string Name,
    UserId OwnerId) : DomainEvent;