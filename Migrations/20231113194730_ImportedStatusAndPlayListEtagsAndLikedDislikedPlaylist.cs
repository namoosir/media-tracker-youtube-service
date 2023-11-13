using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaTrackerYoutubeService.Migrations
{
    /// <inheritdoc />
    public partial class ImportedStatusAndPlayListEtagsAndLikedDislikedPlaylist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Imported",
                table: "Videos",
                type: "bit",
                nullable: false,
                defaultValue: false
            );

            migrationBuilder.AddColumn<string>(
                name: "DisikedVideosEtag",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<string>(
                name: "LikedVideosEtag",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<string>(
                name: "PlaylistsEtag",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<bool>(
                name: "Imported",
                table: "Channels",
                type: "bit",
                nullable: false,
                defaultValue: false
            );

            migrationBuilder.CreateTable(
                name: "UserVideo",
                columns: table =>
                    new
                    {
                        LikedByUsersUserId = table.Column<int>(type: "int", nullable: false),
                        LikedVideosYoutubeId = table.Column<string>(
                            type: "nvarchar(450)",
                            nullable: false
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_UserVideo",
                        x => new { x.LikedByUsersUserId, x.LikedVideosYoutubeId }
                    );
                    table.ForeignKey(
                        name: "FK_UserVideo_Users_LikedByUsersUserId",
                        column: x => x.LikedByUsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_UserVideo_Videos_LikedVideosYoutubeId",
                        column: x => x.LikedVideosYoutubeId,
                        principalTable: "Videos",
                        principalColumn: "YoutubeId",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "UserVideo1",
                columns: table =>
                    new
                    {
                        DislikedByUsersUserId = table.Column<int>(type: "int", nullable: false),
                        DislikedVideosYoutubeId = table.Column<string>(
                            type: "nvarchar(450)",
                            nullable: false
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_UserVideo1",
                        x => new { x.DislikedByUsersUserId, x.DislikedVideosYoutubeId }
                    );
                    table.ForeignKey(
                        name: "FK_UserVideo1_Users_DislikedByUsersUserId",
                        column: x => x.DislikedByUsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_UserVideo1_Videos_DislikedVideosYoutubeId",
                        column: x => x.DislikedVideosYoutubeId,
                        principalTable: "Videos",
                        principalColumn: "YoutubeId",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_UserVideo_LikedVideosYoutubeId",
                table: "UserVideo",
                column: "LikedVideosYoutubeId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_UserVideo1_DislikedVideosYoutubeId",
                table: "UserVideo1",
                column: "DislikedVideosYoutubeId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "UserVideo");

            migrationBuilder.DropTable(name: "UserVideo1");

            migrationBuilder.DropColumn(name: "Imported", table: "Videos");

            migrationBuilder.DropColumn(name: "DisikedVideosEtag", table: "Users");

            migrationBuilder.DropColumn(name: "LikedVideosEtag", table: "Users");

            migrationBuilder.DropColumn(name: "PlaylistsEtag", table: "Users");

            migrationBuilder.DropColumn(name: "Imported", table: "Channels");
        }
    }
}
