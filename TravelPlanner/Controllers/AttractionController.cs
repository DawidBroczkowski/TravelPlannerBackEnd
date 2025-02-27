using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Application.Services;
using TravelPlanner.Domain.Models;
using TravelPlanner.Misc;
using TravelPlanner.Shared.DTOs.Attraction;
using TravelPlanner.Shared.DTOs.Attraction.Location;

namespace TravelPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttractionController : ControllerBase
    {
        private readonly IAttractionService _attractionService;

        public AttractionController(IAttractionService attractionService)
        {
            _attractionService = attractionService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetAttractionDto>> GetAttractionAsync(int id, CancellationToken cancellationToken)
        {
            Enum.TryParse(User.Claims.FirstOrDefault(c => c.Type == "permissions")?.Value, out Permission permissions);
            bool hasPrivateContentAccess = (permissions & Permission.AccessPrivateContent) == Permission.AccessPrivateContent;

            var attraction = await _attractionService.GetAttractionAsync(id, hasPrivateContentAccess, cancellationToken);
            return Ok(attraction);
        }

        [HttpGet]
        public async Task<ActionResult<List<GetAttractionDto>>> GetAttractionsAsync(
            [FromQuery] int from,
            [FromQuery] int to,
            [FromQuery] bool onlyPublic,
            CancellationToken cancellationToken,
            [FromQuery] int? countryId = null,
            [FromQuery] int? provinceId = null,
            [FromQuery] int? localityId = null,
            [FromQuery] int? addressId = null)
        {
            var attractions = await _attractionService.GetAttractionsAsync(from, to, onlyPublic, cancellationToken, countryId, provinceId, localityId, addressId);
            return Ok(attractions);
        }

        [Authorize]
        [RequirePermission(Permission.CreateAttraction)]
        [HttpPost]
        public async Task<ActionResult<int>> CreateAttractionAsync(CreateAttractionDto createAttractionDto,
            CancellationToken cancellationToken)
        {
            int id = await _attractionService.CreateAttractionAsync(createAttractionDto, cancellationToken);
            return Ok(id);
        }

        // Locality methods
        [HttpGet("locality/{id}")]
        public async Task<ActionResult<GetLocalityDto>> GetLocalityAsync(int id, CancellationToken cancellationToken)
        {
            var locality = await _attractionService.GetLocalityAsync(id, cancellationToken);
            return Ok(locality);
        }

        [HttpGet("localities")]
        public async Task<ActionResult<List<GetLocalityDto>>> GetLocalitiesAsync(
            [FromQuery] int from,
            [FromQuery] int to,
            CancellationToken cancellationToken,
            [FromQuery] int? countryId = null,
            [FromQuery] int? provinceId = null)
        {
            var localities = await _attractionService.GetLocalitiesAsync(from, to, cancellationToken, countryId, provinceId);
            return Ok(localities);
        }

        [Authorize]
        [RequirePermission(Permission.CreateAttraction)]
        [HttpPost("locality")]
        public async Task<ActionResult<int>> CreateLocalityAsync(CreateLocalityDto createLocalityDto, CancellationToken cancellationToken)
        {
            int id = await _attractionService.CreateLocalityAsync(createLocalityDto, cancellationToken);
            return Ok(id);
        }

        // Province methods
        [HttpGet("province/{id}")]
        public async Task<ActionResult<GetProvinceDto>> GetProvinceAsync(int id, CancellationToken cancellationToken)
        {
            var province = await _attractionService.GetProvinceAsync(id, cancellationToken);
            return Ok(province);
        }

        [HttpGet("provinces")]
        public async Task<ActionResult<List<GetProvinceDto>>> GetProvincesAsync(
            [FromQuery] int from,
            [FromQuery] int to,
            CancellationToken cancellationToken,
            [FromQuery] int? countryId = null)
        {
            var provinces = await _attractionService.GetProvincesAsync(from, to, cancellationToken, countryId);
            return Ok(provinces);
        }

        [Authorize]
        [RequirePermission(Permission.CreateAttraction)]
        [HttpPost("province")]
        public async Task<ActionResult<int>> CreateProvinceAsync(CreateProvinceDto createProvinceDto, CancellationToken cancellationToken)
        {
            int id = await _attractionService.CreateProvinceAsync(createProvinceDto, cancellationToken);
            return Ok(id);
        }

        // Country methods
        [HttpGet("country/{id}")]
        public async Task<ActionResult<GetCountryDto>> GetCountryAsync(int id, CancellationToken cancellationToken)
        {
            var country = await _attractionService.GetCountryAsync(id, cancellationToken);
            return Ok(country);
        }

        [HttpGet("countries")]
        public async Task<ActionResult<List<GetCountryDto>>> GetCountriesAsync(
            [FromQuery] int from,
            [FromQuery] int to,
            CancellationToken cancellationToken)
        {
            var countries = await _attractionService.GetCountriesAsync(from, to, cancellationToken);
            return Ok(countries);
        }

        [Authorize]
        [RequirePermission(Permission.CreateAttraction)]
        [HttpPost("country")]
        public async Task<ActionResult<int>> CreateCountryAsync(CreateCountryDto createCountryDto, CancellationToken cancellationToken)
        {
            int id = await _attractionService.CreateCountryAsync(createCountryDto, cancellationToken);
            return Ok(id);
        }

        // Address methods
        [HttpGet("address/{id}")]
        public async Task<ActionResult<GetAddressDto>> GetAddressAsync(int id, CancellationToken cancellationToken)
        {
            var address = await _attractionService.GetAddressAsync(id, cancellationToken);
            return Ok(address);
        }

        [HttpGet("addresses")]
        public async Task<ActionResult<List<GetAddressDto>>> GetAddressesAsync(
            [FromQuery] int from,
            [FromQuery] int to,
            CancellationToken cancellationToken,
            [FromQuery] int? countryId = null,
            [FromQuery] int? provinceId = null,
            [FromQuery] int? localityId = null)
        {
            var addresses = await _attractionService.GetAddressesAsync(from, to, cancellationToken, countryId, provinceId, localityId);
            return Ok(addresses);
        }

        [Authorize]
        [RequirePermission(Permission.CreateAttraction)]
        [HttpPost("address")]
        public async Task<ActionResult<int>> CreateAddressAsync(CreateAddressDto createAddressDto, CancellationToken cancellationToken)
        {
            int id = await _attractionService.CreateAddressAsync(createAddressDto, cancellationToken);
            return Ok(id);
        }

        // Category methods
        [HttpGet("category/{id}")]
        public async Task<ActionResult<GetAttractionCategoryDto>> GetCategoryAsync(int id, CancellationToken cancellationToken)
        {
            var category = await _attractionService.GetCategoryAsync(id, cancellationToken);
            return Ok(category);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<List<GetAttractionCategoryDto>>> GetCategoriesAsync(
            [FromQuery] int from,
            [FromQuery] int to,
            CancellationToken cancellationToken)
        {
            var categories = await _attractionService.GetCategoriesAsync(from, to, cancellationToken);
            return Ok(categories);
        }

        [Authorize]
        [RequirePermission(Permission.CreateAttraction)]
        [HttpPost("category")]
        public async Task<ActionResult<int>> CreateCategoryAsync(CreateAttractionCategoryDto createCategoryDto, CancellationToken cancellationToken)
        {
            int id = await _attractionService.CreateCategoryAsync(createCategoryDto, cancellationToken);
            return Ok(id);
        }

        // Tag methods
        [HttpGet("tag/{id}")]
        public async Task<ActionResult<GetAttractionTagDto>> GetTagAsync(int id, CancellationToken cancellationToken)
        {
            var tag = await _attractionService.GetTagAsync(id, cancellationToken);
            return Ok(tag);
        }

        [HttpGet("tags")]
        public async Task<ActionResult<List<GetAttractionTagDto>>> GetTagsAsync(
            [FromQuery] int from,
            [FromQuery] int to,
            CancellationToken cancellationToken)
        {
            var tags = await _attractionService.GetTagsAsync(from, to, cancellationToken);
            return Ok(tags);
        }

        [Authorize]
        [RequirePermission(Permission.CreateAttraction)]
        [HttpPost("tag")]
        public async Task<ActionResult<int>> CreateTagAsync(CreateAttractionTagDto createTagDto, CancellationToken cancellationToken)
        {
            int id = await _attractionService.CreateTagAsync(createTagDto, cancellationToken);
            return Ok(id);
        }
    }
}
