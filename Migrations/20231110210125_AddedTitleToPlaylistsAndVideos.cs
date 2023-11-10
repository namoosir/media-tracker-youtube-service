using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaTrackerYoutubeService.Migrations
{
    /// <inheritdoc />
    public partial class AddedTitleToPlaylistsAndVideos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Playlists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: ""
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Title", table: "Videos");

            migrationBuilder.DropColumn(name: "Title", table: "Playlists");
        }
    }
}
