using TravelPlanner.Application.Services;
using TravelPlanner.Application.Services.Interfaces;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.Application
{
    public class ApplicationServiceFactory
    {
        private IServiceProvider _serviceProvider;
        private readonly Dictionary<EntityType, Type> _serviceMap;

        public ApplicationServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _serviceMap = new Dictionary<EntityType, Type>
            {
                { EntityType.User, typeof(IUserService) },
                { EntityType.UserProfile, typeof(IUserProfileService) },
                { EntityType.Attraction, typeof(IAttractionService) }

                //{ EntityType.Attraction, typeof(AttractionService) },
                //{ EntityType.TravelPlan, typeof(TravelPlanService) }
            };
        }

        public IEntityFileManager GetFileManagerServiceByEntityType(EntityType entityType)
        {
            if (_serviceMap.TryGetValue(entityType, out var serviceType) is false)
            {
                throw new ArgumentException($"No service found for entity type {entityType}");
            }

            return (IEntityFileManager)_serviceProvider.GetService(serviceType)!;
        }
    }
}
