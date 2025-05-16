using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coinnova.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthProviderToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthProvider",
                table: "User",
                type: "character varying(15)",
                nullable: false,
                defaultValue: "Local");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthProvider",
                table: "User");
        }
    }
}
