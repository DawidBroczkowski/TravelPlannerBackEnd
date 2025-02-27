using AutoMapper;
using TravelPlanner.Domain.Models;
using TravelPlanner.Domain.Models.Attractions;
using TravelPlanner.Domain.Models.Attractions.Location;
using TravelPlanner.Domain.Models.Attractions.Time;
using TravelPlanner.Domain.Models.Trails;
using TravelPlanner.Infrastructure;
using TravelPlanner.Shared.DTOs.Attraction;
using TravelPlanner.Shared.DTOs.Attraction.Location;
using TravelPlanner.Shared.DTOs.Attraction.Time;
using TravelPlanner.Shared.DTOs.Auth;
using TravelPlanner.Shared.DTOs.File;
using TravelPlanner.Shared.DTOs.Penalty;
using TravelPlanner.Shared.DTOs.Trail;
using TravelPlanner.Shared.DTOs.User;
using TravelPlanner.Shared.DTOs.UserProfile;

namespace TravelPlanner.Misc
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, GetUserDto>()
               .ForMember(dest => dest.IsBanned, opt => opt.MapFrom(src =>
                    src.Penalties.Any(p => !p.IsDeleted &&
                       (p.IsPermanent || p.StartDate <= DateTime.UtcNow && DateTime.UtcNow <= p.EndDate) &&
                       p.Type == PenaltyType.AccountBan)))
               .ForMember(dest => dest.IsMuted, opt => opt.MapFrom(src =>
                    src.Penalties.Any(p => !p.IsDeleted &&
                       (p.IsPermanent || p.StartDate <= DateTime.UtcNow && DateTime.UtcNow <= p.EndDate) &&
                       p.Type == PenaltyType.Mute)))
               .ForMember(dest => dest.MuteEndDate, opt => opt.MapFrom(src =>
                    src.Penalties
                   .Where(p => !p.IsDeleted &&
                               p.Type == PenaltyType.Mute &&
                               (p.IsPermanent || p.StartDate <= DateTime.UtcNow && DateTime.UtcNow <= p.EndDate))
                   .OrderByDescending(p => p.EndDate) // Get the most relevant mute penalty
                   .Select(p => p.IsPermanent ? (DateTime?)null : p.EndDate)
                   .FirstOrDefault())); // Get the first (if any) mute end date 

            CreateMap<ApplicationUser, GetUserCredentialsDto>();
            CreateMap<RegisterUserDto, ApplicationUser>();
            CreateMap<LoginUserDto, ApplicationUser>();
            CreateMap<ApplicationUser, RefreshTokenDto>();
            CreateMap<FileData, FileDataDto>();
            CreateMap<CreateUserDto, ApplicationUser>();
            CreateMap<GetPenaltyDto, Penalty>();
            CreateMap<Penalty, GetPenaltyDto>();
            CreateMap<CreatePenaltyDto, Penalty>();
            CreateMap<UserProfile, GetUserProfileDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User!.UserName));
            CreateMap<FileDataDto, FileData>();
            CreateMap<GetAttractionDto, GetAttractionDto>();

            // Mappings for Schedule
            CreateMap<Schedule, GetScheduleDto>();
            CreateMap<CreateScheduleDto, Schedule>();

            // Mappings for ScheduleTimeSlot
            CreateMap<ScheduleTimeSlot, GetScheduleTimeSlotDto>();
            CreateMap<CreateScheduleTimeSlotDto, ScheduleTimeSlot>();

            // Mappings for SeasonalAvailability
            CreateMap<SeasonalAvailability, GetSeasonalAvailabilityDto>();
            CreateMap<CreateSeasonalAvailabilityDto, SeasonalAvailability>();

            // Mappings for SpecialDay
            CreateMap<SpecialDay, GetSpecialDayDto>();
            CreateMap<CreateSpecialDayDto, SpecialDay>();

            CreateMap<CreateCountryDto, Country>();
            CreateMap<Country, GetCountryDto>();

            CreateMap<CreateProvinceDto, Province>();
            CreateMap<Province, GetProvinceDto>();

            CreateMap<CreateLocalityDto, Locality>();
            CreateMap<Locality, GetLocalityDto>();

            CreateMap<CreateAttractionDto, Attraction>();
            CreateMap<Attraction, GetAttractionDto>();

            // Mappings for Category
            CreateMap<CreateAttractionCategoryDto, AttractionCategory>();
            CreateMap<AttractionCategory, GetAttractionCategoryDto>();

            // Mappings for Tag
            CreateMap<CreateAttractionTagDto, AttractionTag>();
            CreateMap<AttractionTag, GetAttractionTagDto>();

            CreateMap<Address, GetAddressDto>()
                .ForMember(dest => dest.Locality, opt => opt.MapFrom(src => src.Locality))
                .ForMember(dest => dest.LocalityId, opt => opt.MapFrom(src => src.Locality!.Id))
                .ForMember(dest => dest.ProvinceId, opt => opt.MapFrom(src => src.Locality!.ProvinceId))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.Locality!.Province!.CountryId));

            CreateMap<Region, GetRegionDto>();

            CreateMap<Attraction, GetAttractionDto>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address ?? new Address()))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category ?? new AttractionCategory()))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags ?? new List<AttractionTag>()))
                .ForMember(dest => dest.Regions, opt => opt.MapFrom(src => src.Regions ?? new List<Region>()))
                .ForMember(dest => dest.Schedules, opt => opt.MapFrom(src => src.Schedules ?? new List<Schedule>()))
                .ForMember(dest => dest.SeasonalAvailabilities, opt => opt.MapFrom(src => src.SeasonalAvailabilities ?? new List<SeasonalAvailability>()))
                .ForMember(dest => dest.SpecialDays, opt => opt.MapFrom(src => src.SpecialDays ?? new List<SpecialDay>()))
                .ForMember(dest => dest.MainImageId, opt => opt.MapFrom(src => src.MainImageId))
                .ForPath(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForPath(dest => dest.Address!.Locality, opt => opt.MapFrom(src => src.Address!.Locality))
                .ForPath(dest => dest.Address!.Locality!.Province, opt => opt.MapFrom(src => src.Address!.Locality!.Province))
                .ForPath(dest => dest.Address!.Locality!.Province!.Country, opt => opt.MapFrom(src => src.Address!.Locality!.Province!.Country));

            CreateMap<Trail, GetTrailDto>()
                .ForMember(dest => dest.AttractionInTrailIds, opt => opt.MapFrom(src => src.Attractions!.Select(a => a!.Id).ToList()))
                .ForMember(dest => dest.CreatedById, opt => opt.MapFrom(src => src.CreatedBy!.Id));
            //.ForMember(dest => dest.TransportationModes, opt => opt.MapFrom(src => src.Attractions!.Select(a => a!.TransportationMode).Distinct().ToList()))
            //.ForMember(dest => dest.TotalTravelDistance, opt => opt.MapFrom(src => src.Attractions!.Sum(a => a!.TravelDistance)))
            //.ForMember(dest => dest.TotalTravelTime, opt => opt.MapFrom(src =>
            //    TimeSpan.FromMinutes(src.Attractions!.Sum(a => a!.TravelTime) + src.Attractions!.Sum(a => a!.Attraction!.AverageVisitDuration.TotalMinutes))));

            CreateMap<CreateTrailDto, Trail>();

            CreateMap<AttractionInTrail, GetAttractionInTrailDto>();
        }
    }

    public class FileIdsResolver : IValueResolver<Attraction, GetAttractionDto, List<Guid>>
    {
        private readonly TravelPlannerContext _context;

        public FileIdsResolver(TravelPlannerContext context)
        {
            _context = context;
        }

        public List<Guid> Resolve(Attraction source, GetAttractionDto destination, List<Guid> destMember, ResolutionContext context)
        {
            return _context.Set<FileData>()
                           .Where(fd => fd.EntityType == EntityType.Attraction && fd.EntityId == source.Id)
                           .Select(fd => fd.FileId)
                           .ToList();
        }
    }
}

