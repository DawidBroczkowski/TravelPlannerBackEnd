using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TravelPlanner.Domain.Models;
using TravelPlanner.Domain.Models.Trails;
using TravelPlanner.Shared.DTOs.Trail;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class DbTrailRepository : BaseDbRepository, ITrailRepository
    {
        public DbTrailRepository(TravelPlannerContext db, IMapper mapper) : base(db, mapper)
        {
        }

        public async Task<int> CreateTrailAsync(CreateTrailDto trailDto, int createdById, CancellationToken cancellationToken)
        {
            var trail = _mapper.Map<Trail>(trailDto);
            var user = await _db.Users.FindAsync(createdById, cancellationToken);
            trail.CreatedBy = user;
            await _db.Trails.AddAsync(trail, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return trail.Id;
        }

        public async Task<GetTrailDto?> GetTrailAsync(int id, CancellationToken cancellationToken)
        {
            var trail = await _db.Trails
                //.ProjectTo<GetTrailDto>(_mapper.ConfigurationProvider)
                .Include(x => x.CreatedBy)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            var result = _mapper.Map<GetTrailDto>(trail);
            return result;
        }

        public async Task<List<GetTrailDto>> GetTrailsAsync(int from, int to, bool onlyPublic, CancellationToken cancellationToken, int? countryId = null, int? provinceId = null, int? localityId = null)
        {
            var trails = await _db.Trails
                .Include(x => x.Attractions)!
                .ThenInclude(x => x.Attraction)
                .ThenInclude(x => x!.Address)
                .ThenInclude(x => x!.Locality)
                .ThenInclude(x => x!.Province)
                .ThenInclude(x => x!.Country)
                .Where(x => (countryId == null || x.Attractions!.First()!.Attraction!.Address!.Locality!.Province!.CountryId == countryId) &&
                            (provinceId == null || x.Attractions!.First()!.Attraction!.Address!.Locality!.ProvinceId == provinceId) &&
                            (localityId == null || x.Attractions!.First()!.Attraction!.Address!.Locality!.Id == localityId) &&
                            (onlyPublic ? x.IsPublic : true))
                .Skip(from - 1)
                .Take(to - from + 1)
                .ProjectTo<GetTrailDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return trails;
        }

        public async Task<List<GetTrailDto>> GetUserTrailsAsync(int userId, int from, int to, bool onlyPublic, CancellationToken cancellationToken = default)
        {
            var trails = await _db.Trails
                .Include(x => x.Attractions)!
                .ThenInclude(x => x.Attraction)
                .ThenInclude(x => x!.Address)
                .ThenInclude(x => x!.Locality)
                .ThenInclude(x => x!.Province)
                .ThenInclude(x => x!.Country)
                .Include(x => x.CreatedBy)
                .Where(x => x.CreatedBy!.Id == userId &&
                    (onlyPublic ? x.IsPublic : true))
                .Skip(from - 1)
                .Take(to - from + 1)
                .ProjectTo<GetTrailDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return trails;
        }

        public async Task<GetAttractionInTrailDto?> GetAttractionInTrailAsync(int attractionInTrailId, CancellationToken cancellationToken)
        {
            var attraction = await _db.AttractionInTrails
                .Include(x => x.Trail)
                .Include(x => x.Attraction)
                .ThenInclude(x => x!.Address)
                .ThenInclude(x => x!.Locality)
                .ThenInclude(x => x!.Province)
                .ThenInclude(x => x!.Country)
                .Include(x => x.Attraction)
                .ThenInclude(x => x!.Category)
                .Include(x => x.Attraction)
                .ThenInclude(x => x!.Tags)
                .Include(x => x.Attraction)
                .ThenInclude(x => x!.Regions)
                .Include(x => x.Attraction)
                .ThenInclude(x => x!.Schedules)
                .ThenInclude(x => x!.TimeSlots)
                .Include(x => x.Attraction)
                .ThenInclude(x => x!.SeasonalAvailabilities)
                .Include(x => x.Attraction)
                .ThenInclude(x => x!.SpecialDays)
                .FirstOrDefaultAsync(x => x.Id == attractionInTrailId);

            var result = _mapper.Map<GetAttractionInTrailDto>(attraction);

            result!.Attraction!.FileIds = await _db.FilesData.Where(x => x.EntityType == EntityType.Attraction && x.EntityId == result.Attraction.Id).Select(x => x.FileId).ToListAsync();

            return result;
        }

        public async Task<List<GetAttractionInTrailDto>> GetAttractionsInTrailAsync(int trailId, bool onlyPublic, CancellationToken cancellationToken)
        {
            var attraction = await _db.AttractionInTrails
                .Include(x => x.Trail)
                .Include(x => x.Attraction)
                .ThenInclude(x => x!.Address)
                .ThenInclude(x => x!.Locality)
                .ThenInclude(x => x!.Province)
                .ThenInclude(x => x!.Country)
                .Include(x => x.Attraction)
                .ThenInclude(x => x!.Category)
                .Include(x => x.Attraction)
                .ThenInclude(x => x!.Tags)
                .Include(x => x.Attraction)
                .ThenInclude(x => x!.Regions)
                .Include(x => x.Attraction)
                .ThenInclude(x => x!.Schedules)
                .ThenInclude(x => x!.TimeSlots)
                .Include(x => x.Attraction)
                .ThenInclude(x => x!.SeasonalAvailabilities)
                .Include(x => x.Attraction)
                .ThenInclude(x => x!.SpecialDays)
                .Where(x => x.Trail!.Id == trailId) // only public?
                .OrderBy(x => x.Order)
                //.ProjectTo<GetAttractionInTrailDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var result = _mapper.Map<List<GetAttractionInTrailDto>>(attraction);

            foreach (var item in result)
            {
                item.Attraction!.FileIds = await _db.FilesData.Where(x => x.EntityType == EntityType.Attraction && x.EntityId == item.Attraction.Id).Select(x => x.FileId).ToListAsync();
            }

            return result;
        }

        public async Task<int> AddAttractionToTrailAsync(CreateAttractionInTrailDto dto, CancellationToken cancellationToken)
        {
            var trail = await _db.Trails
                .Include(x => x.Attractions)
                .FirstOrDefaultAsync(x => x.Id == dto.TrailId, cancellationToken);
            var attraction = await _db.Attractions.FindAsync(dto.AttractionId, cancellationToken);
            var attractionInTrail = new AttractionInTrail
            {
                Trail = trail!,
                Attraction = attraction!,
                TransportationMode = dto.TransportationMode,
                TravelTime = dto.TravelTime,
                TravelDistance = dto.TravelDistance,
                Order = dto.Order
            };
            trail!.Attractions!.Add(attractionInTrail);
            await _db.SaveChangesAsync(cancellationToken);
            return attractionInTrail.Id;
        }

        public async Task RemoveAttractionFromTrailAsync(CreateAttractionInTrailDto dto, CancellationToken cancellationToken)
        {
            var trail = await _db.Trails
                .Include(x => x.Attractions)
                .FirstOrDefaultAsync(x => x.Id == dto.TrailId, cancellationToken);
            var attraction = await _db.AttractionInTrails.FindAsync(dto.AttractionId, cancellationToken);
            trail!.Attractions!.Remove(attraction!);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteTrailAsync(int id, CancellationToken cancellationToken)
        {
            var trail = await _db.Trails.FindAsync(id, cancellationToken);
            _db.Trails.Remove(trail!);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
