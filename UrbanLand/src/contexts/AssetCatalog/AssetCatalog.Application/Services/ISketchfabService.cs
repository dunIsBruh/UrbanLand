using AssetCatalog.Domain.ValueObjects;

namespace AssetCatalog.Application.Services;

public interface ISketchfabService
{
    Task<ModelData?> DownloadModelAsync(string sketchfabModelId);
}
