using AssetCatalog.Domain.Enums;
using AssetCatalog.Domain.Events;
using AssetCatalog.Domain.ValueObjects;
using Core.Abstractions;
using Core.Exceptions;
using Core.Identity;
using Core.Primitives;

namespace AssetCatalog.Domain.Entities;

public class AssetTemplate : AggregateRoot<AssetId>
{
    private readonly List<AssetVersion> _versions = [];
    private readonly List<string> _tags = [];

    public string Name { get; private set; }
    public string Description { get; private set; }
    public AssetCategory Category { get; private set; }
    public bool IsCustom { get; private set; }
    public UserId UploaderId { get; private set; }
    public AssetStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public TopographicSymbol? TopographicSymbol { get; private set; }

    public IReadOnlyCollection<AssetVersion> Versions => _versions.AsReadOnly();
    public IReadOnlyCollection<string> Tags => _tags.AsReadOnly();
    public AssetVersion CurrentVersion => _versions.OrderByDescending(v => v.VersionNumber).First();

    // EF Core
    private AssetTemplate()
    {
        Name = null!;
        Description = null!;
        UploaderId = null!;
    }

    private AssetTemplate(AssetId id, string name, UserId uploaderId, string? description = null) : base(id)
    {
        Name = name;
        UploaderId = uploaderId;
        Description = description ?? string.Empty;
    }

    public static Result<AssetTemplate> ImportCustom(
        string name,
        string? description,
        UserId uploaderId,
        ModelData model)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<AssetTemplate>.Failure(Error.Validation("Template name is required"));
        }
        
        if (name.Length > 200)
        {
            throw new DomainException("Asset template name must be less than 200 characters"); 
        }

        if (model.FileSize > 100 * 1024 * 1024)
        {
            return Result<AssetTemplate>.Failure(Error.Validation("File size exceeds 100MB"));
        }

        var asset = new AssetTemplate(AssetId.New(), name, uploaderId, description)
        {
            Category = AssetCategory.Custom,
            IsCustom = true,
            Status = AssetStatus.Processing,
            CreatedAt = DateTime.UtcNow
        };

        asset._tags.Add("custom");
        asset._versions.Add(new AssetVersion(1, model, DateTime.UtcNow));

        asset.AddDomainEvent(new AssetImportedDomainEvent(asset.Id, uploaderId, name));

        return Result<AssetTemplate>.Success(asset);
    }

    public Result AddVersion(ModelData model)
    {
        if (Status != AssetStatus.Active)
        {
            return Result.Failure(Error.Validation("Can only add versions to active assets"));
        }

        var newVersion = new AssetVersion(_versions.Count + 1, model, DateTime.UtcNow);
        _versions.Add(newVersion);

        return Result.Success();
    }

    public static Result<AssetTemplate> CreateTopographicSymbol(
        string name,
        string description,
        UserId uploaderId,
        TopographicSymbol symbol)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<AssetTemplate>.Failure(Error.Validation("Template name is required"));
        }

        var asset = new AssetTemplate(AssetId.New(), name, uploaderId, description)
        {
            Category = AssetCategory.Decoration,
            IsCustom = false,
            Status = AssetStatus.Active,
            CreatedAt = DateTime.UtcNow,
            TopographicSymbol = symbol
        };

        asset._tags.Add("topographic");

        asset.AddDomainEvent(new AssetImportedDomainEvent(asset.Id, uploaderId, name));

        return Result<AssetTemplate>.Success(asset);
    }

    public void Approve() => Status = AssetStatus.Active;
    public void Reject() => Status = AssetStatus.Rejected;
    public void Retire() => Status = AssetStatus.Retired;
}
