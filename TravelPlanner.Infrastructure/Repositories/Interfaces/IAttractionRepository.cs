using TravelPlanner.Shared.DTOs.Attraction;
using TravelPlanner.Shared.DTOs.Attraction.Location;
using TravelPlanner.Shared.DTOs.Attraction.Time;

namespace TravelPlanner.Infrastructure.Repositories.Interfaces
{
    public interface IAttractionRepository
    {
        Task AssignCategoryAsync(int id, int categoryId, CancellationToken cancellationToken);
        Task AssignFileAsync(int id, int fileDataId, CancellationToken cancellationToken);
        Task AssignTagAsync(int id, int tagId, CancellationToken cancellationToken);
        Task<int> CreateAddressAsync(CreateAddressDto createAddressDto, CancellationToken cancellationToken);
        Task<int> CreateAttractionAsync(CreateAttractionDto createAttractionDto, CancellationToken cancellationToken);
        Task<int> CreateCategoryAsync(CreateAttractionCategoryDto createCategoryDto, CancellationToken cancellationToken);
        Task<int> CreateCountryAsync(CreateCountryDto createCountryDto, CancellationToken cancellationToken);
        Task<int> CreateLocalityAsync(CreateLocalityDto createLocalityDto, CancellationToken cancellationToken);
        Task<int> CreateProvinceAsync(CreateProvinceDto createProvinceDto, CancellationToken cancellationToken);
        Task<int> CreateScheduleAsync(CreateScheduleDto createScheduleDto, CancellationToken cancellationToken);
        Task<int> CreateScheduleTimeSlotAsync(CreateScheduleTimeSlotDto createScheduleTimeSlotDto, CancellationToken cancellationToken);
        Task<int> CreateSeasonalAvailabilityAsync(CreateSeasonalAvailabilityDto createSeasonalAvailabilityDto, CancellationToken cancellationToken);
        Task<int> CreateSpecialDayAsync(GetSpecialDayDto createSpecialDayDto, CancellationToken cancellationToken);
        Task<int> CreateTagAsync(CreateAttractionTagDto createTagDto, CancellationToken cancellationToken);
        Task DeleteCategoryAsync(int id, CancellationToken cancellationToken);
        Task DeleteFileAsync(int id, int fileDataId, CancellationToken cancellationToken);
        Task DeleteTagAsync(int id, int tagId, CancellationToken cancellationToken);
        Task<GetAddressDto?> GetAddressAsync(int id, CancellationToken cancellationToken);
        Task<List<GetAddressDto>> GetAddressesAsync(int from, int to, CancellationToken cancellationToken, int? countryId = null, int? provinceId = null, int? localityId = null);
        Task<GetAttractionDto?> GetAttractionAsync(int id, CancellationToken cancellationToken);
        Task<List<GetAttractionDto>> GetAttractionsAsync(int from, int to, bool onlyPublic, CancellationToken cancellationToken, int? countryId = null, int? provinceId = null, int? localityId = null, int? addressId = null);
        Task<List<GetAttractionCategoryDto>> GetCategoriesAsync(int from, int to, CancellationToken cancellationToken);
        Task<GetAttractionCategoryDto> GetCategoryAsync(int id, CancellationToken cancellationToken);
        Task<List<GetCountryDto>> GetCountriesAsync(int from, int to, CancellationToken cancellationToken);
        Task<GetCountryDto?> GetCountryAsync(int id, CancellationToken cancellationToken);
        Task<List<GetLocalityDto>> GetLocalitiesAsync(int from, int to, CancellationToken cancellationToken, int? countryId = null, int? provinceId = null);
        Task<GetLocalityDto?> GetLocalityAsync(int id, CancellationToken cancellationToken);
        Task<GetProvinceDto?> GetProvinceAsync(int id, CancellationToken cancellationToken);
        Task<List<GetProvinceDto>> GetProvincesAsync(int from, int to, CancellationToken cancellationToken, int? countryId = null);
        Task<GetScheduleDto?> GetScheduleAsync(int id, CancellationToken cancellationToken);
        Task<List<GetScheduleDto>> GetSchedulesAsync(int from, int to, CancellationToken cancellationToken);
        Task<GetScheduleTimeSlotDto?> GetScheduleTimeSlotAsync(int id, CancellationToken cancellationToken);
        Task<List<GetScheduleTimeSlotDto>> GetScheduleTimeSlotsAsync(int from, int to, CancellationToken cancellationToken);
        Task<List<GetSeasonalAvailabilityDto>> GetSeasonalAvailabilitiesAsync(int from, int to, CancellationToken cancellationToken);
        Task<GetSeasonalAvailabilityDto?> GetSeasonalAvailabilityAsync(int id, CancellationToken cancellationToken);
        Task<GetSpecialDayDto?> GetSpecialDayAsync(int id, CancellationToken cancellationToken);
        Task<List<GetSpecialDayDto>> GetSpecialDaysAsync(int from, int to, CancellationToken cancellationToken);
        Task<GetAttractionTagDto> GetTagAsync(int id, CancellationToken cancellationToken);
        Task<List<GetAttractionTagDto>> GetTagsAsync(int from, int to, CancellationToken cancellationToken);
    }
}