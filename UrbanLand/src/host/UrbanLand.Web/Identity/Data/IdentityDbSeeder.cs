using Microsoft.AspNetCore.Identity;

namespace UrbanLand.Web.Identity.Data;

public static class DbSeeder
{
    public static async Task SeedIdentityAsync(
        IdentityDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>
            {
                Id = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000001"),
                Name = "User",
                NormalizedName = "USER"
            });
        }

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>
            {
                Id = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000002"),
                Name = "Admin",
                NormalizedName = "ADMIN"
            });
        }

        // Создание администратора, если нет пользователей
        if (!dbContext.Users.Any())
        {
            var adminUser = new ApplicationUser
            {
                Id = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000100"),
                UserName = "admin@urbanland.com",
                Email = "admin@urbanland.com",
                DisplayName = "System Administrator",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                AvatarUrl = null
            };

            var createResult = await userManager.CreateAsync(adminUser, "Admin123!");
            if (createResult.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                await userManager.AddToRoleAsync(adminUser, "User");
            }

            var regularUser = new ApplicationUser
            {
                Id = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000101"),
                UserName = "user@urbanland.com",
                Email = "user@urbanland.com",
                DisplayName = "Regular User",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                AvatarUrl = null
            };

            createResult = await userManager.CreateAsync(regularUser, "User123!");
            if (createResult.Succeeded)
            {
                await userManager.AddToRoleAsync(regularUser, "User");
            }
        }
    }

    // public static async Task SeedProjectManagementAsync(
    //     ProjectManagement.Infrastructure.Persistence.ProjectManagementDbContext dbContext)
    // {
    //     if (dbContext.Projects.Any())
    //         return;
    //
    //     // Получаем ID пользователей (предполагаем, что они уже созданы в Identity)
    //     var adminId = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000100");
    //     var userId = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000101");
    //
    //     var projects = new List<Project>
    //     {
    //         new Project(
    //             name: "Urban Landscape Platform",
    //             description: "Main platform for 3D urban planning",
    //             ownerId: adminId,
    //             settings: new ProjectSettings()),
    //         
    //         new Project(
    //             name: "Smart City Initiative",
    //             description: "IoT integration for city management",
    //             ownerId: adminId,
    //             settings: new ProjectSettings()),
    //         
    //         new Project(
    //             name: "Green Spaces Design",
    //             description: "Parks and recreation areas planning",
    //             ownerId: userId,
    //             settings: new ProjectSettings())
    //     };
    //
    //     await dbContext.Projects.AddRangeAsync(projects);
    //
    //     // Добавляем участников проектов
    //     var members = new List<ProjectMember>
    //     {
    //         new ProjectMember(projects[0].Id, userId, ProjectRole.Viewer),
    //         new ProjectMember(projects[1].Id, userId, ProjectRole.Contributor),
    //         new ProjectMember(projects[2].Id, adminId, ProjectRole.Owner)
    //     };
    //
    //     await dbContext.Members.AddRangeAsync(members);
    //
    //     await dbContext.SaveChangesAsync();
    // }


    public static async Task SeedAllAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        
        // Identity seeding
        var identityDb = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        await SeedIdentityAsync(identityDb, userManager, roleManager);
    }
}



// using Microsoft.AspNetCore.Identity;
// using ProjectManagement.Domain.Entities;
// using ProjectManagement.Domain.ValueObjects;
// using SceneDesign.Domain.ValueObjects;
//
// namespace UrbanLand.Web.Identity.Data;
//
// public static class DbSeeder
// {
//     public static async Task SeedIdentityAsync(
//         IdentityDbContext dbContext,
//         UserManager<ApplicationUser> userManager,
//         RoleManager<IdentityRole<Guid>> roleManager)
//     {
//         if (!await roleManager.RoleExistsAsync("User"))
//         {
//             await roleManager.CreateAsync(new IdentityRole<Guid>
//             {
//                 Id = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000001"),
//                 Name = "User",
//                 NormalizedName = "USER"
//             });
//         }
//
//         if (!await roleManager.RoleExistsAsync("Admin"))
//         {
//             await roleManager.CreateAsync(new IdentityRole<Guid>
//             {
//                 Id = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000002"),
//                 Name = "Admin",
//                 NormalizedName = "ADMIN"
//             });
//         }
//
//         // Создание администратора, если нет пользователей
//         if (!dbContext.Users.Any())
//         {
//             var adminUser = new ApplicationUser
//             {
//                 Id = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000100"),
//                 UserName = "admin@urbanland.com",
//                 Email = "admin@urbanland.com",
//                 DisplayName = "System Administrator",
//                 EmailConfirmed = true,
//                 CreatedAt = DateTime.UtcNow,
//                 AvatarUrl = null
//             };
//
//             var createResult = await userManager.CreateAsync(adminUser, "Admin123!");
//             if (createResult.Succeeded)
//             {
//                 await userManager.AddToRoleAsync(adminUser, "Admin");
//                 await userManager.AddToRoleAsync(adminUser, "User");
//             }
//
//             var regularUser = new ApplicationUser
//             {
//                 Id = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000101"),
//                 UserName = "user@urbanland.com",
//                 Email = "user@urbanland.com",
//                 DisplayName = "Regular User",
//                 EmailConfirmed = true,
//                 CreatedAt = DateTime.UtcNow,
//                 AvatarUrl = null
//             };
//
//             createResult = await userManager.CreateAsync(regularUser, "User123!");
//             if (createResult.Succeeded)
//             {
//                 await userManager.AddToRoleAsync(regularUser, "User");
//             }
//         }
//     }
//
//     public static async Task SeedProjectManagementAsync(
//         ProjectManagement.Infrastructure.Persistence.ProjectManagementDbContext dbContext)
//     {
//         if (dbContext.Projects.Any())
//             return;
//
//         // Получаем ID пользователей (предполагаем, что они уже созданы в Identity)
//         var adminId = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000100");
//         var userId = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000101");
//
//         var projects = new List<Project>
//         {
//             new Project(
//                 name: "Urban Landscape Platform",
//                 description: "Main platform for 3D urban planning",
//                 ownerId: adminId,
//                 settings: new ProjectSettings()),
//             
//             new Project(
//                 name: "Smart City Initiative",
//                 description: "IoT integration for city management",
//                 ownerId: adminId,
//                 settings: new ProjectSettings()),
//             
//             new Project(
//                 name: "Green Spaces Design",
//                 description: "Parks and recreation areas planning",
//                 ownerId: userId,
//                 settings: new ProjectSettings())
//         };
//
//         await dbContext.Projects.AddRangeAsync(projects);
//
//         // Добавляем участников проектов
//         var members = new List<ProjectMember>
//         {
//             new ProjectMember(projects[0].Id, userId, ProjectRole.Viewer),
//             new ProjectMember(projects[1].Id, userId, ProjectRole.Contributor),
//             new ProjectMember(projects[2].Id, adminId, ProjectRole.Owner)
//         };
//
//         await dbContext.Members.AddRangeAsync(members);
//
//         await dbContext.SaveChangesAsync();
//     }
//
//     public static async Task SeedSceneDesignAsync(
//         SceneDesign.Infrastructure.Persistence.SceneDesignDbContext dbContext)
//     {
//         if (dbContext.Scenes.Any())
//             return;
//
//         var userId = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000101");
//         var adminId = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000100");
//
//         var scenes = new List<SceneDesign.Domain.Entities.Scene>
//         {
//             new SceneDesign.Domain.Entities.Scene(
//                 name: "Downtown City Center",
//                 description: "Main business district with high-rise buildings",
//                 projectId: Guid.NewGuid(), // В реальном проекте используйте реальные ProjectId
//                 ownerId: adminId,
//                 settings: new SceneSettings()),
//             
//             new SceneDesign.Domain.Entities.Scene(
//                 name: "Residential Area Alpha",
//                 description: "Modern housing complex with parks",
//                 projectId: Guid.NewGuid(),
//                 ownerId: userId,
//                 settings: new SceneSettings())
//         };
//
//         await dbContext.Scenes.AddRangeAsync(scenes);
//         await dbContext.SaveChangesAsync();
//     }
//
//     public static async Task SeedAssetCatalogAsync(
//         AssetCatalog.Infrastructure.Persistence.AssetCatalogDbContext dbContext)
//     {
//         if (dbContext.Assets.Any())
//             return;
//
//         var assets = new List<Asset>
//         {
//             new Asset
//             {
//                 Id = Guid.NewGuid(),
//                 Name = "Oak Tree Model",
//                 Type = AssetType.Tree,
//                 Category = "Vegetation",
//                 Tags = new List<string> { "tree", "nature", "oak" },
//                 FileUrl = "https://cdn.urbanland.com/assets/trees/oak.glb",
//                 ThumbnailUrl = "https://cdn.urbanland.com/assets/trees/oak.png",
//                 CreatedBy = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000101"),
//                 CreatedAt = DateTime.UtcNow,
//                 IsPublic = true
//             },
//             new Asset
//             {
//                 Id = Guid.NewGuid(),
//                 Name = "Modern Building Pack",
//                 Type = AssetType.Building,
//                 Category = "Architecture",
//                 Tags = new List<string> { "building", "modern", "city" },
//                 FileUrl = "https://cdn.urbanland.com/assets/buildings/modern-pack.zip",
//                 ThumbnailUrl = "https://cdn.urbanland.com/assets/buildings/modern-pack.png",
//                 CreatedBy = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000100"),
//                 CreatedAt = DateTime.UtcNow,
//                 IsPublic = true
//             },
//             new Asset
//             {
//                 Id = Guid.NewGuid(),
//                 Name = "Park Bench",
//                 Type = AssetType.Furniture,
//                 Category = "Street Furniture",
//                 Tags = new List<string> { "bench", "park", "seating" },
//                 FileUrl = "https://cdn.urbanland.com/assets/furniture/bench.glb",
//                 ThumbnailUrl = "https://cdn.urbanland.com/assets/furniture/bench.png",
//                 CreatedBy = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000101"),
//                 CreatedAt = DateTime.UtcNow,
//                 IsPublic = true
//             }
//         };
//
//         await dbContext.Assets.AddRangeAsync(assets);
//         await dbContext.SaveChangesAsync();
//     }
//
//     // Общий метод для вызова всех сидов
//     public static async Task SeedAllAsync(
//         IServiceProvider serviceProvider)
//     {
//         using var scope = serviceProvider.CreateScope();
//         
//         // Identity seeding
//         var identityDb = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
//         var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
//         var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
//         await SeedIdentityAsync(identityDb, userManager, roleManager);
//         
//         // Project Management seeding
//         var pmDb = scope.ServiceProvider.GetRequiredService<ProjectManagement.Infrastructure.Persistence.ProjectManagementDbContext>();
//         await SeedProjectManagementAsync(pmDb);
//         
//         // Scene Design seeding (раскомментировать, когда добавите)
//         // var sceneDb = scope.ServiceProvider.GetRequiredService<SceneDesign.Infrastructure.Persistence.SceneDesignDbContext>();
//         // await SeedSceneDesignAsync(sceneDb);
//         
//         // Asset Catalog seeding (раскомментировать, когда добавите)
//         // var assetDb = scope.ServiceProvider.GetRequiredService<AssetCatalog.Infrastructure.Persistence.AssetCatalogDbContext>();
//         // await SeedAssetCatalogAsync(assetDb);
//     }
// }