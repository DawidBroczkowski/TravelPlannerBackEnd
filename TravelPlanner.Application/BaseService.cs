using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using TravelPlanner.Application.Services;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.Application
{
    public class BaseService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IConfiguration _configuration;
        protected readonly IMapper _mapper;
        protected readonly IUserContextService _userContext;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly RoleManager<IdentityRole<int>> _roleManager;

        public BaseService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _mapper = _serviceProvider.GetRequiredService<IMapper>();
            _userContext = _serviceProvider.GetRequiredService<IUserContextService>();
            _userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
        }

        protected int _userId => int
            .TryParse(_userContext.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
            out int userId) ? userId : 0;

        protected Permission _userPermissions
        {
            get
            {
                var permissionClaim = _userContext.Claims.FirstOrDefault(c => c.Type == "permissions")?.Value;
                if (string.IsNullOrEmpty(permissionClaim))
                {
                    return Permission.None;
                }

                try
                {
                    return (Permission)Enum.Parse(typeof(Permission), permissionClaim);
                }
                catch (ArgumentException)
                {
                    // Handle the case where the permission claim cannot be parsed
                    return Permission.None;
                }
            }
        }
    }
}
