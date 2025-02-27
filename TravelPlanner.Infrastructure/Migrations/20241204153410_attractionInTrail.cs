using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelPlanner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class attractionInTrail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttractionTrail");

            migrationBuilder.AddColumn<int>(
                name: "AttractionId",
                table: "Trails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AttractionInTrails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttractionId = table.Column<int>(type: "int", nullable: false),
                    TrailId = table.Column<int>(type: "int", nullable: false),
                    TransportationMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TravelTime = table.Column<double>(type: "float", nullable: false),
                    TravelDistance = table.Column<double>(type: "float", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionInTrails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttractionInTrails_Attractions_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttractionInTrails_Trails_TrailId",
                        column: x => x.TrailId,
                        principalTable: "Trails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trails_AttractionId",
                table: "Trails",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionInTrails_AttractionId",
                table: "AttractionInTrails",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionInTrails_TrailId",
                table: "AttractionInTrails",
                column: "TrailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trails_Attractions_AttractionId",
                table: "Trails",
                column: "AttractionId",
                principalTable: "Attractions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trails_Attractions_AttractionId",
                table: "Trails");

            migrationBuilder.DropTable(
                name: "AttractionInTrails");

            migrationBuilder.DropIndex(
                name: "IX_Trails_AttractionId",
                table: "Trails");

            migrationBuilder.DropColumn(
                name: "AttractionId",
                table: "Trails");

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

            migrationBuilder.CreateIndex(
                name: "IX_AttractionTrail_TrailsId",
                table: "AttractionTrail",
                column: "TrailsId");
        }
    }
}
