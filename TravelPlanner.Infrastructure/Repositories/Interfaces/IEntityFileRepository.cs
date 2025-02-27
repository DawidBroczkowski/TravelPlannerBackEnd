namespace TravelPlanner.Infrastructure.Repositories.Interfaces
{
    public interface IEntityFileRepository
    {
        Task AssignFileAsync(int id, int fileDataId, CancellationToken cancellationToken);
        Task DeleteFileAsync(int id, int fileDataId, CancellationToken cancellationToken);
    }
}
