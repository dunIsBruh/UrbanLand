using MediatR;
using ProjectManagement.Domain.ValueObjects;
using SharedKernel.Primitives;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Application.Commands.UpdateSettings;

public record UpdateSettingsCommand(
    ProjectId ProjectId,
    double DefaultGridSize,
    bool ShowGrid,
    string DefaultTerrainType,
    int MaxObjectsLimit,
    int InvitationValidityHours) : IRequest<Result>;
