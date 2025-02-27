using TravelPlanner.Shared.DTOs.Attraction;
using TravelPlanner.Shared.DTOs.Attraction.Location;

namespace TravelPlanner.Application.Services
{
    public interface IAttractionService
    {
        Task<int> CreateAddressAsync(CreateAddressDto createAddressDto, CancellationToken cancellationToken);
        Task<int> CreateAttractionAsync(CreateAttractionDto createAttractionDto, CancellationToken cancellationToken);
        Task<int> CreateCategoryAsync(CreateAttractionCategoryDto createCategoryDto, CancellationToken cancellationToken);
        Task<int> CreateCountryAsync(CreateCountryDto createCountryDto, CancellationToken cancellationToken);
        Task<int> CreateLocalityAsync(CreateLocalityDto createLocalityDto, CancellationToken cancellationToken);
        Task<int> CreateProvinceAsync(CreateProvinceDto createProvinceDto, CancellationToken cancellationToken);
        Task<int> CreateTagAsync(CreateAttractionTagDto createTagDto, CancellationToken cancellationToken);
        Task<GetAddressDto> GetAddressAsync(int id, CancellationToken cancellationToken);
        Task<List<GetAddressDto>> GetAddressesAsync(int from, int to, CancellationToken cancellationToken, int? countryId = null, int? provinceId = null, int? localityId = null);
        Task<GetAttractionDto> GetAttractionAsync(int id, bool permissionOverride, CancellationToken cancellationToken);
        Task<List<GetAttractionDto>> GetAttractionsAsync(int from, int to, bool onlyPublic, CancellationToken cancellationToken, int? countryId = null, int? provinceId = null, int? localityId = null, int? addressId = null);
        Task<List<GetAttractionCategoryDto>> GetCategoriesAsync(int from, int to, CancellationToken cancellationToken);
        Task<GetAttractionCategoryDto> GetCategoryAsync(int id, CancellationToken cancellationToken);
        Task<List<GetCountryDto>> GetCountriesAsync(int from, int to, CancellationToken cancellationToken);
        Task<GetCountryDto> GetCountryAsync(int id, CancellationToken cancellationToken);
        Task<List<GetLocalityDto>> GetLocalitiesAsync(int from, int to, CancellationToken cancellationToken, int? countryId = null, int? provinceId = null);
        Task<GetLocalityDto> GetLocalityAsync(int id, CancellationToken cancellationToken);
        Task<GetProvinceDto> GetProvinceAsync(int id, CancellationToken cancellationToken);
        Task<List<GetProvinceDto>> GetProvincesAsync(int from, int to, CancellationToken cancellationToken, int? countryId = null);
        Task<GetAttractionTagDto> GetTagAsync(int id, CancellationToken cancellationToken);
        Task<List<GetAttractionTagDto>> GetTagsAsync(int from, int to, CancellationToken cancellationToken);
    }
}