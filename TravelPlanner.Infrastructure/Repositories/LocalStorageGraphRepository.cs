using Microsoft.Extensions.Configuration;
using TravelPlanner.Infrastructure.Repositories.Interfaces;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class LocalStorageGraphRepository : IGraphRepository
    {
        private IConfiguration _configuration;

        public LocalStorageGraphRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SaveGraphAsync(string json)
        {
            await File.WriteAllTextAsync(_configuration["Graph:Path"]!, json);
        }

        public async Task<string> LoadGraphAsync()
        {
            return await File.ReadAllTextAsync(_configuration["Graph:Path"]!);
        }
    }
}
