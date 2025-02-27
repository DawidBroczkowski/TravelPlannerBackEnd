using System.Security.Claims;

namespace TravelPlanner.Application.Services
{
    public interface IUserContextService
    {
        List<Claim> Claims { get; }
    }
}