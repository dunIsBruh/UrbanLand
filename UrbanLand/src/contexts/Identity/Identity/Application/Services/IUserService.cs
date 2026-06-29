using Core.Identity;
using Core.Primitives;
using Identity.Presentation.Models.Requests;
using Identity.Presentation.Models.Responses;

namespace Identity.Application.Services;

public interface IUserService
{
    Task<Result<UserResponse>> GetAsync(UserId userId, CancellationToken ct = default);
    Task<Result<UserResponse>> UpdateProfileAsync(UserId userId, UpdateProfileRequest request, CancellationToken ct = default);
    Task<Result> ChangePasswordAsync(UserId userId, ChangePasswordRequest request, CancellationToken ct = default);
}