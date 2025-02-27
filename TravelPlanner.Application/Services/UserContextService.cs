using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace TravelPlanner.Application.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<Claim> Claims => _httpContextAccessor.HttpContext!.User.Claims.ToList();
    }
}
