using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelPlanner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class trails2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Trails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Trails",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateIndex(
                name: "IX_Trails_CreatedById",
                table: "Trails",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Trails_AspNetUsers_CreatedById",
                table: "Trails",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trails_AspNetUsers_CreatedById",
                table: "Trails");

            migrationBuilder.DropIndex(
                name: "IX_Trails_CreatedById",
                table: "Trails");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Trails");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Trails");
        }
    }
}
