using AutoMapper;
using TravelPlanner.Infrastructure.Repositories.Interfaces;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class BaseDbEntityFileRepository : BaseDbRepository, IEntityFileRepository
    {
        public BaseDbEntityFileRepository(TravelPlannerContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public virtual async Task AssignFileAsync(int id, int fileDataId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual async Task DeleteFileAsync(int id, int fileDataId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
