using Core.Primitives;
using MediatR;
using ProjectManagement.Domain.ValueObjects;
using Core.Primitives;
using ProjectId = Core.Contracts.ProjectId;

namespace ProjectManagement.Application.Queries.GetProjectMembers;

public record GetProjectMembersQuery(ProjectId ProjectId) : IRequest<Result<List<ProjectMemberDto>>>;
