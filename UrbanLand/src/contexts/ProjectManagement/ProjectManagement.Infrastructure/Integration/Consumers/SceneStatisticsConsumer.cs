using MassTransit;
using Microsoft.Extensions.Logging;
using ProjectManagement.Domain.Repositories;
using ProjectManagement.Domain.ValueObjects;
using Core.IntegrationEvents.SceneDesign;
using ProjectId = Core.Contracts.ProjectId;

namespace ProjectManagement.Infrastructure.Integration.Consumers;

public class SceneStatisticsConsumer(
    IProjectRepository projectRepository,
    ILogger<SceneStatisticsConsumer> logger)
    : IConsumer<SceneStatisticsUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<SceneStatisticsUpdatedIntegrationEvent> context)
    {
        var message = context.Message;
        
        logger.LogInformation(
            "Received scene statistics update: ProjectId={ProjectId}, Objects={ObjectCount}",
            message.ProjectId, 
            message.TotalObjects);

        var project = await projectRepository.GetByIdAsync(ProjectId.From(message.ProjectId));
        
        if (project == null)
        {
            logger.LogWarning(
                "Project {ProjectId} not found for statistics update. Skipping.",
                message.ProjectId);
            return;
        }

        // Здесь можно обновить статистику проекта
        // Например, project.UpdateStatistics(message.TotalObjects);
        
        await projectRepository.SaveAsync(project, context.CancellationToken);
        
        logger.LogInformation(
            "Updated statistics for project {ProjectId}", 
            message.ProjectId);
    }
}