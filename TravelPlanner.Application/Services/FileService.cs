using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelPlanner.Application.Services.Interfaces;
using TravelPlanner.Infrastructure.Repositories.Interfaces;
using TravelPlanner.Shared;
using TravelPlanner.Shared.DTOs.File;

namespace TravelPlanner.Application.Services
{
    public class FileService : BaseService, IFileService
    {
        private IFileDataRepository _fileDataRepository;
        private IFileRepository _fileRepository;
        public FileService(IServiceProvider serviceProvider, IConfiguration configuration) : base(serviceProvider, configuration)
        {
            _fileDataRepository = serviceProvider.GetRequiredService<IFileDataRepository>();
            _fileRepository = serviceProvider.GetRequiredService<IFileRepository>();
        }

        public async Task AssignFileAsync(AssignFileDto dto, bool permissionOverride, CancellationToken cancellationToken)
        {
            var fileData = await _fileDataRepository.GetFileDataAsync(dto.FileId, cancellationToken);
            if (fileData is null)
            {
                var ex = new KeyNotFoundException("File with this id does not exist");
                ex.Data.Add(nameof(dto.FileId), dto.FileId);
                throw ex;
            }

            if (fileData.UploadedById != _userId && permissionOverride is false)
            {
                var ex = new UnauthorizedAccessException("You cannot assign a file uploaded by another user");
                ex.Data.Add(nameof(dto.FileId), dto.FileId);
                throw ex;
            }

            await _fileDataRepository.AssignFileAsync(dto, cancellationToken);
        }

        public async Task AssignFileAsync(List<AssignFileDto> dto, bool permissionOverride, CancellationToken cancellationToken)
        {
            foreach (var item in dto)
            {
                await AssignFileAsync(item, permissionOverride, cancellationToken);
            }
        }

        public async Task DeleteFileAsync(Guid fileId, bool permissionOverride, CancellationToken cancellationToken)
        {
            var fileData = await _fileDataRepository.GetFileDataAsync(fileId, cancellationToken);
            if (fileData is null)
            {
                var ex = new KeyNotFoundException("File with this id does not exist");
                ex.Data.Add(nameof(fileId), fileId);
                throw ex;
            }

            if (fileData.UploadedById != _userId && permissionOverride is false)
            {
                var ex = new UnauthorizedAccessException("You cannot delete a file uploaded by another user");
                ex.Data.Add(nameof(fileId), fileId);
                throw ex;
            }

            await _fileDataRepository.DeleteFileDataAsync(fileData.Id, cancellationToken);
            _fileRepository.DeleteFile(fileData);
        }

        public async Task DeleteFileAsync(List<Guid> fileIds, bool permissionOverride, CancellationToken cancellationToken)
        {
            foreach (var id in fileIds)
            {
                await DeleteFileAsync(id, permissionOverride, cancellationToken);
            }
        }

        public async Task<(FileStream Stream, string ContentType, string FileName)> GetFileAsync(Guid fileId,
            bool permissionOverride, CancellationToken cancellationToken)
        {
            var fileData = await _fileDataRepository.GetFileDataAsync(fileId, cancellationToken);

            if (fileData is null)
            {
                var ex = new KeyNotFoundException("File with this id does not exist");
                ex.Data.Add(nameof(fileId), fileId);
                throw ex;
            }

            if (fileData.UploadedById != _userId && fileData.IsPublic is false && permissionOverride is false)
            {
                var ex = new UnauthorizedAccessException("You cannot download a file uploaded by another user");
                ex.Data.Add(nameof(fileId), fileId);
                throw ex;
            }

            var stream = _fileRepository.GetFileStream(fileData);

            var contentType = AllowedFileTypes.ImageMimeTypes
                                .Concat(AllowedFileTypes.VideoMimeTypes)
                                .Concat(AllowedFileTypes.DocumentMimeTypes)
                                .FirstOrDefault(mime => fileData.FileName.EndsWith(mime.Split('/').Last(),
                                    StringComparison.OrdinalIgnoreCase))
                                ?? "application/octet-stream";

            var fileName = fileData.FileName;

            return (stream, contentType, fileName);
        }
    }
}
