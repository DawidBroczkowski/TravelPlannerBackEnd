using TravelPlanner.Shared.DTOs.File;

namespace TravelPlanner.Application.Services.Interfaces
{
    public interface IFileService
    {
        Task AssignFileAsync(AssignFileDto dto, bool permissionOverride, CancellationToken cancellationToken);
        Task AssignFileAsync(List<AssignFileDto> dto, bool permissionOverride, CancellationToken cancellationToken);
        Task DeleteFileAsync(Guid fileId, bool permissionOverride, CancellationToken cancellationToken);
        Task DeleteFileAsync(List<Guid> fileIds, bool permissionOverride, CancellationToken cancellationToken);
        Task<(FileStream Stream, string ContentType, string FileName)> GetFileAsync(Guid fileId, bool permissionOverride, CancellationToken cancellationToken);
    }
}