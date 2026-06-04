using System.Text.Json;
using System.Text.Json.Serialization;
using AssetCatalog.Domain.ValueObjects;
using AssetCatalog.Application.Services;
using Microsoft.Extensions.Logging;

namespace AssetCatalog.Infrastructure.Services;

public class SketchfabService(
    HttpClient httpClient,
    ILogger<SketchfabService> logger)
    : ISketchfabService
{
    public async Task<ModelData?> DownloadModelAsync(string sketchfabModelId)
    {
        try
        {
            var response = await httpClient.GetAsync(
                $"https://api.sketchfab.com/v3/models/{sketchfabModelId}");

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Sketchfab API returned {StatusCode} for model {ModelId}",
                    response.StatusCode, sketchfabModelId);
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var model = JsonSerializer.Deserialize<SketchfabModelResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (model?.Data == null)
                return null;

            return new ModelData(
                $"https://sketchfab.com/models/{sketchfabModelId}/download",
                $"{model.Data.Name}.glb",
                0,
                "glb",
                new Dimensions(
                    model.Data.BoundingBox?.Width ?? 1,
                    model.Data.BoundingBox?.Height ?? 1,
                    model.Data.BoundingBox?.Depth ?? 1),
                model.Data.FaceCount ?? 0);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to download model {ModelId} from Sketchfab", sketchfabModelId);
            return null;
        }
    }

    private class SketchfabModelResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("data")]
        public SketchfabModelData? Data { get; set; }
    }

    private class SketchfabModelData
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("faceCount")]
        public int? FaceCount { get; set; }

        [JsonPropertyName("boundingBox")]
        public SketchfabBoundingBox? BoundingBox { get; set; }
    }

    private class SketchfabBoundingBox
    {
        [JsonPropertyName("width")]
        public double Width { get; set; }

        [JsonPropertyName("height")]
        public double Height { get; set; }

        [JsonPropertyName("depth")]
        public double Depth { get; set; }
    }
}
