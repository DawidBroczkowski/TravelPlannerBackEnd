using TravelPlanner.Shared.DTOs.Trail;

namespace TravelPlanner.Application.Services.Interfaces
{
    public interface ITrailService
    {
        Task<int> CreateTrailAsync(CreateTrailDto trailDto, CancellationToken cancellationToken);
        Task<GetTrailDto?> GetTrailAsync(int id, bool permissionOverride, CancellationToken cancellationToken);
        Task<List<GetTrailDto>> GetTrailsAsync(int from, int to, bool onlyPublic, CancellationToken cancellationToken, int? countryId = null, int? provinceId = null, int? localityId = null);
        Task<List<GetTrailDto>> GetUserTrailsAsync(int userId, int from, int to, bool permissionOverride, CancellationToken cancellationToken);
        Task<List<GetAttractionInTrailDto>> GetAttractionsInTrailAsync(int trailId, bool onlyPublic, bool permissionOverride, CancellationToken cancellationToken);
        Task<GetAttractionInTrailDto?> GetAttractionInTrailAsync(int attractionInTrailId, bool permissionOverride, CancellationToken cancellationToken);
        Task<int> AddAttractionToTrailAsync(CreateAttractionInTrailDto dto, CancellationToken cancellationToken);
        Task RemoveAttractionFromTrailAsync(CreateAttractionInTrailDto dto, CancellationToken cancellationToken);
        Task DeleteTrailAsync(int id, CancellationToken cancellationToken);
    }
}