using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelPlanner.Application.Services.Interfaces;
using TravelPlanner.Infrastructure.Repositories;
using TravelPlanner.Shared.DTOs.Trail;

namespace TravelPlanner.Application.Services
{
    public class TrailService : BaseService, ITrailService
    {
        private readonly ITrailRepository _trailRepository;

        public TrailService(IServiceProvider serviceProvider, IConfiguration configuration) : base(serviceProvider, configuration)
        {
            _trailRepository = serviceProvider.GetRequiredService<ITrailRepository>();
        }

        public async Task<int> CreateTrailAsync(CreateTrailDto trailDto, CancellationToken cancellationToken)
        {
            int id = await _trailRepository.CreateTrailAsync(trailDto, _userId, cancellationToken);
            return id;
        }

        public async Task<GetTrailDto?> GetTrailAsync(int id, bool permissionOverride, CancellationToken cancellationToken)
        {
            var trail = await _trailRepository.GetTrailAsync(id, cancellationToken);
            if (trail is null)
            {
                var ex = new KeyNotFoundException($"Trail not found");
                ex.Data.Add(nameof(trail.Id), id);
                throw ex;
            }

            if (trail.CreatedById != _userId && trail.IsPublic is false && permissionOverride is false)
            {
                var ex = new UnauthorizedAccessException($"You cannot view this trail.");
                ex.Data.Add(nameof(trail.Id), id);
                throw ex;
            }

            return trail;
        }

        public async Task<List<GetTrailDto>> GetTrailsAsync(int from, int to, bool onlyPublic, CancellationToken cancellationToken, int? countryId = null, int? provinceId = null, int? localityId = null)
        {
            return await _trailRepository.GetTrailsAsync(from, to, onlyPublic, cancellationToken, countryId, provinceId, localityId);
        }

        public async Task<List<GetTrailDto>> GetUserTrailsAsync(int userId, int from, int to, bool permissionOverride, CancellationToken cancellationToken)
        {
            bool onlyPublic = true;

            if (userId == _userId || permissionOverride is true)
            {
                onlyPublic = false;
            }

            return await _trailRepository.GetUserTrailsAsync(userId, from, to, onlyPublic, cancellationToken);
        }

        public async Task<List<GetAttractionInTrailDto>> GetAttractionsInTrailAsync(int trailId, bool onlyPublic, bool permissionOverride, CancellationToken cancellationToken)
        {
            var trail = await _trailRepository.GetTrailAsync(trailId, cancellationToken);
            if (trail is null)
            {
                var ex = new KeyNotFoundException($"Trail not found");
                ex.Data.Add(nameof(trail.Id), trailId);
                throw ex;
            }

            if (trail.CreatedById != _userId && permissionOverride is false)
            {
                var ex = new UnauthorizedAccessException($"You cannot view this trail.");
                ex.Data.Add(nameof(trail.Id), trailId);
                throw ex;
            }

            return await _trailRepository.GetAttractionsInTrailAsync(trailId, onlyPublic, cancellationToken);
        }

        public async Task<GetAttractionInTrailDto?> GetAttractionInTrailAsync(int attractionInTrailId, bool permissionOverride, CancellationToken cancellationToken)
        {
            var attractionInTrail = await _trailRepository.GetAttractionInTrailAsync(attractionInTrailId, cancellationToken);
            if (attractionInTrail is null)
            {
                var ex = new KeyNotFoundException($"Attraction in trail not found");
                ex.Data.Add(nameof(attractionInTrail.Id), attractionInTrailId);
                throw ex;
            }

            var trail = await _trailRepository.GetTrailAsync(attractionInTrail.TrailId, cancellationToken);
            if (trail!.CreatedById != _userId && trail.IsPublic is false && permissionOverride is false)
            {
                var ex = new UnauthorizedAccessException($"You cannot view this attraction in trail.");
                ex.Data.Add(nameof(attractionInTrail.Id), attractionInTrailId);
                throw ex;
            }

            return await _trailRepository.GetAttractionInTrailAsync(attractionInTrailId, cancellationToken);
        }

        public async Task<int> AddAttractionToTrailAsync(CreateAttractionInTrailDto dto, CancellationToken cancellationToken)
        {
            var trail = await _trailRepository.GetTrailAsync(dto.TrailId, cancellationToken);
            if (trail is null)
            {
                var ex = new KeyNotFoundException($"Trail not found");
                ex.Data.Add(nameof(trail.Id), dto.TrailId);
                throw ex;
            }

            if (trail.CreatedById != _userId)
            {
                var ex = new UnauthorizedAccessException($"You cannot add an attraction to this trail.");
                ex.Data.Add(nameof(trail.Id), dto.TrailId);
                throw ex;
            }

            return await _trailRepository.AddAttractionToTrailAsync(dto, cancellationToken);
        }

        public async Task RemoveAttractionFromTrailAsync(CreateAttractionInTrailDto dto, CancellationToken cancellationToken)
        {
            var trail = await _trailRepository.GetTrailAsync(dto.TrailId, cancellationToken);
            if (trail is null)
            {
                var ex = new KeyNotFoundException($"Trail not found");
                ex.Data.Add(nameof(trail.Id), dto.TrailId);
                throw ex;
            }

            if (trail.CreatedById != _userId)
            {
                var ex = new UnauthorizedAccessException($"You cannot remove an attraction from this trail.");
                ex.Data.Add(nameof(trail.Id), dto.TrailId);
                throw ex;
            }

            await _trailRepository.RemoveAttractionFromTrailAsync(dto, cancellationToken);
        }

        public async Task DeleteTrailAsync(int id, CancellationToken cancellationToken)
        {
            var trail = await _trailRepository.GetTrailAsync(id, cancellationToken);
            if (trail is null)
            {
                var ex = new KeyNotFoundException($"Trail not found");
                ex.Data.Add(nameof(trail.Id), id);
                throw ex;
            }

            if (trail.CreatedById != _userId)
            {
                var ex = new UnauthorizedAccessException($"You cannot delete this trail.");
                ex.Data.Add(nameof(trail.Id), id);
                throw ex;
            }

            await _trailRepository.DeleteTrailAsync(id, cancellationToken);
        }
    }
}
