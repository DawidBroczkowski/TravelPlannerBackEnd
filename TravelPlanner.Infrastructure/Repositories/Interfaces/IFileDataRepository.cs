using TravelPlanner.Shared.DTOs.File;

namespace TravelPlanner.Infrastructure.Repositories.Interfaces
{
    public interface IFileDataRepository
    {
        Task AddFileDataAsync(FileDataDto fileDataDto, CancellationToken cancellationToken);
        Task DeleteFileDataAsync(Guid fileId, CancellationToken cancellationToken);
        Task DeleteFileDataAsync(int id, CancellationToken cancellationToken);
        Task<FileDataDto?> GetFileDataAsync(Guid fileId, CancellationToken cancellationToken);
        Task<FileDataDto?> GetFileDataAsync(int id, CancellationToken cancellationToken);
        Task<List<FileDataDto>> GetFilesDataAsync(int from, int to, CancellationToken cancellationToken);
        Task<List<FileDataDto>> GetFilesDataAsync(List<Guid> fileIds, CancellationToken cancellationToken);
        Task<List<FileDataDto>> GetFilesDataAsync(int from, int to, string contentType, CancellationToken cancellationToken);
        Task AssignFileAsync(AssignFileDto dto, CancellationToken cancellationToken);
    }
}