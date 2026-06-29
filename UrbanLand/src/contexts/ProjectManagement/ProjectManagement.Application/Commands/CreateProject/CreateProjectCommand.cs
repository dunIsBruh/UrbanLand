using Core.Primitives;
using MediatR;
using ProjectManagement.Domain.ValueObjects;
using Core.Primitives;
using ProjectId = Core.Contracts.ProjectId;

namespace ProjectManagement.Application.Commands.CreateProject;

public record CreateProjectCommand(string Name, string SceneType, string? Description = null)
    : IRequest<Result<ProjectId>>;
