using TravelPlanner.Infrastructure.Repositories.Interfaces;
using TravelPlanner.Shared.DTOs.File;
using TravelPlanner.Shared.Extensions;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class LocalStorageFileRepository : IFileRepository
    {
        private string _uploadPath = "E:\\repos\\TravelPlanner\\TravelPlanner\\Uploads"; // TODO: move to config

        public FileStream GetFileStream(FileDataDto fileData)
        {
            var filePath = $"{_uploadPath}\\{fileData.GetPath()}";

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            var fileStream = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 4096,
                useAsync: true
            );

            return fileStream;
        }

        public void DeleteFile(FileDataDto fileData)
        {
            var filePath = $"{_uploadPath}\\{fileData.GetPath()}";

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            File.Delete(filePath);
        }
    }
}
