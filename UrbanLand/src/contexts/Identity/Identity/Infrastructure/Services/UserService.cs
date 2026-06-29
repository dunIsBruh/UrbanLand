// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Caching.Memory;
// using Core.Identity;
// using UrbanLand.Web.Identity.Application.Entities;
//
// namespace UrbanLand.Web.Identity.Application.Services;
//
// public class UserService(
//     UserManager<ApplicationUser> userManager,
//     IMemoryCache cache,
//     ILogger<UserService> logger
//     ) : ProjectManagement.Domain.Services.IUserService,
//         SceneDesign.Domain.Services.IUserService,
//         AssetCatalog.Domain.Services.IUserService
// {
//     private readonly ILogger<UserService> _logger = logger;
//
//     public async Task<bool> UserExistsAsync(UserId userId)
//     {
//         var cacheKey = $"user_exists_{userId.Value}";
//         
//         return await cache.GetOrCreateAsync(cacheKey, async entry =>
//         {
//             entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
//             
//             var user = await userManager.FindByIdAsync(userId.Value.ToString());
//             return user != null;
//         });
//     }
//
//     public async Task<UserInfo?> GetUserInfoAsync(UserId userId)
//     {
//         var cacheKey = $"user_info_{userId.Value}";
//         
//         return await cache.GetOrCreateAsync(cacheKey, async entry =>
//         {
//             entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
//             
//             var user = await userManager.FindByIdAsync(userId.Value.ToString());
//             
//             if (user == null)
//             {
//                 return null;
//             }
//
//             return new UserInfo(user.Id, user.DisplayName, user.Email, user.AvatarUrl);
//         });
//     }
//
//     public async Task<Dictionary<UserId, UserInfo>> GetUsersInfoAsync(IEnumerable<UserId> userIds)
//     {
//         var ids = userIds.Select(id => id.Value).ToList();
//         
//         var users = await userManager.Users
//             .Where(u => ids.Contains(u.Id))
//             .Select(u => new { u.Id, u.DisplayName, u.Email, u.AvatarUrl })
//             .ToListAsync();
//
//         return users.ToDictionary(
//             u => new UserId(u.Id),
//             u => new UserInfo(
//                 u.Id,
//                 u.DisplayName,
//                 u.Email,
//                 u.AvatarUrl
//             ));
//     }
// }
