using TravelPlanner.Shared.DTOs.File;

namespace TravelPlanner.Infrastructure.Repositories.Interfaces
{
    public interface IFileRepository
    {
        void DeleteFile(FileDataDto fileData);
        FileStream GetFileStream(FileDataDto fileData);
    }
}