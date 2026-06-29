using Core.Primitives;
using MediatR;
using Core.Primitives;

namespace ProjectManagement.Application.Queries.GetUserProjects;

public record GetUserProjectsQuery(int Page = 1, int PageSize = 20) : IRequest<Result<List<UserProjectDto>>>;
