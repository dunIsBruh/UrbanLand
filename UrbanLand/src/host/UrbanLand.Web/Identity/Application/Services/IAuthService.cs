using SharedKernel.Primitives;
using UrbanLand.Web.Identity.Presentation.Models.Requests;
using UrbanLand.Web.Identity.Presentation.Models.Responses;

namespace UrbanLand.Web.Identity.Application.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default);
}