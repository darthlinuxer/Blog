using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Migrations
{
    /// <inheritdoc />
    public partial class AddedPostStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostStatus",
                table: "Posts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostStatus",
                table: "Posts");
        }
    }
}
