using Audit.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Json;
using TravelPlanner.Domain.Models;
using TravelPlanner.Infrastructure.Email;
using TravelPlanner.Infrastructure.Email.Interfaces;
using TravelPlanner.Infrastructure.Graphs;
using TravelPlanner.Infrastructure.Repositories;
using TravelPlanner.Infrastructure.Repositories.Interfaces;

namespace TravelPlanner.Infrastructure
{
    public static class DependencyInjection
    {
        public static WebApplication EnsureDatabaseIsCreated(this WebApplication app)
        {
            //using (var scope = app.Services.CreateScope())
            //{
            //    var dbContext = scope.ServiceProvider.GetRequiredService<TravelPlannerContext>();

            //    dbContext.Database.Migrate();

            //    // Check if the database is created and the seeding hasn't been performed yet
            //    if (!dbContext.InitializationLogs.Any(log => log.IsSeeded))
            //    {
            //        app.Services.SeedRoles();

            //        // Mark that seeding is done
            //        dbContext.InitializationLogs.Add(new InitializationLog { IsSeeded = true });
            //        dbContext.SaveChanges();
            //    }
            //}

            return app;
        }

        public static WebApplication ConfigureAudit(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                Audit.Core.Configuration.Setup()
                    .UseEntityFramework(ef => ef
                        .AuditTypeMapper(t => typeof(AuditLog)) // Map audits to the AuditLog entity
                        .AuditEntityAction<AuditLog>((ev, entry, auditEntity) =>
                        {
                            var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
                            var httpContext = httpContextAccessor?.HttpContext;
                            auditEntity.ChangedById = null;
                            if (httpContext != null)
                            {
                                var userId = httpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                                if (int.TryParse(userId, out int parsedUserId))
                                {
                                    auditEntity.ChangedById = parsedUserId;
                                }
                            }

                            auditEntity.Id = default; // Reset the ID to let the database generate it

                            auditEntity.TableName = entry.Table;
                            auditEntity.RecordId = (int)entry.PrimaryKey.FirstOrDefault().Value; // Assumes primary key is int
                            auditEntity.OperationType = entry.Action; // "Insert", "Update", or "Delete"

                            if (entry.Action == "Update")
                            {
                                // For updates, capture both old and new values
                                auditEntity.OldValues = JsonSerializer.Serialize(
                                    entry.Changes.ToDictionary(c => c.ColumnName, c => c.OriginalValue));
                                auditEntity.NewValues = JsonSerializer.Serialize(
                                    entry.Changes.ToDictionary(c => c.ColumnName, c => c.NewValue));
                            }
                            else if (entry.Action == "Insert")
                            {
                                // For inserts, capture only the current values
                                auditEntity.NewValues = JsonSerializer.Serialize(entry.ColumnValues);
                            }
                            else if (entry.Action == "Delete")
                            {
                                // For deletes, capture only the original values
                                auditEntity.OldValues = JsonSerializer.Serialize(entry.ColumnValues);
                            }

                            auditEntity.ChangedAt = DateTime.UtcNow;
                        }));
            }

            return app;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register IHttpContextAccessor
            services.AddHttpContextAccessor();

            // Register DbContext and add interceptors
            services.AddDbContext<TravelPlannerContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"));
                options.EnableSensitiveDataLogging(false);
                // Add the interceptor
                //options.AddInterceptors(new AuditLogInterceptor(services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>()));
            });//, b => b.MigrationsAssembly(typeof(TravelPlannerContext).Assembly.FullName), ServiceLifetime.Transient);

            // Register other services
            services.AddScoped<IUserRepository, DbUserRepository>();
            services.AddScoped<IJwtBlacklistRepository, RedisJwtBlacklistRepository>();
            services.AddScoped<IFileDataRepository, DbFileDataRepository>();
            services.AddScoped<IUserProfileRepository, DbUserProfileRepository>();
            services.AddScoped<IJwtBlacklistRepository, RedisJwtBlacklistRepository>();
            services.AddScoped<IFileRepository, LocalStorageFileRepository>();
            services.AddScoped<IFileDataRepository, DbFileDataRepository>();
            services.AddScoped<IAttractionRepository, DbAttractionRepository>();
            services.AddScoped<IRouteService, GrapHopperRouteService>();
            services.AddScoped<ITrailRepository, DbTrailRepository>();
            services.AddScoped<IGraphRepository, LocalStorageGraphRepository>();

            var postmarkSettings = new PostmarkSettings
            {
                ServerToken = configuration["Postmark:ServerToken"]!,
                FromEmail = configuration["Postmark:FromEmail"]!
            };
            services.AddSingleton(Options.Create(postmarkSettings));
            services.AddTransient<IEmailService, PostmarkEmailService>();

            return services;
        }

        public static void SeedRoles(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
                IdentityRole<int> role = new();
                Permission rolePermissions = Permission.None;

                role.Name = "User";
                roleManager.CreateAsync(role).Wait();
                role = roleManager.FindByNameAsync("User").Result!;
                rolePermissions = Permission.CreateTravelPlan | Permission.SaveTravelPlan | Permission.ViewTravelPlan
                        | Permission.DeleteTravelPlan | Permission.MakeTravelPlanPublic | Permission.MakeTravelPlanPrivate
                        | Permission.ViewPublicTravelPlans | Permission.CommentOnPublicTravelPlan | Permission.EditOwnCommentOnPublicPlan
                        | Permission.DeleteOwnCommentOnPublicPlan | Permission.CommentOnAttraction | Permission.EditOwnComment
                        | Permission.DeleteOwnComment;
                roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("permissions", rolePermissions.ToString())).Wait();


                role = new IdentityRole<int>();
                role.Name = "Contributor";
                roleManager.CreateAsync(role).Wait();
                role = roleManager.FindByNameAsync("Contributor").Result!;
                rolePermissions = Permission.CreateTravelPlan | Permission.SaveTravelPlan | Permission.ViewTravelPlan
                        | Permission.DeleteTravelPlan | Permission.MakeTravelPlanPublic | Permission.MakeTravelPlanPrivate
                        | Permission.ViewPublicTravelPlans | Permission.CommentOnPublicTravelPlan | Permission.EditOwnCommentOnPublicPlan
                        | Permission.DeleteOwnCommentOnPublicPlan | Permission.CommentOnAttraction | Permission.EditOwnComment
                        | Permission.DeleteOwnComment | Permission.CreateAttraction | Permission.EditAttraction | Permission.DeleteAttraction;

                roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("permissions", rolePermissions.ToString())).Wait();


                role = new IdentityRole<int>();
                role.Name = "Moderator";
                roleManager.CreateAsync(role).Wait();
                role = roleManager.FindByNameAsync("Moderator").Result!;
                rolePermissions = Permission.CreateTravelPlan | Permission.SaveTravelPlan | Permission.ViewTravelPlan
                        | Permission.DeleteTravelPlan | Permission.MakeTravelPlanPublic | Permission.MakeTravelPlanPrivate
                        | Permission.ViewPublicTravelPlans | Permission.CommentOnPublicTravelPlan | Permission.EditOwnCommentOnPublicPlan
                        | Permission.DeleteOwnCommentOnPublicPlan | Permission.CommentOnAttraction | Permission.EditOwnComment
                        | Permission.DeleteOwnComment | Permission.CreateAttraction | Permission.EditAttraction | Permission.DeleteAttraction
                        | Permission.ModerateComments | Permission.ModerateAttractions | Permission.ModerateUsers
                        | Permission.ViewFlaggedContent | Permission.ModerateCommentsOnPublicPlans | Permission.ModeratePublicTravelPlans
                        | Permission.AccessPrivateContent;
                roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("permissions", rolePermissions.ToString())).Wait();


                role = new IdentityRole<int>();
                role.Name = "Administrator";
                roleManager.CreateAsync(role).Wait();
                role = roleManager.FindByNameAsync("Administrator").Result!;
                rolePermissions = Permission.CreateTravelPlan | Permission.SaveTravelPlan | Permission.ViewTravelPlan
                        | Permission.DeleteTravelPlan | Permission.MakeTravelPlanPublic | Permission.MakeTravelPlanPrivate
                        | Permission.ViewPublicTravelPlans | Permission.CommentOnPublicTravelPlan | Permission.EditOwnCommentOnPublicPlan
                        | Permission.DeleteOwnCommentOnPublicPlan | Permission.CommentOnAttraction | Permission.EditOwnComment
                        | Permission.DeleteOwnComment | Permission.CreateAttraction | Permission.EditAttraction | Permission.DeleteAttraction
                        | Permission.ModerateComments | Permission.ModerateAttractions | Permission.ModerateUsers
                        | Permission.ViewFlaggedContent | Permission.ModerateCommentsOnPublicPlans | Permission.ModeratePublicTravelPlans
                        | Permission.AccessPrivateContent
                        | Permission.ManageUsers | Permission.ManageRoles | Permission.AccessAnalytics | Permission.ManageSiteSettings
                        | Permission.ViewAdminReports | Permission.AccessAdminPanel | Permission.FeaturePublicTravelPlan
                        | Permission.FeatureAttraction;
                roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("permissions", rolePermissions.ToString())).Wait();


                role = new IdentityRole<int>();
                role.Name = "SuperAdmin";
                roleManager.CreateAsync(role).Wait();
                role = roleManager.FindByNameAsync("SuperAdmin").Result!;
                rolePermissions = Permission.All;
                roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("permissions", rolePermissions.ToString())).Wait();
            }
        }
    }
}
