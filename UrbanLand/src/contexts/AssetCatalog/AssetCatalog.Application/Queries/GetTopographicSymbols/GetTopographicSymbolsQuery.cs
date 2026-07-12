using MediatR;
using Core.Primitives;

namespace AssetCatalog.Application.Queries.GetTopographicSymbols;

public record GetTopographicSymbolsQuery() : IRequest<Result<List<TopographicSymbolDto>>>;
