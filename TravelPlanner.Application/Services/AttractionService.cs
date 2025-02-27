using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelPlanner.Infrastructure.Repositories.Interfaces;
using TravelPlanner.Shared.DTOs.Attraction;
using TravelPlanner.Shared.DTOs.Attraction.Location;

namespace TravelPlanner.Application.Services
{
    public class AttractionService : BaseService, IAttractionService
    {
        private readonly IAttractionRepository _attractionRepository;

        public AttractionService(IServiceProvider serviceProvider, IConfiguration configuration) : base(serviceProvider, configuration)
        {
            _attractionRepository = serviceProvider.GetRequiredService<IAttractionRepository>();
        }

        public async Task<GetAttractionDto> GetAttractionAsync(int id, bool permissionOverride, CancellationToken cancellationToken)
        {
            var attraction = await _attractionRepository.GetAttractionAsync(id, cancellationToken);
            if (attraction is null)
            {
                var ex = new KeyNotFoundException("Attraction not found");
                ex.Data.Add(nameof(id), id);
                throw ex;
            }

            if (attraction.IsPublic is false && permissionOverride is false)
            {
                var ex = new UnauthorizedAccessException("You cannot view this attraction");
                ex.Data.Add(nameof(attraction.Id), "This attraction is not public.");
                throw ex;
            }

            return attraction;
        }

        public async Task<List<GetAttractionDto>> GetAttractionsAsync(int from, int to, bool onlyPublic, CancellationToken cancellationToken,
            int? countryId = null, int? provinceId = null, int? localityId = null, int? addressId = null)
        {
            return await _attractionRepository.GetAttractionsAsync(from, to, onlyPublic, cancellationToken, countryId, provinceId, localityId, addressId);
        }

        public async Task<int> CreateAttractionAsync(CreateAttractionDto createAttractionDto, CancellationToken cancellationToken)
        {
            return await _attractionRepository.CreateAttractionAsync(createAttractionDto, cancellationToken);
        }

        // Locality methods
        public async Task<GetLocalityDto> GetLocalityAsync(int id, CancellationToken cancellationToken)
        {
            var locality = await _attractionRepository.GetLocalityAsync(id, cancellationToken);
            if (locality is null)
            {
                var ex = new KeyNotFoundException("Locality not found");
                ex.Data.Add(nameof(id), id);
                throw ex;
            }
            return locality;
        }

        public async Task<List<GetLocalityDto>> GetLocalitiesAsync(int from, int to, CancellationToken cancellationToken, int? countryId = null, int? provinceId = null)
        {
            return await _attractionRepository.GetLocalitiesAsync(from, to, cancellationToken, countryId, provinceId);
        }

        public async Task<int> CreateLocalityAsync(CreateLocalityDto createLocalityDto, CancellationToken cancellationToken)
        {
            return await _attractionRepository.CreateLocalityAsync(createLocalityDto, cancellationToken);
        }

        // Province methods
        public async Task<GetProvinceDto> GetProvinceAsync(int id, CancellationToken cancellationToken)
        {
            var province = await _attractionRepository.GetProvinceAsync(id, cancellationToken);
            if (province is null)
            {
                var ex = new KeyNotFoundException("Province not found");
                ex.Data.Add(nameof(id), id);
                throw ex;
            }
            return province;
        }

        public async Task<List<GetProvinceDto>> GetProvincesAsync(int from, int to, CancellationToken cancellationToken, int? countryId = null)
        {
            return await _attractionRepository.GetProvincesAsync(from, to, cancellationToken, countryId);
        }

        public async Task<int> CreateProvinceAsync(CreateProvinceDto createProvinceDto, CancellationToken cancellationToken)
        {
            return await _attractionRepository.CreateProvinceAsync(createProvinceDto, cancellationToken);
        }

        // Country methods
        public async Task<GetCountryDto> GetCountryAsync(int id, CancellationToken cancellationToken)
        {
            var country = await _attractionRepository.GetCountryAsync(id, cancellationToken);
            if (country is null)
            {
                var ex = new KeyNotFoundException("Country not found");
                ex.Data.Add(nameof(id), id);
                throw ex;
            }
            return country;
        }

        public async Task<List<GetCountryDto>> GetCountriesAsync(int from, int to, CancellationToken cancellationToken)
        {
            return await _attractionRepository.GetCountriesAsync(from, to, cancellationToken);
        }

        public async Task<int> CreateCountryAsync(CreateCountryDto createCountryDto, CancellationToken cancellationToken)
        {
            return await _attractionRepository.CreateCountryAsync(createCountryDto, cancellationToken);
        }

        // Address methods
        public async Task<GetAddressDto> GetAddressAsync(int id, CancellationToken cancellationToken)
        {
            var address = await _attractionRepository.GetAddressAsync(id, cancellationToken);
            if (address is null)
            {
                var ex = new KeyNotFoundException("Address not found");
                ex.Data.Add(nameof(id), id);
                throw ex;
            }
            return address;
        }

        public async Task<List<GetAddressDto>> GetAddressesAsync(int from, int to, CancellationToken cancellationToken
            , int? countryId = null, int? provinceId = null, int? localityId = null)
        {
            return await _attractionRepository.GetAddressesAsync(from, to, cancellationToken, countryId, provinceId, localityId);
        }

        public async Task<int> CreateAddressAsync(CreateAddressDto createAddressDto, CancellationToken cancellationToken)
        {
            return await _attractionRepository.CreateAddressAsync(createAddressDto, cancellationToken);
        }

        public async Task<GetAttractionCategoryDto> GetCategoryAsync(int id, CancellationToken cancellationToken)
        {
            var category = await _attractionRepository.GetCategoryAsync(id, cancellationToken);
            if (category is null)
            {
                var ex = new KeyNotFoundException("Category not found");
                ex.Data.Add(nameof(id), id);
                throw ex;
            }
            return category;
        }

        public async Task<List<GetAttractionCategoryDto>> GetCategoriesAsync(int from, int to, CancellationToken cancellationToken)
        {
            return await _attractionRepository.GetCategoriesAsync(from, to, cancellationToken);
        }

        public async Task<int> CreateCategoryAsync(CreateAttractionCategoryDto createCategoryDto, CancellationToken cancellationToken)
        {
            return await _attractionRepository.CreateCategoryAsync(createCategoryDto, cancellationToken);
        }

        public async Task<GetAttractionTagDto> GetTagAsync(int id, CancellationToken cancellationToken)
        {
            var tag = await _attractionRepository.GetTagAsync(id, cancellationToken);
            if (tag is null)
            {
                var ex = new KeyNotFoundException("Tag not found");
                ex.Data.Add(nameof(id), id);
                throw ex;
            }
            return tag;
        }

        public async Task<List<GetAttractionTagDto>> GetTagsAsync(int from, int to, CancellationToken cancellationToken)
        {
            return await _attractionRepository.GetTagsAsync(from, to, cancellationToken);
        }

        public async Task<int> CreateTagAsync(CreateAttractionTagDto createTagDto, CancellationToken cancellationToken)
        {
            return await _attractionRepository.CreateTagAsync(createTagDto, cancellationToken);
        }
    }
}
