using ProjectManagement.Domain.Enums;
using ProjectManagement.Domain.ValueObjects;
using SharedKernel.Identity;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Application.Queries.GetProject;

public record ProjectDto(
    ProjectId Id,
    string Name,
    UserId OwnerId,
    ProjectStatus Status,
    DateTime CreatedAt,
    int MemberCount);
