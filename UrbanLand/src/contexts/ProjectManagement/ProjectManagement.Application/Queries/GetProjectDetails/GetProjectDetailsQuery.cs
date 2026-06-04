using MediatR;
using ProjectManagement.Domain.ValueObjects;
using SharedKernel.Primitives;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Application.Queries.GetProjectDetails;

public record GetProjectDetailsQuery(ProjectId ProjectId) : IRequest<Result<ProjectDetailsDto>>;
