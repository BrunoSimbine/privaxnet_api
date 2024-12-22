using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace privaxnet_api.Migrations
{
    /// <inheritdoc />
    public partial class ExpirationTokenMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Users",
                newName: "Token");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Token",
                table: "Users",
                newName: "Status");
        }
    }
}
