using Audit.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using TravelPlanner.Domain.Models;
using TravelPlanner.Domain.Models.Attractions;
using TravelPlanner.Domain.Models.Attractions.Location;
using TravelPlanner.Domain.Models.Attractions.Time;
using TravelPlanner.Domain.Models.Trails;
using TravelPlanner.Shared.DTOs.Attraction;

namespace TravelPlanner.Infrastructure
{
    public class TravelPlannerContext : AuditIdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TravelPlannerContext(DbContextOptions<TravelPlannerContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public DbSet<InitializationLog> InitializationLogs { get; set; }
        public DbSet<Penalty> Penalties { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<FileData> FilesData { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        // <-- Attraction data -->
        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<AttractionCategory> AttractionCategories { get; set; }
        public DbSet<AttractionTag> AttractionTags { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Locality> Localities { get; set; }
        public DbSet<ZipCode> ZipCodes { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public DbSet<Region> Region { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<ScheduleTimeSlot> ScheduleTimeSlot { get; set; }
        public DbSet<SeasonalAvailability> SeasonalAvailability { get; set; }
        public DbSet<SpecialDay> SpecialDay { get; set; }

        public DbSet<Trail> Trails { get; set; }
        public DbSet<AttractionInTrail> AttractionInTrails { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure the Email column to be of type varchar(320)
            builder.Entity<ApplicationUser>()
                .Property(u => u.Email)
                .HasColumnType("varchar(320)");

            // Configure other properties as needed
            builder.Entity<ApplicationUser>()
                .Property(u => u.PasswordHash)
                .HasMaxLength(256);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.UserProfile)
                .WithOne(up => up.User)
                .HasForeignKey<UserProfile>(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Penalties)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            builder.Entity<Penalty>()
                .HasOne(p => p.IssuedBy)
                .WithMany()
                .HasForeignKey(p => p.IssuedById);

            builder.Entity<AuditLog>()
                .HasOne(a => a.ChangedBy)
                .WithMany()
                .HasForeignKey(a => a.ChangedById);

            builder.Entity<FileData>()
                .HasOne(f => f.UploadedBy)
                .WithMany(u => u.Files)
                .HasForeignKey(f => f.UploadedById)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<GetAttractionDto>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("vw_AttractionDetails");

                // Basic properties
                entity.Property(e => e.Id).HasColumnName("AttractionId");
                entity.Property(e => e.Name).HasColumnName("AttractionName");
                entity.Property(e => e.Description).HasColumnName("AttractionDescription");
                entity.Property(e => e.MainImageId).HasColumnName("MainImageId");

                //// Address
                //entity.Property(e => e.Address)
                //    .HasColumnName("Address")
                //    .HasConversion(
                //        v => JsonSerializer.Serialize(v, new JsonSerializerOptions { WriteIndented = false }),
                //        v => string.IsNullOrWhiteSpace(v)
                //            ? null
                //            : JsonSerializer.Deserialize<GetAddressDto>(v, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                //    );

                //// Category
                //entity.Property(e => e.Category)
                //    .HasColumnName("Category")
                //    .HasConversion(
                //        v => JsonSerializer.Serialize(v, new JsonSerializerOptions { WriteIndented = false }),
                //        v => string.IsNullOrWhiteSpace(v)
                //            ? null
                //            : JsonSerializer.Deserialize<GetAttractionCategoryDto>(v, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                //    );

                //entity.Property(e => e.Regions)
                //    .HasColumnName("Regions")
                //    .HasConversion(
                //        v => JsonSerializer.Serialize(v, new JsonSerializerOptions { WriteIndented = false }),
                //        v => string.IsNullOrWhiteSpace(v)
                //            ? new List<GetRegionDto>()
                //            : JsonSerializer.Deserialize<List<GetRegionDto>>(v, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!
                //    )
                //    .Metadata.SetValueComparer(ValueComparers.JsonListComparer<GetRegionDto>());

                //entity.Property(e => e.Schedules)
                //    .HasColumnName("Schedules")
                //    .HasConversion(
                //        v => JsonSerializer.Serialize(v, new JsonSerializerOptions { WriteIndented = false }),
                //        v => string.IsNullOrWhiteSpace(v)
                //            ? new List<GetScheduleDto>()
                //            : JsonSerializer.Deserialize<List<GetScheduleDto>>(v, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!
                //    )
                //    .Metadata.SetValueComparer(ValueComparers.JsonListComparer<GetScheduleDto>());

                //entity.Property(e => e.SeasonalAvailabilities)
                //    .HasColumnName("SeasonalAvailabilities")
                //    .HasConversion(
                //        v => JsonSerializer.Serialize(v, new JsonSerializerOptions { WriteIndented = false }),
                //        v => string.IsNullOrWhiteSpace(v)
                //            ? new List<GetSeasonalAvailabilityDto>()
                //            : JsonSerializer.Deserialize<List<GetSeasonalAvailabilityDto>>(v, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!
                //    )
                //    .Metadata.SetValueComparer(ValueComparers.JsonListComparer<GetSeasonalAvailabilityDto>());

                //entity.Property(e => e.SpecialDays)
                //    .HasColumnName("SpecialDays")
                //    .HasConversion(
                //        v => JsonSerializer.Serialize(v, new JsonSerializerOptions { WriteIndented = false }),
                //        v => string.IsNullOrWhiteSpace(v)
                //            ? new List<CreateSpecialDayDto>()
                //            : JsonSerializer.Deserialize<List<CreateSpecialDayDto>>(v, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!
                //    )
                //    .Metadata.SetValueComparer(ValueComparers.JsonListComparer<CreateSpecialDayDto>());

                //entity.Property(e => e.Tags)
                //    .HasColumnName("Tags")
                //    .HasConversion(
                //        v => JsonSerializer.Serialize(v, new JsonSerializerOptions { WriteIndented = false }),
                //        v => string.IsNullOrWhiteSpace(v)
                //            ? new List<GetAttractionTagDto>()
                //            : JsonSerializer.Deserialize<List<GetAttractionTagDto>>(v, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!
                //    )
                //    .Metadata.SetValueComparer(ValueComparers.JsonListComparer<GetAttractionTagDto>());
            });

            // Trail
            builder.Entity<Trail>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Name).HasMaxLength(128);
                entity.Property(t => t.Description).HasMaxLength(512);
                entity.Property(t => t.ImageId).IsRequired(false);
                entity.HasMany(t => t.Attractions)
                    .WithOne(a => a.Trail);
                entity.HasOne(t => t.CreatedBy)
                    .WithMany(u => u.Trails);
            });

            // AttractionInTrail
            builder.Entity<AttractionInTrail>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.HasOne(a => a.Attraction)
                    .WithMany();
                entity.HasOne(a => a.Trail)
                    .WithMany(t => t.Attractions);
            });
        }

        private static void ConfigureAddress(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(a => a.Id);

            // Locality relationship - No cascading behavior
            builder.HasOne(a => a.Locality)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            // ZipCode relationship - No cascading behavior
            builder.HasOne(a => a.ZipCode)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }

        private static void ConfigureProvince(EntityTypeBuilder<Province> builder)
        {
            builder.HasKey(p => p.Id);

            // Relationship with Country - No cascading deletes
            builder.HasOne(p => p.Country)
                .WithMany(c => c.Provinces)
                .OnDelete(DeleteBehavior.NoAction);

            // Relationship with Localities - No cascading deletes
            builder.HasMany(p => p.Localities)
                .WithOne(l => l.Province)
                .OnDelete(DeleteBehavior.NoAction);
        }

        private static void ConfigureLocality(EntityTypeBuilder<Locality> builder)
        {
            builder.HasKey(l => l.Id);

            // Relationship with Province - No cascading deletes
            builder.HasOne(l => l.Province)
                .WithMany(p => p.Localities)
                .OnDelete(DeleteBehavior.NoAction);

            // Relationship with ZipCodes - Cascade if necessary, or NoAction
            builder.HasMany(l => l.ZipCodes)
                .WithOne(z => z.Locality)
                .OnDelete(DeleteBehavior.Cascade); // Keep Cascade for ZipCode cleanup
        }


        private static void ConfigureCountry(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(c => c.Id);

            // Relationship with Provinces - No cascading deletes
            builder.HasMany(c => c.Provinces)
                .WithOne(p => p.Country)
                .OnDelete(DeleteBehavior.NoAction);
        }

        private static void ConfigureAttraction(EntityTypeBuilder<Attraction> builder)
        {
            builder.HasKey(a => a.Id);

            // One-to-one relationship with Address
            builder.HasOne(a => a.Address)
                .WithOne()
                .HasForeignKey<Attraction>(a => a.Id)
                .OnDelete(DeleteBehavior.SetNull);

            // Many-to-many relationship with Tags
            builder.HasMany(a => a.Tags)
                .WithMany(t => t.Attractions);

            // Many-to-many relationship with Regions
            builder.HasMany(a => a.Regions)
                .WithMany(r => r.Attractions);
        }

        private static void ConfigureAttractionCategory(EntityTypeBuilder<AttractionCategory> builder)
        {
            builder.HasKey(ac => ac.Id);
            builder.HasMany(ac => ac.Attractions)
                .WithOne(a => a.Category)
                .OnDelete(DeleteBehavior.SetNull);
        }

        private static void ConfigureAttractionTag(EntityTypeBuilder<AttractionTag> builder)
        {
            builder.HasKey(at => at.Id);
        }

        private static void ConfigureZipCode(EntityTypeBuilder<ZipCode> builder)
        {
            builder.HasKey(z => z.Id);

            // ZipCode belongs to a Locality
            builder.HasOne(z => z.Locality)
                .WithMany(l => l.ZipCodes)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureRegion(EntityTypeBuilder<Region> builder)
        {
            builder.HasKey(r => r.Id);

            // Many-to-many relationship with Attraction
            builder.HasMany(r => r.Attractions)
                .WithMany(a => a.Regions);
        }

        private static void ConfigureSchedule(EntityTypeBuilder<Schedule> builder)
        {
            builder.HasKey(s => s.Id);

            // Schedule belongs to an Attraction
            builder.HasOne(s => s.Attraction)
                .WithMany(a => a.Schedules)
                .OnDelete(DeleteBehavior.Cascade);

            // Schedule has many TimeSlots
            builder.HasMany(s => s.TimeSlots)
                .WithOne(ts => ts.Schedule)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureScheduleTimeSlot(EntityTypeBuilder<ScheduleTimeSlot> builder)
        {
            builder.HasKey(ts => ts.Id);

            // ScheduleTimeSlot belongs to a Schedule
            builder.HasOne(ts => ts.Schedule)
                .WithMany(s => s.TimeSlots)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureSeasonalAvailability(EntityTypeBuilder<SeasonalAvailability> builder)
        {
            builder.HasKey(sa => sa.Id);

            // SeasonalAvailability belongs to an Attraction
            builder.HasOne(sa => sa.Attraction)
                .WithMany(a => a.SeasonalAvailabilities)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureSpecialDay(EntityTypeBuilder<SpecialDay> builder)
        {
            builder.HasKey(sd => sd.Id);

            // SpecialDay belongs to an Attraction
            builder.HasOne(sd => sd.Attraction)
                .WithMany(a => a.SpecialDays)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }


    public static class ValueComparers
    {
        public static ValueComparer<List<T>> JsonListComparer<T>() =>
            new ValueComparer<List<T>>(
                (c1, c2) => JsonSerializer.Serialize(c1, new JsonSerializerOptions { WriteIndented = false }) ==
                            JsonSerializer.Serialize(c2, new JsonSerializerOptions { WriteIndented = false }),
                c => JsonSerializer.Serialize(c, new JsonSerializerOptions { WriteIndented = false }).GetHashCode(),
                c => c == null
                    ? null
                    : JsonSerializer.Deserialize<List<T>>(JsonSerializer.Serialize(c, new JsonSerializerOptions { WriteIndented = false }),
                                                          new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            );
    }
}
