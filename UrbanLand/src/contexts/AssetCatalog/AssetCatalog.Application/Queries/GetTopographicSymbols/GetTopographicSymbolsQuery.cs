using MediatR;
using SharedKernel.Primitives;

namespace AssetCatalog.Application.Queries.GetTopographicSymbols;

public record GetTopographicSymbolsQuery() : IRequest<Result<List<TopographicSymbolDto>>>;
