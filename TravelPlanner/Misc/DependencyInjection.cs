using TravelPlanner.Services;
using TravelPlanner.Services.Interfaces;

namespace TravelPlanner.Misc
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddScoped<FileIdsResolver>();
            return services;
        }
    }
}
