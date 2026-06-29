using AssetCatalog.Domain.ValueObjects;
using Core.Abstractions;
using Core.Identity;

namespace AssetCatalog.Domain.Events;

public sealed record AssetImportedDomainEvent(
    AssetId AssetId,
    UserId UploaderId,
    string AssetName) : DomainEvent;