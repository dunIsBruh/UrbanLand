using MediatR;
using ProjectManagement.Domain.ValueObjects;
using SharedKernel.Primitives;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Application.Queries.GetProjectMembers;

public record GetProjectMembersQuery(ProjectId ProjectId) : IRequest<Result<List<ProjectMemberDto>>>;
