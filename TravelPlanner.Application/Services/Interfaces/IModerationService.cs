using TravelPlanner.Shared.DTOs.Penalty;

namespace TravelPlanner.Application.Services.Interfaces
{
    public interface IModerationService
    {
        Task ApplyPenaltyToUserAsync(CreatePenaltyDto dto, CancellationToken cancellationToken);
    }
}