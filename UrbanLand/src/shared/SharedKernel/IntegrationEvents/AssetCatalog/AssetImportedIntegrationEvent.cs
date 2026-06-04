using SharedKernel.Abstractions;

namespace SharedKernel.IntegrationEvents.AssetCatalog;

public sealed record AssetImportedIntegrationEvent(
    Guid AssetId,
    string Name,
    Guid UploaderId) : IntegrationEvent;
