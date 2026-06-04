using MediatR;
using ProjectManagement.Domain.ValueObjects;
using SharedKernel.Primitives;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Application.Commands.ArchiveProject;

public record ArchiveProjectCommand(ProjectId ProjectId) : IRequest<Result>;
