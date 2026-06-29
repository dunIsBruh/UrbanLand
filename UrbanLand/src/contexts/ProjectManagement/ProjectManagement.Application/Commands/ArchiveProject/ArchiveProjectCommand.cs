using Core.Primitives;
using MediatR;
using ProjectManagement.Domain.ValueObjects;
using Core.Primitives;
using ProjectId = Core.Contracts.ProjectId;

namespace ProjectManagement.Application.Commands.ArchiveProject;

public record ArchiveProjectCommand(ProjectId ProjectId) : IRequest<Result>;
