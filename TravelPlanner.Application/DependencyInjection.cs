using Microsoft.Extensions.DependencyInjection;
using TravelPlanner.Application.Services;
using TravelPlanner.Application.Services.Graphs;
using TravelPlanner.Application.Services.Interfaces;

namespace TravelPlanner.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IModerationService, ModerationService>();
            services.AddTransient<IUserContextService, UserContextService>();
            services.AddTransient<IUserProfileService, UserProfileService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IAttractionService, AttractionService>();
            services.AddTransient<ITrailService, TrailService>();
            services.AddTransient<IAttractionSearchService, AttractionSearchService>();
            services.AddTransient<IGraphService, GraphService>();
            services.AddTransient<ISearchEngine, SearchEngine>();
            services.AddSingleton<TravelGraphAccessor>();

            return services;
        }
    }
}
