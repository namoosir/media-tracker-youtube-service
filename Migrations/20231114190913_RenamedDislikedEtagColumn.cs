using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaTrackerYoutubeService.Migrations
{
    /// <inheritdoc />
    public partial class RenamedDislikedEtagColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DisikedVideosEtag",
                table: "Users",
                newName: "DislikedVideosEtag"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DislikedVideosEtag",
                table: "Users",
                newName: "DisikedVideosEtag"
            );
        }
    }
}
