namespace TravelPlanner.Infrastructure.Repositories.Interfaces
{
    public interface IGraphRepository
    {
        Task<string> LoadGraphAsync();
        Task SaveGraphAsync(string json);
    }
}