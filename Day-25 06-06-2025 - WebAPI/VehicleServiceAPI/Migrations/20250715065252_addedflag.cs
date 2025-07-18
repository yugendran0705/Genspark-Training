using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleServiceAPI.Migrations
{
    /// <inheritdoc />
    public partial class addedflag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DiscountFlag",
                table: "Invoices",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountFlag",
                table: "Invoices");
        }
    }
}
