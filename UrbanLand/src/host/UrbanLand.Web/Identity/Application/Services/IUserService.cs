using SharedKernel.Identity;
using SharedKernel.Primitives;
using UrbanLand.Web.Identity.Presentation.Models.Requests;
using UrbanLand.Web.Identity.Presentation.Models.Responses;

namespace UrbanLand.Web.Identity.Application.Services;

public interface IUserService
{
    Task<Result<UserResponse>> GetAsync(UserId userId, CancellationToken ct = default);
    Task<Result<UserResponse>> UpdateProfileAsync(UserId userId, UpdateProfileRequest request, CancellationToken ct = default);
    Task<Result> ChangePasswordAsync(UserId userId, ChangePasswordRequest request, CancellationToken ct = default);
}