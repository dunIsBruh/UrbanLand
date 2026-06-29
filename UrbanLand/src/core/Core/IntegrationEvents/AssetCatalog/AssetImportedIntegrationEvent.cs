using Core.Abstractions;

namespace Core.IntegrationEvents.AssetCatalog;

public sealed record AssetImportedIntegrationEvent(
    Guid AssetId,
    string Name,
    Guid UploaderId) : IntegrationEvent;
