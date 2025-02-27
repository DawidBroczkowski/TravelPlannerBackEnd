using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using TravelPlanner.Application;
using TravelPlanner.Domain.Models;
using TravelPlanner.Services.Interfaces;

namespace TravelPlanner.Services
{
    public class AuthService : BaseService, IAuthService
    {
        public AuthService(IServiceProvider serviceProvider, IConfiguration configuration) : base(serviceProvider, configuration)
        {
        }

        // Create a JWT Access Token
        public async Task<(string, Guid, DateTime, long)> CreateAccessTokenAsync(ApplicationUser user,
            CancellationToken cancellationToken)
        {
            Guid jti = Guid.NewGuid();

            var userDto = await user.AsGetUserDto(_serviceProvider, cancellationToken);

            if (userDto.IsBanned is true)
            {
                throw new AccessViolationException("User is banned");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, jti.ToString()),
                new Claim("muted", userDto.IsMuted.ToString())
            };

            Permission permissions = await user.GetUserPermissionsAsync(_serviceProvider, cancellationToken);
            claims.Add(new Claim("permissions", ((long)permissions).ToString()));

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            DateTime expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:Expires"]!));
            claims.Add(new Claim(JwtRegisteredClaimNames.Exp, expires.ToString()));

            if (userDto.IsMuted && userDto.MuteEndDate < expires)
            {
                expires = userDto.MuteEndDate.Value;
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), jti, expires, (long)permissions);
        }

        public string CreateRefreshToken(out DateTime expires)
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Refresh:Expires"]!));
            return Convert.ToBase64String(randomNumber);
        }
    }
}
