using Core.Primitives;
using MediatR;
using ProjectManagement.Domain.ValueObjects;
using Core.Primitives;
using ProjectId = Core.Contracts.ProjectId;

namespace ProjectManagement.Application.Queries.GetProjectDetails;

public record GetProjectDetailsQuery(ProjectId ProjectId) : IRequest<Result<ProjectDetailsDto>>;
