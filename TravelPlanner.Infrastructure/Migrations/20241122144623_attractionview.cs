using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelPlanner.Infrastructure.Migrations
{
    public partial class attractionview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW vw_AttractionDetails AS
                SELECT 
                    -- Attraction Information
                    a.Id AS AttractionId,
                    a.Name AS AttractionName,
                    a.Description AS AttractionDescription,
                    a.WebsiteUrl,
                    a.PhoneNumber,
                    a.Email,
                    a.CreatedAt,
                    a.UpdatedAt,
                    a.IsDeleted,
                    a.IsPublic,

                    -- Address Information
                    adr.Id AS AddressId,
                    adr.Street AS Street,
                    adr.Latitude AS Latitude,
                    adr.Longitude AS Longitude,
                    loc.Name AS LocalityName,
                    prov.Name AS ProvinceName,
                    ctry.Name AS CountryName,

                    -- Category Information
                    cat.Id AS CategoryId,
                    cat.Name AS CategoryName,

                    -- Tags as JSON Array
                    (
                        SELECT 
                            t.Id, t.Name
                        FROM AttractionAttractionTag ata
                        INNER JOIN AttractionTags t ON ata.TagsId = t.Id
                        WHERE ata.AttractionsId = a.Id
                        FOR JSON PATH
                    ) AS Tags,

                    -- Regions as JSON Array
                    (
                        SELECT 
                            r.Id, r.Name
                        FROM AttractionRegion ar
                        INNER JOIN Region r ON ar.RegionsId = r.Id
                        WHERE ar.AttractionsId = a.Id
                        FOR JSON PATH
                    ) AS Regions,

                    -- Files as JSON Array
                    (
                        SELECT 
                            f.Id, f.FileId, f.FileName, f.Extension, f.ContentType, f.Size, 
                            f.UploadedAt, f.IsUploaded, f.IsPublic, f.EntityId
                        FROM FilesData f
                        WHERE f.EntityId = a.Id AND f.EntityType = 'Attraction'
                        FOR JSON PATH
                    ) AS Files,

                    -- Main Image as JSON Object
                    (
                        SELECT 
                            TOP 1 f.Id, f.FileId, f.FileName, f.Extension, f.ContentType, f.Size, 
                            f.UploadedAt, f.IsUploaded, f.IsPublic, f.EntityId
                        FROM FilesData f
                        WHERE f.EntityId = a.Id AND f.EntityType = 'Attraction'
                        ORDER BY f.UploadedAt DESC
                        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
                    ) AS MainImage,

                    -- Schedules as JSON Array
                    (
                        SELECT 
                            s.Id, s.DayOfWeek,
                            (
                                SELECT 
                                    ts.Id AS TimeSlotId, ts.StartTime, ts.EndTime
                                FROM ScheduleTimeSlot ts
                                WHERE ts.ScheduleId = s.Id
                                FOR JSON PATH
                            ) AS TimeSlots
                        FROM Schedule s
                        WHERE s.AttractionId = a.Id
                        FOR JSON PATH
                    ) AS Schedules,

                    -- Seasonal Availabilities as JSON Array
                    (
                        SELECT 
                            sa.Id, sa.StartDate, sa.EndDate, sa.IsOpen
                        FROM SeasonalAvailability sa
                        WHERE sa.AttractionId = a.Id
                        FOR JSON PATH
                    ) AS SeasonalAvailabilities,

                    -- Special Days as JSON Array
                    (
                        SELECT 
                            sd.Id, sd.Date, sd.IsOpen, sd.OpeningTime, sd.ClosingTime
                        FROM SpecialDay sd
                        WHERE sd.AttractionId = a.Id
                        FOR JSON PATH
                    ) AS SpecialDays

                FROM Attractions a
                LEFT JOIN Addresses adr ON a.AddressId = adr.Id
                LEFT JOIN Localities loc ON adr.LocalityId = loc.Id
                LEFT JOIN Provinces prov ON loc.ProvinceId = prov.Id
                LEFT JOIN Countries ctry ON prov.CountryId = ctry.Id
                LEFT JOIN AttractionCategories cat ON a.CategoryId = cat.Id;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_AttractionDetails;");
        }
    }
}
