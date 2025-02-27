using TravelPlanner.Shared.DTOs.File;

namespace TravelPlanner.Application.Services.Interfaces
{
    public interface IEntityFileManager
    {
        Task AssignFileAsync(AssignFileDto dto, bool permissionOverride, CancellationToken cancellationToken);
        Task DeleteFileAsync(AssignFileDto id, bool permissionOverride, CancellationToken cancellationToken);
    }
}
