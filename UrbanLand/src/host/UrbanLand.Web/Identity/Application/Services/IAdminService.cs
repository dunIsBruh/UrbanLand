using SharedKernel.Primitives;
using UrbanLand.Web.Identity.Presentation.Models.Responses;

namespace UrbanLand.Web.Identity.Application.Services;

public interface IAdminService
{
    Task<List<UserResponse>> GetUsersAsync(CancellationToken ct = default);

    Task<Result<UserResponse>> SetUserRoleAsync(Guid userId, string role, CancellationToken ct = default);
}