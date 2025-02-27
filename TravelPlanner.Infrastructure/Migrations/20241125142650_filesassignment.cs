using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelPlanner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class filesassignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attractions_FilesData_MainImageId",
                table: "Attractions");

            migrationBuilder.DropForeignKey(
                name: "FK_FilesData_Attractions_AttractionId",
                table: "FilesData");

            migrationBuilder.DropForeignKey(
                name: "FK_FilesData_UserProfiles_UserProfileId",
                table: "FilesData");

            migrationBuilder.DropIndex(
                name: "IX_FilesData_AttractionId",
                table: "FilesData");

            migrationBuilder.DropIndex(
                name: "IX_FilesData_UserProfileId",
                table: "FilesData");

            migrationBuilder.DropIndex(
                name: "IX_Attractions_MainImageId",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "AttractionId",
                table: "FilesData");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "FilesData");

            migrationBuilder.DropColumn(
                name: "MainImageId",
                table: "Attractions");

            migrationBuilder.AddColumn<Guid>(
                name: "MainImageId",
                table: "Attractions",
                type: "uniqueidentifier",
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttractionId",
                table: "FilesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserProfileId",
                table: "FilesData",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MainImageId",
                table: "Attractions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FilesData_AttractionId",
                table: "FilesData",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_FilesData_UserProfileId",
                table: "FilesData",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Attractions_MainImageId",
                table: "Attractions",
                column: "MainImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attractions_FilesData_MainImageId",
                table: "Attractions",
                column: "MainImageId",
                principalTable: "FilesData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FilesData_Attractions_AttractionId",
                table: "FilesData",
                column: "AttractionId",
                principalTable: "Attractions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FilesData_UserProfiles_UserProfileId",
                table: "FilesData",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id");
        }
    }
}
