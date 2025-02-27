using AutoMapper;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class BaseDbRepository
    {
        protected TravelPlannerContext _db;
        protected IMapper _mapper;

        protected BaseDbRepository(TravelPlannerContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
    }
}
