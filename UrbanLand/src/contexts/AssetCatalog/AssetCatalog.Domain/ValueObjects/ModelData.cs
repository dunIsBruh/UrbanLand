using Core.Abstractions;

namespace AssetCatalog.Domain.ValueObjects;

public record ModelData : ValueObject
{
    public string FileUrl { get; }
    public string FileName { get; }
    public long FileSize { get; }
    public string Format { get; }
    public Dimensions Dimensions { get; }
    public int PolygonCount { get; }

    private ModelData() { }

    public ModelData(
        string fileUrl,
        string fileName,
        long fileSize,
        string format,
        Dimensions dimensions,
        int polygonCount)
    {
        FileUrl = fileUrl;
        FileName = fileName;
        FileSize = fileSize;
        Format = format;
        Dimensions = dimensions;
        PolygonCount = polygonCount;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FileUrl;
        yield return FileName;
        yield return Format;
        yield return Dimensions;
    }
}