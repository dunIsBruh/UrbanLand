using Core.Primitives;
using MediatR;
using ProjectManagement.Domain.ValueObjects;
using Core.Primitives;
using ProjectId = Core.Contracts.ProjectId;

namespace ProjectManagement.Application.Commands.DeleteProject;

public record DeleteProjectCommand(ProjectId ProjectId) : IRequest<Result>;
