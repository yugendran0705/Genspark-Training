using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleServiceAPI.Migrations
{
    /// <inheritdoc />
    public partial class Init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Images",
                newName: "Base64Data");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Base64Data",
                table: "Images",
                newName: "FilePath");
        }
    }
}
