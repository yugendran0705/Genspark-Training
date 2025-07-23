using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleServiceAPI.Migrations
{
    /// <inheritdoc />
    public partial class invoiceupdate : Migration
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

            migrationBuilder.AddColumn<int>(
                name: "DiscountPercentage",
                table: "Invoices",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountFlag",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "Invoices");
        }
    }
}
