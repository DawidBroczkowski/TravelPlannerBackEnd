using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TravelPlanner.Domain.Models;
using TravelPlanner.Infrastructure.Repositories.Interfaces;
using TravelPlanner.Shared.DTOs.Penalty;
using TravelPlanner.Shared.DTOs.User;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class DbUserRepository : BaseDbRepository, IUserRepository
    {
        public DbUserRepository(TravelPlannerContext db, IMapper mapper) : base(db, mapper) { }

        public async Task<GetUserDto?> GetUserAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _db.Users
                .ProjectTo<GetUserDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            return user;
        }

        public async Task<GetUserDto?> GetUserAsync(string name, CancellationToken cancellationToken)
        {
            var user = await _db.Users
                .ProjectTo<GetUserDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(u => u.Name == name, cancellationToken);
            return user;
        }

        public async Task<GetUserDto?> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var user = await _db.Users.
                ProjectTo<GetUserDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, cancellationToken);
            return user;
        }

        public async Task<List<GetUserDto>> GetUsersAsync(int from, int to, CancellationToken cancellationToken)
        {
            var users = await _db.Users
                .ProjectTo<GetUserDto>(_mapper.ConfigurationProvider)
                .Where(u => u.Id >= from && u.Id <= to)
                .ToListAsync(cancellationToken);
            return users;
        }

        public async Task<GetUserDataDto> GetUserDataAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _db.Users.ProjectTo<GetUserDataDto>(_mapper.ConfigurationProvider).FirstAsync(u => u.Id == id, cancellationToken);
            return user;
        }

        public async Task<GetUserDataDto> GetUserDataAsync(string name, CancellationToken cancellationToken)
        {
            var user = await _db.Users.ProjectTo<GetUserDataDto>(_mapper.ConfigurationProvider).FirstAsync(u => u.Name == name, cancellationToken);
            return user;
        }

        public async Task<List<GetUserDataDto>> GetUsersDataAsync(int from, int to, CancellationToken cancellationToken)
        {
            var users = await _db.Users
                .ProjectTo<GetUserDataDto>(_mapper.ConfigurationProvider)
                .Where(u => u.Id >= from && u.Id <= to)
                .ToListAsync(cancellationToken);
            return users;
        }

        public async Task<GetUserDto?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _db.Users.ProjectTo<GetUserDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
            return user;
        }

        public async Task<GetUserCredentialsDto> GetUserCredentialsAsync(int id, CancellationToken cancellationToken)
        {
            var credentials = await _db.Users.ProjectTo<GetUserCredentialsDto>(_mapper.ConfigurationProvider).FirstAsync(u => u.Id == id, cancellationToken);
            return credentials;
        }

        public async Task<GetUserCredentialsDto> GetUserCredentialsByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var credentials = await _db.Users.ProjectTo<GetUserCredentialsDto>(_mapper.ConfigurationProvider).FirstAsync(u => u.Email == email, cancellationToken);
            return credentials;
        }

        public async Task<string?> GetUserRefreshTokenAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.Users
                .Where(u => u.Id == id)
                .Select(u => u.RefreshToken)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> UserExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.Users.AnyAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<bool> UserExistsAsync(string name, CancellationToken cancellationToken)
        {
            return await _db.Users.AnyAsync(u => u.Name == name, cancellationToken);
        }

        public async Task<bool> UserExistsAsync(string name, string email, CancellationToken cancellationToken)
        {
            return await _db.Users.AnyAsync(u => u.Name == name && u.Email == email, cancellationToken);
        }

        public async Task<bool> UserExistsByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _db.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }

        public async Task UpdateRefreshTokenAsync(int id, string refreshToken, CancellationToken cancellationToken)
        {
            var user = await _db.Users.FindAsync(id);
            user!.RefreshToken = refreshToken;
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateTokensAsync(int id, string refreshToken, DateTime refreshTokenExpiry, Guid jti, DateTime jtiExpiry, CancellationToken cancellationToken)
        {
            var user = await _db.Users.FindAsync(id);
            user!.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = refreshTokenExpiry;
            user.Jti = jti;
            user.JtiExpiry = jtiExpiry;
            await _db.SaveChangesAsync(cancellationToken);
        }

        //public async Task AddUserAsync(CreateUserDto dto, string roleName, CancellationToken cancellationToken)
        //{
        //    var role = await _db.Roles.FirstAsync(r => r.Name == roleName, cancellationToken);
        //    if (role is null)
        //    {
        //        role = new Role();
        //    }

        //    User user = new User
        //    {
        //        Name = dto.Name,
        //        Email = dto.Email,
        //        PasswordHash = dto.PasswordHash,
        //        PasswordSalt = dto.PasswordSalt,
        //        Role = role
        //    };

        //    await _db.Users.AddAsync(user, cancellationToken);
        //    await _db.SaveChangesAsync(cancellationToken);
        //}

        public async Task AddPenaltyToUserAsync(CreatePenaltyDto penaltyDto, int issuerId, CancellationToken cancellationToken)
        {
            var penalty = _mapper.Map<Penalty>(penaltyDto);
            penalty.IssuedById = issuerId;
            await _db.Penalties.AddAsync(penalty, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<GetPenaltyDto>> GetUserPenaltiesAsync(int userId, CancellationToken cancellationToken)
        {
            var penalties = await _db.Penalties
                .Where(p => p.UserId == userId)
                .ProjectTo<GetPenaltyDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return penalties;
        }

        public async Task<List<GetPenaltyDto>> GetActiveUserPenalties(int userId, CancellationToken cancellationToken)
        {
            var penalties = await _db.Penalties
                .Where(p => p.UserId == userId && p.EndDate > DateTime.UtcNow && p.StartDate < DateTime.UtcNow)
                .ProjectTo<GetPenaltyDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return penalties;
        }
    }
}
