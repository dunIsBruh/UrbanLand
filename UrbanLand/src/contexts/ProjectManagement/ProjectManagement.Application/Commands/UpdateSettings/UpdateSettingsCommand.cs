using Core.Primitives;
using MediatR;
using ProjectManagement.Domain.ValueObjects;
using Core.Primitives;
using ProjectId = Core.Contracts.ProjectId;

namespace ProjectManagement.Application.Commands.UpdateSettings;

public record UpdateSettingsCommand(
    ProjectId ProjectId,
    double DefaultGridSize,
    bool ShowGrid,
    string DefaultTerrainType,
    int MaxObjectsLimit,
    int InvitationValidityHours) : IRequest<Result>;
