using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coinnova.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentCountAndReplyCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "comment_count",
                table: "post",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "reply_count",
                table: "comment",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "comment_count",
                table: "post");

            migrationBuilder.DropColumn(
                name: "reply_count",
                table: "comment");
        }
    }
}
