using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TravelPlanner.Domain.Models;
using TravelPlanner.Infrastructure.Repositories.Interfaces;
using TravelPlanner.Shared.DTOs.File;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class DbFileDataRepository : BaseDbRepository, IFileDataRepository
    {
        public DbFileDataRepository(TravelPlannerContext db, IMapper mapper) : base(db, mapper)
        {
        }

        public async Task<FileDataDto?> GetFileDataAsync(int id, CancellationToken cancellationToken)
        {
            var fileData = await _db.FilesData
                .ProjectTo<FileDataDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return fileData;
        }

        public async Task<FileDataDto?> GetFileDataAsync(Guid fileId, CancellationToken cancellationToken)
        {
            var fileData = await _db.FilesData
                .ProjectTo<FileDataDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(fd => fd.FileId == fileId, cancellationToken);
            return fileData;
        }

        public async Task<List<FileDataDto>> GetFilesDataAsync(int from, int to, CancellationToken cancellationToken)
        {
            var filesData = await _db.FilesData.ProjectTo<FileDataDto>(_mapper.ConfigurationProvider)
                .Where(fd => fd.Id >= from && fd.Id <= to)
                .ToListAsync(cancellationToken);
            return filesData;
        }

        public async Task<List<FileDataDto>> GetFilesDataAsync(int from, int to, string contentType, CancellationToken cancellationToken)
        {
            var filesData = await _db.FilesData
                .ProjectTo<FileDataDto>(_mapper.ConfigurationProvider)
                .Where(fd => fd.ContentType == contentType)
                .Where(fd => fd.Id >= from && fd.Id <= to)
                .ToListAsync(cancellationToken);
            return filesData;
        }

        public async Task<List<FileDataDto>> GetFilesDataAsync(List<Guid> fileIds, CancellationToken cancellationToken)
        {
            var filesData = await _db.FilesData
                .ProjectTo<FileDataDto>(_mapper.ConfigurationProvider)
                .Where(x => fileIds.Contains(x.FileId))
                .ToListAsync(cancellationToken);
            return filesData;
        }

        public async Task AddFileDataAsync(FileDataDto fileDataDto, CancellationToken cancellationToken)
        {
            var fileData = _mapper.Map<FileData>(fileDataDto);
            _db.FilesData.Add(fileData);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteFileDataAsync(int id, CancellationToken cancellationToken)
        {
            var fileData = await _db.FilesData.FindAsync(id);
            _db.FilesData.Remove(fileData!);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteFileDataAsync(Guid fileId, CancellationToken cancellationToken)
        {
            var fileData = await _db.FilesData.FirstOrDefaultAsync(fd => fd.FileId == fileId, cancellationToken);
            _db.FilesData.Remove(fileData!);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task AssignFileAsync(AssignFileDto dto, CancellationToken cancellationToken)
        {
            var fileData = await _db.FilesData.FirstAsync(fd => fd.FileId == dto.FileId, cancellationToken);
            fileData!.EntityId = dto.EntityId;
            fileData!.EntityType = dto.EntityType;
            fileData!.IsUploaded = true;
            fileData!.IsPublic = dto.IsPublic;
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
