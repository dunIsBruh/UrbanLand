using Core.Primitives;
using MediatR;
using ProjectManagement.Domain.ValueObjects;
using Core.Primitives;
using ProjectId = Core.Contracts.ProjectId;

namespace ProjectManagement.Application.Queries.GetProject;

public record GetProjectQuery(ProjectId ProjectId) : IRequest<Result<ProjectDto>>;
