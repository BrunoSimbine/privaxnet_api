using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace privaxnet_api.Migrations
{
    /// <inheritdoc />
    public partial class OtherMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Wallets",
                newName: "Fullname");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "PayAgents",
                newName: "Fullname");

            migrationBuilder.RenameColumn(
                name: "ExchangeRate",
                table: "Currencies",
                newName: "Rate");

            migrationBuilder.RenameColumn(
                name: "CurrencySymbol",
                table: "Currencies",
                newName: "Symbol");

            migrationBuilder.RenameColumn(
                name: "CurrencyName",
                table: "Currencies",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Fullname",
                table: "Wallets",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "Fullname",
                table: "PayAgents",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Symbol",
                table: "Currencies",
                newName: "CurrencySymbol");

            migrationBuilder.RenameColumn(
                name: "Rate",
                table: "Currencies",
                newName: "ExchangeRate");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Currencies",
                newName: "CurrencyName");
        }
    }
}
