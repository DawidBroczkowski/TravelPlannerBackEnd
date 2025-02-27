namespace TravelPlanner
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using TravelPlanner.Infrastructure.Repositories.Interfaces;

    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IJwtBlacklistRepository jwtBlacklistRepository)
        {
            var endpoint = context.GetEndpoint();
            var isEndpointAuthorized = endpoint?.Metadata?.GetMetadata<IAuthorizeData>() != null;

            if (isEndpointAuthorized is true && context.User.Identity?.IsAuthenticated is true)
            {
                // Attempt to get the token from the Authorization header
                var authHeader = context.Request.Headers["Authorization"].ToString();

                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    // Extract the token after "Bearer "
                    var token = authHeader.Substring("Bearer ".Length).Trim();

                    if (!string.IsNullOrEmpty(token))
                    {
                        var handler = new JwtSecurityTokenHandler();
                        var jwtToken = handler.ReadJwtToken(token);

                        // Extract the JTI claim
                        var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                        // Check if the JTI is in the Redis blacklist
                        if (!string.IsNullOrEmpty(jti))
                        {
                            bool isBlacklisted = await jwtBlacklistRepository.IsTokenBlacklistedAsync(jti);
                            if (isBlacklisted)
                            {
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                await context.Response.WriteAsync("Token is blacklisted.");
                                return;
                            }
                        }

                        // Merge claims from JWT token with existing claims
                        var currentClaims = context.User.Claims.ToList();
                        var newClaims = jwtToken.Claims
                            .Where(jwtClaim => !currentClaims.Any(c => c.Type == jwtClaim.Type))
                            .ToList();

                        // Combine current claims with new claims from JWT
                        var allClaims = currentClaims.Concat(newClaims).ToList();

                        // Create a new ClaimsIdentity with merged claims
                        var mergedClaimsIdentity = new ClaimsIdentity(allClaims, "Bearer");
                        context.User = new ClaimsPrincipal(mergedClaimsIdentity);
                    }
                    else
                    {
                        // Token not found or empty; respond with 401 Unauthorized
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Unauthorized request. Token is missing.");
                        return;
                    }
                }
                else
                {
                    // No Authorization header or no Bearer token found
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized request. Bearer token is missing.");
                    return;
                }
            }

            await _next(context);
        }
    }
}