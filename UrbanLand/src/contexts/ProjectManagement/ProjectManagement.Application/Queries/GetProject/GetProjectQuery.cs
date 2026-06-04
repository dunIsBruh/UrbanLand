using MediatR;
using ProjectManagement.Domain.ValueObjects;
using SharedKernel.Primitives;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Application.Queries.GetProject;

public record GetProjectQuery(ProjectId ProjectId) : IRequest<Result<ProjectDto>>;
