using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelPlanner.Application.Services.Interfaces;
using TravelPlanner.Infrastructure.Repositories.Interfaces;
using TravelPlanner.Shared.DTOs.File;

namespace TravelPlanner.Application
{
    public class BaseEntityFileManagerService : BaseService, IEntityFileManager
    {
        protected readonly IFileDataRepository _fileDataRepository;
        protected readonly IFileRepository _fileRepository;
        public BaseEntityFileManagerService(IServiceProvider serviceProvider, IConfiguration configuration) : base(serviceProvider, configuration)
        {
            _fileDataRepository = serviceProvider.GetRequiredService<IFileDataRepository>();
            _fileRepository = serviceProvider.GetRequiredService<IFileRepository>();
        }

        public virtual Task AssignFileAsync(AssignFileDto dto, bool permissionOverride, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task DeleteFileAsync(AssignFileDto dto, bool permissionOverride, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
