using MediatR;
using ProjectManagement.Domain.ValueObjects;
using SharedKernel.Primitives;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Application.Commands.DeleteProject;

public record DeleteProjectCommand(ProjectId ProjectId) : IRequest<Result>;
