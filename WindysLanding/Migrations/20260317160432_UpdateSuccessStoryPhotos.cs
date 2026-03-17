using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WindysLanding.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSuccessStoryPhotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "SuccessStories");

            migrationBuilder.AddColumn<int>(
                name: "SuccessStoryId",
                table: "Photos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_SuccessStoryId",
                table: "Photos",
                column: "SuccessStoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_SuccessStories_SuccessStoryId",
                table: "Photos",
                column: "SuccessStoryId",
                principalTable: "SuccessStories",
                principalColumn: "StoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_SuccessStories_SuccessStoryId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_SuccessStoryId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "SuccessStoryId",
                table: "Photos");

            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "SuccessStories",
                type: "int",
                nullable: true);
        }
    }
}
