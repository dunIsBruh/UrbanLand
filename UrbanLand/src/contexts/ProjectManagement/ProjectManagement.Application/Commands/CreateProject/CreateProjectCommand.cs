using MediatR;
using ProjectManagement.Domain.ValueObjects;
using SharedKernel.Primitives;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Application.Commands.CreateProject;

public record CreateProjectCommand(string Name, string SceneType, string? Description = null)
    : IRequest<Result<ProjectId>>;
