using Identity.Presentation.Models.Responses;
using SharedKernel.Primitives;

namespace Identity.Application.Services;

public interface IAdminService
{
    Task<List<UserResponse>> GetUsersAsync(CancellationToken ct = default);

    Task<Result<UserResponse>> SetUserRoleAsync(Guid userId, string role, CancellationToken ct = default);
}