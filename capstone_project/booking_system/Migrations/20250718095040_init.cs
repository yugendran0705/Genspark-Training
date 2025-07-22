using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace booking_system.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WalletCustomerEmail",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    CustomerEmail = table.Column<string>(type: "text", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    balance = table.Column<int>(type: "integer", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.CustomerEmail);
                    table.ForeignKey(
                        name: "FK_Wallets_Customers_CustomerEmail",
                        column: x => x.CustomerEmail,
                        principalTable: "Customers",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_WalletCustomerEmail",
                table: "Users",
                column: "WalletCustomerEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Wallets_WalletCustomerEmail",
                table: "Users",
                column: "WalletCustomerEmail",
                principalTable: "Wallets",
                principalColumn: "CustomerEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Wallets_WalletCustomerEmail",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Users_WalletCustomerEmail",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WalletCustomerEmail",
                table: "Users");
        }
    }
}
