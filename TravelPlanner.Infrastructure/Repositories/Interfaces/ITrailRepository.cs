using TravelPlanner.Shared.DTOs.Trail;

namespace TravelPlanner.Infrastructure.Repositories
{
    public interface ITrailRepository
    {
        Task<int> AddAttractionToTrailAsync(CreateAttractionInTrailDto dto, CancellationToken cancellationToken);
        Task<int> CreateTrailAsync(CreateTrailDto trailDto, int createdById, CancellationToken cancellationToken);
        Task DeleteTrailAsync(int id, CancellationToken cancellationToken);
        Task<GetAttractionInTrailDto?> GetAttractionInTrailAsync(int attractionInTrailId, CancellationToken cancellationToken);
        Task<List<GetAttractionInTrailDto>> GetAttractionsInTrailAsync(int trailId, bool onlyPublic, CancellationToken cancellationToken);
        Task<GetTrailDto?> GetTrailAsync(int id, CancellationToken cancellationToken);
        Task<List<GetTrailDto>> GetTrailsAsync(int from, int to, bool onlyPublic, CancellationToken cancellationToken, int? countryId = null, int? provinceId = null, int? localityId = null);
        Task<List<GetTrailDto>> GetUserTrailsAsync(int userId, int from, int to, bool onlyPublic, CancellationToken cancellationToken = default);
        Task RemoveAttractionFromTrailAsync(CreateAttractionInTrailDto dto, CancellationToken cancellationToken);
    }
}