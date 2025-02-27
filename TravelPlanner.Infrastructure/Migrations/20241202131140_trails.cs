using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelPlanner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class trails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "AverageVisitDuration",
                table: "Attractions",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<double>(
                name: "EnergyLevel",
                table: "Attractions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Popularity",
                table: "Attractions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "GetAttractionCategoryDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetAttractionCategoryDto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GetCountryDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetCountryDto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GetProvinceDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetProvinceDto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GetProvinceDto_GetCountryDto_CountryId",
                        column: x => x.CountryId,
                        principalTable: "GetCountryDto",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AttractionTrail",
                columns: table => new
                {
                    AttractionsId = table.Column<int>(type: "int", nullable: false),
                    TrailsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionTrail", x => new { x.AttractionsId, x.TrailsId });
                    table.ForeignKey(
                        name: "FK_AttractionTrail_Attractions_AttractionsId",
                        column: x => x.AttractionsId,
                        principalTable: "Attractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttractionTrail_Trails_TrailsId",
                        column: x => x.TrailsId,
                        principalTable: "Trails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GetLocalityDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetLocalityDto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GetLocalityDto_GetProvinceDto_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "GetProvinceDto",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GetAddressDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalityId = table.Column<int>(type: "int", nullable: true),
                    ProvinceId = table.Column<int>(type: "int", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetAddressDto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GetAddressDto_GetLocalityDto_LocalityId",
                        column: x => x.LocalityId,
                        principalTable: "GetLocalityDto",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttractionTrail_TrailsId",
                table: "AttractionTrail",
                column: "TrailsId");

            migrationBuilder.CreateIndex(
                name: "IX_GetAddressDto_LocalityId",
                table: "GetAddressDto",
                column: "LocalityId");

            migrationBuilder.CreateIndex(
                name: "IX_GetLocalityDto_ProvinceId",
                table: "GetLocalityDto",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_GetProvinceDto_CountryId",
                table: "GetProvinceDto",
                column: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttractionTrail");

            migrationBuilder.DropTable(
                name: "GetAddressDto");

            migrationBuilder.DropTable(
                name: "GetAttractionCategoryDto");

            migrationBuilder.DropTable(
                name: "Trails");

            migrationBuilder.DropTable(
                name: "GetLocalityDto");

            migrationBuilder.DropTable(
                name: "GetProvinceDto");

            migrationBuilder.DropTable(
                name: "GetCountryDto");

            migrationBuilder.DropColumn(
                name: "AverageVisitDuration",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "EnergyLevel",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "Popularity",
                table: "Attractions");
        }
    }
}
