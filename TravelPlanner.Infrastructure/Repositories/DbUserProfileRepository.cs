using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TravelPlanner.Domain.Models;
using TravelPlanner.Infrastructure.Repositories.Interfaces;
using TravelPlanner.Shared.DTOs.UserProfile;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class DbUserProfileRepository : BaseDbEntityFileRepository, IUserProfileRepository
    {
        public DbUserProfileRepository(TravelPlannerContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {

        }

        public async Task<GetUserProfileDto?> GetUserProfileAsync(int userId, CancellationToken cancellationToken)
        {
            var userProfile = await _db.UserProfiles
                .Include(x => x.User)
                .ProjectTo<GetUserProfileDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
            return userProfile;
        }

        public async Task<List<GetUserProfileDto>> GetUserProfilesAsync(int from, int to, bool onlyPublic, CancellationToken cancellationToken)
        {
            var userProfiles = await _db.UserProfiles
                .Where(x => onlyPublic ? x.IsPublic : true)
                .Include(x => x.User)
                .Skip(from - 1)
                .Take(to - from + 1)
                .ProjectTo<GetUserProfileDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
            return userProfiles;
        }

        public async Task CreateUserProfileAsync(int userId, CancellationToken cancellationToken)
        {
            var userProfile = new UserProfile
            {
                UserId = userId
            };
            await _db.UserProfiles.AddAsync(userProfile, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateUserProfileAsync(UpdateUserProfileDto userProfile, CancellationToken cancellationToken)
        {
            var entity = await _db.UserProfiles.FindAsync(userProfile.Id, cancellationToken);
            entity!.Description = userProfile.Description ?? entity.Description;
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task AssignProfileImageAsync(int userId, Guid fileId, CancellationToken cancellationToken)
        {
            var userProfile = await _db.UserProfiles.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
            userProfile!.ProfileImageId = fileId;
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
