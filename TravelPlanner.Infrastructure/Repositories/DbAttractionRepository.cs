using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TravelPlanner.Domain.Models.Attractions;
using TravelPlanner.Domain.Models.Attractions.Location;
using TravelPlanner.Domain.Models.Attractions.Time;
using TravelPlanner.Infrastructure.Repositories.Interfaces;
using TravelPlanner.Shared.DTOs.Attraction;
using TravelPlanner.Shared.DTOs.Attraction.Location;
using TravelPlanner.Shared.DTOs.Attraction.Time;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class DbAttractionRepository : BaseDbEntityFileRepository, IAttractionRepository
    {
        public DbAttractionRepository(TravelPlannerContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        // Projection doesn't work for some reason and I don't have the time to fix it
        public async Task<GetAttractionDto?> GetAttractionAsync(int id, CancellationToken cancellationToken)
        {
            var attraction = await _db.Attractions
                .Where(a => a.Id == id)
                .Include(a => a.Category)
                .Include(a => a.Tags)
                .Include(a => a.Schedules)
                .ThenInclude(s => s.TimeSlots)
                .Include(a => a.SeasonalAvailabilities)
                .Include(a => a.Regions)
                .Include(a => a.SpecialDays)
                .Include(a => a.Address)
                .ThenInclude(a => a.Locality)
                .ThenInclude(l => l.Province)
                .ThenInclude(p => p.Country)
                .FirstOrDefaultAsync(cancellationToken);

            if (attraction == null)
            {
                return null;
            }

            var result = _mapper.Map<GetAttractionDto>(attraction);

            var fileIds = await _db.FilesData
                .Where(fd => fd.EntityType == Domain.Models.EntityType.Attraction && fd.EntityId == id)
                .Select(fd => fd.FileId)
                .ToListAsync(cancellationToken);

            result.FileIds = fileIds;
            return result;
        }

        // Probably can be done better
        public async Task<List<GetAttractionDto>> GetAttractionsAsync(int from, int to, bool onlyPublic, CancellationToken cancellationToken,
            int? countryId = null, int? provinceId = null, int? localityId = null, int? addressId = null)
        {
            var result = await _db.Attractions
                                .Include(a => a.Category)
                                .Include(a => a.Tags)
                                .Include(a => a.Schedules)
                                .ThenInclude(s => s.TimeSlots)
                                .Include(a => a.SeasonalAvailabilities)
                                .Include(a => a.Regions)
                                .Include(a => a.SpecialDays)
                                .Include(a => a.Address)
                                .ThenInclude(a => a.Locality)
                                .ThenInclude(l => l.Province)
                                .ThenInclude(p => p.Country)
                                .Where(a => (countryId == null || a.Address!.Locality!.Province!.Country!.Id == countryId) &&
                                            (provinceId == null || a.Address!.Locality!.Province!.Id == provinceId) &&
                                            (localityId == null || a.Address!.Locality!.Id == localityId) &&
                                            (addressId == null || a.Address!.Id == addressId) &&
                                            (onlyPublic ? a.IsPublic : true))
                                .Skip(from - 1)
                                .Take(to - from + 1)
                                .ToListAsync(cancellationToken);

            List<GetAttractionDto> dtos = new();
            foreach (var attraction in result)
            {
                var dto = _mapper.Map<GetAttractionDto>(attraction);

                var fileIds = await _db.FilesData
                    .Where(fd => fd.EntityType == Domain.Models.EntityType.Attraction && fd.EntityId == attraction.Id)
                    .Select(fd => fd.FileId)
                    .ToListAsync(cancellationToken);

                dto.FileIds = fileIds;
                dtos.Add(dto);
            }

            return dtos;
        }

        public async Task<int> CreateAttractionAsync(CreateAttractionDto createAttractionDto, CancellationToken cancellationToken)
        {
            var attraction = _mapper.Map<Attraction>(createAttractionDto);

            // Retrieve the locality, province, and country
            var locality = await _db.Localities
                .Include(l => l.Province)
                .ThenInclude(p => p.Country)
                .FirstAsync(l => l.Id == createAttractionDto.LocalityId);

            // Create the address
            var address = new Address
            {
                Street = createAttractionDto.Street,
                Latitude = createAttractionDto.Latitude,
                Longitude = createAttractionDto.Longitude,
                Locality = locality
            };

            attraction.Address = address;

            // Add schedules
            foreach (var scheduleDto in createAttractionDto.Schedule)
            {
                var schedule = _mapper.Map<Schedule>(scheduleDto);
                attraction.Schedules.Add(schedule);
            }

            // Add seasonal availabilities
            foreach (var seasonalAvailabilityDto in createAttractionDto.SeasonalAvailability)
            {
                var seasonalAvailability = _mapper.Map<SeasonalAvailability>(seasonalAvailabilityDto);
                attraction.SeasonalAvailabilities.Add(seasonalAvailability);
            }

            // Add Category
            var category = await _db.AttractionCategories.FindAsync(createAttractionDto.CategoryId);
            attraction.Category = category;

            // Add Tags
            foreach (var tagId in createAttractionDto.TagIds)
            {
                var tag = await _db.AttractionTags.FindAsync(tagId);
                attraction.Tags.Add(tag!);
            }

            _db.Attractions.Add(attraction);
            await _db.SaveChangesAsync(cancellationToken);
            return attraction.Id;
        }

        // Locality methods
        public async Task<GetLocalityDto?> GetLocalityAsync(int id, CancellationToken cancellationToken)
        {
            var locality = await _db.Localities.ProjectTo<GetLocalityDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
            return locality;
        }

        public async Task<List<GetLocalityDto>> GetLocalitiesAsync(int from, int to, CancellationToken cancellationToken, int? countryId = null, int? provinceId = null)
        {
            var localities = await _db.Localities
                .Where(l => (countryId == null || l.Province.Country.Id == countryId) &&
                            (provinceId == null || l.Province.Id == provinceId))
                .Skip(from - 1)
                .Take(to - from + 1)
                .ProjectTo<GetLocalityDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return localities;
        }

        public async Task<int> CreateLocalityAsync(CreateLocalityDto createLocalityDto, CancellationToken cancellationToken)
        {
            var locality = _mapper.Map<Locality>(createLocalityDto);
            await _db.Localities.AddAsync(locality, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return locality.Id;
        }

        // Province methods
        public async Task<GetProvinceDto?> GetProvinceAsync(int id, CancellationToken cancellationToken)
        {
            var province = await _db.Provinces.ProjectTo<GetProvinceDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            return province;
        }

        public async Task<List<GetProvinceDto>> GetProvincesAsync(int from, int to, CancellationToken cancellationToken, int? countryId = null)
        {
            var provinces = await _db.Provinces
                .Where(p => countryId == null || p.Country.Id == countryId)
                .Skip(from - 1)
                .Take(to - from + 1)
                .ProjectTo<GetProvinceDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return provinces;
        }

        public async Task<int> CreateProvinceAsync(CreateProvinceDto createProvinceDto, CancellationToken cancellationToken)
        {
            var province = _mapper.Map<Province>(createProvinceDto);
            await _db.Provinces.AddAsync(province, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return province.Id;
        }

        // Country methods
        public async Task<GetCountryDto?> GetCountryAsync(int id, CancellationToken cancellationToken)
        {
            var country = await _db.Countries.ProjectTo<GetCountryDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            return country;
        }

        public async Task<List<GetCountryDto>> GetCountriesAsync(int from, int to, CancellationToken cancellationToken)
        {
            var countries = await _db.Countries
                .Skip(from - 1)
                .Take(to - from + 1)
                .ProjectTo<GetCountryDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return countries;
        }

        public async Task<int> CreateCountryAsync(CreateCountryDto createCountryDto, CancellationToken cancellationToken)
        {
            var country = _mapper.Map<Country>(createCountryDto);
            await _db.Countries.AddAsync(country, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return country.Id;
        }

        // Address methods
        public async Task<GetAddressDto?> GetAddressAsync(int id, CancellationToken cancellationToken)
        {
            var address = await _db.Addresses.ProjectTo<GetAddressDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
            return address;
        }

        public async Task<List<GetAddressDto>> GetAddressesAsync(int from, int to, CancellationToken cancellationToken, int? countryId = null, int? provinceId = null, int? localityId = null)
        {
            var addresses = await _db.Addresses
                .Where(a => (countryId == null || a.Locality.Province.Country.Id == countryId) &&
                            (provinceId == null || a.Locality.Province.Id == provinceId) &&
                            (localityId == null || a.Locality.Id == localityId))
                .Skip(from - 1)
                .Take(to - from + 1)
                .ProjectTo<GetAddressDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return addresses;
        }

        public async Task<int> CreateAddressAsync(CreateAddressDto createAddressDto, CancellationToken cancellationToken)
        {
            var address = _mapper.Map<Address>(createAddressDto);
            await _db.Addresses.AddAsync(address, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return address.Id;
        }

        // Schedule methods
        public async Task<GetScheduleDto?> GetScheduleAsync(int id, CancellationToken cancellationToken)
        {
            var schedule = await _db.Schedule.ProjectTo<GetScheduleDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
            return schedule;
        }

        public async Task<List<GetScheduleDto>> GetSchedulesAsync(int from, int to, CancellationToken cancellationToken)
        {
            var schedules = await _db.Schedule
                .Skip(from - 1)
                .Take(to - from + 1)
                .ProjectTo<GetScheduleDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return schedules;
        }

        public async Task<int> CreateScheduleAsync(CreateScheduleDto createScheduleDto, CancellationToken cancellationToken)
        {
            var schedule = _mapper.Map<Schedule>(createScheduleDto);
            await _db.Schedule.AddAsync(schedule, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return schedule.Id;
        }

        // ScheduleTimeSlot methods
        public async Task<GetScheduleTimeSlotDto?> GetScheduleTimeSlotAsync(int id, CancellationToken cancellationToken)
        {
            var scheduleTimeSlot = await _db.ScheduleTimeSlot.ProjectTo<GetScheduleTimeSlotDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(sts => sts.Id == id, cancellationToken);
            return scheduleTimeSlot;
        }

        public async Task<List<GetScheduleTimeSlotDto>> GetScheduleTimeSlotsAsync(int from, int to, CancellationToken cancellationToken)
        {
            var scheduleTimeSlots = await _db.ScheduleTimeSlot
                .Skip(from - 1)
                .Take(to - from + 1)
                .ProjectTo<GetScheduleTimeSlotDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return scheduleTimeSlots;
        }

        public async Task<int> CreateScheduleTimeSlotAsync(CreateScheduleTimeSlotDto createScheduleTimeSlotDto, CancellationToken cancellationToken)
        {
            var scheduleTimeSlot = _mapper.Map<ScheduleTimeSlot>(createScheduleTimeSlotDto);
            await _db.ScheduleTimeSlot.AddAsync(scheduleTimeSlot, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return scheduleTimeSlot.Id;
        }

        // SeasonalAvailability methods
        public async Task<GetSeasonalAvailabilityDto?> GetSeasonalAvailabilityAsync(int id, CancellationToken cancellationToken)
        {
            var seasonalAvailability = await _db.SeasonalAvailability.ProjectTo<GetSeasonalAvailabilityDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(sa => sa.Id == id, cancellationToken);
            return seasonalAvailability;
        }

        public async Task<List<GetSeasonalAvailabilityDto>> GetSeasonalAvailabilitiesAsync(int from, int to, CancellationToken cancellationToken)
        {
            var seasonalAvailabilities = await _db.SeasonalAvailability
                .Skip(from - 1)
                .Take(to - from + 1)
                .ProjectTo<GetSeasonalAvailabilityDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return seasonalAvailabilities;
        }

        public async Task<int> CreateSeasonalAvailabilityAsync(CreateSeasonalAvailabilityDto createSeasonalAvailabilityDto, CancellationToken cancellationToken)
        {
            var seasonalAvailability = _mapper.Map<SeasonalAvailability>(createSeasonalAvailabilityDto);
            await _db.SeasonalAvailability.AddAsync(seasonalAvailability, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return seasonalAvailability.Id;
        }

        // SpecialDay methods
        public async Task<GetSpecialDayDto?> GetSpecialDayAsync(int id, CancellationToken cancellationToken)
        {
            var specialDay = await _db.SpecialDay.ProjectTo<GetSpecialDayDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(sd => sd.Id == id, cancellationToken);
            return specialDay;
        }

        public async Task<List<GetSpecialDayDto>> GetSpecialDaysAsync(int from, int to, CancellationToken cancellationToken)
        {
            var specialDays = await _db.SpecialDay
                .Skip(from - 1)
                .Take(to - from + 1)
                .ProjectTo<GetSpecialDayDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return specialDays;
        }

        public async Task<int> CreateSpecialDayAsync(GetSpecialDayDto createSpecialDayDto, CancellationToken cancellationToken)
        {
            var specialDay = _mapper.Map<SpecialDay>(createSpecialDayDto);
            await _db.SpecialDay.AddAsync(specialDay, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return specialDay.Id;
        }

        public async Task<GetAttractionCategoryDto> GetCategoryAsync(int id, CancellationToken cancellationToken)
        {
            var category = await _db.AttractionCategories.ProjectTo<GetAttractionCategoryDto>(_mapper.ConfigurationProvider).FirstAsync(c => c.Id == id, cancellationToken);
            return category;
        }

        public async Task<List<GetAttractionCategoryDto>> GetCategoriesAsync(int from, int to, CancellationToken cancellationToken)
        {
            var categories = await _db.AttractionCategories
                .Skip(from - 1)
                .Take(to - from + 1)
                .ProjectTo<GetAttractionCategoryDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return categories;
        }

        public async Task<int> CreateCategoryAsync(CreateAttractionCategoryDto createCategoryDto, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<AttractionCategory>(createCategoryDto);
            await _db.AttractionCategories.AddAsync(category, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return category.Id;
        }

        public async Task AssignCategoryAsync(int id, int categoryId, CancellationToken cancellationToken)
        {
            var attraction = await _db.Attractions.FindAsync(id);
            var category = await _db.AttractionCategories.FindAsync(categoryId);
            attraction!.Category = category;
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteCategoryAsync(int id, CancellationToken cancellationToken)
        {
            var attraction = await _db.Attractions.FindAsync(id);
            attraction!.Category = null;
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<GetAttractionTagDto> GetTagAsync(int id, CancellationToken cancellationToken)
        {
            var tag = await _db.AttractionTags.ProjectTo<GetAttractionTagDto>(_mapper.ConfigurationProvider).FirstAsync(t => t.Id == id, cancellationToken);
            return tag;
        }

        public async Task<List<GetAttractionTagDto>> GetTagsAsync(int from, int to, CancellationToken cancellationToken)
        {
            var tags = await _db.AttractionTags
                .Skip(from - 1)
                .Take(to - from + 1)
                .ProjectTo<GetAttractionTagDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return tags;
        }

        public async Task<int> CreateTagAsync(CreateAttractionTagDto createTagDto, CancellationToken cancellationToken)
        {
            var tag = _mapper.Map<AttractionTag>(createTagDto);
            await _db.AttractionTags.AddAsync(tag, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return tag.Id;
        }

        public async Task AssignTagAsync(int id, int tagId, CancellationToken cancellationToken)
        {
            var attraction = await _db.Attractions.FindAsync(id);
            var tag = await _db.AttractionTags.FindAsync(tagId);
            attraction!.Tags.Add(tag!);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteTagAsync(int id, int tagId, CancellationToken cancellationToken)
        {
            var attraction = await _db.Attractions.FindAsync(id);
            var tag = await _db.AttractionTags.FindAsync(tagId);
            attraction!.Tags.Remove(tag!);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
