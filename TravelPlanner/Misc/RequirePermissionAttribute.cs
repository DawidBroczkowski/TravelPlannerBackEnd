using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.Misc
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class RequirePermissionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly Permission _requiredPermission;

        public RequirePermissionAttribute(Permission requiredPermission)
        {
            _requiredPermission = requiredPermission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user is null || user.Identity!.IsAuthenticated is false)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Parse the string back to the Permission enum
            if (!Enum.TryParse(user.FindFirst("permissions")?.Value, out Permission permissionClaim))
            {
                context.Result = new ForbidResult();
                return;
            }

            // Check if the user has the required permission using bitwise AND comparison
            if ((permissionClaim & _requiredPermission) != _requiredPermission)
            {
                context.Result = new ForbidResult();
            }
        }
    }

}
