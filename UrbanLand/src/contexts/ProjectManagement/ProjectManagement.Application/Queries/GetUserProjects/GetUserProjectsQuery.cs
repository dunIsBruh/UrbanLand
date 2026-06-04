using MediatR;
using SharedKernel.Primitives;

namespace ProjectManagement.Application.Queries.GetUserProjects;

public record GetUserProjectsQuery(int Page = 1, int PageSize = 20) : IRequest<Result<List<UserProjectDto>>>;
