using Identity.Presentation.Models.Requests;
using Identity.Presentation.Models.Responses;
using SharedKernel.Primitives;

namespace Identity.Application.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default);
}