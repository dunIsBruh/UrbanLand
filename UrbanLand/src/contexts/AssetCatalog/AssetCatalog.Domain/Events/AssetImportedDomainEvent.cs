using AssetCatalog.Domain.ValueObjects;
using SharedKernel.Abstractions;
using SharedKernel.Identity;

namespace AssetCatalog.Domain.Events;

public sealed record AssetImportedDomainEvent(
    AssetId AssetId,
    UserId UploaderId,
    string AssetName) : DomainEvent;