using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaTrackerYoutubeService.Migrations
{
    /// <inheritdoc />
    public partial class AddBidirectionalReferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Playlists_Users_UserId", table: "Playlists");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Playlists_PlaylistYoutubeId",
                table: "Videos"
            );

            migrationBuilder.DropIndex(name: "IX_Videos_PlaylistYoutubeId", table: "Videos");

            migrationBuilder.DropColumn(name: "PlaylistYoutubeId", table: "Videos");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Playlists",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true
            );

            migrationBuilder.CreateTable(
                name: "PlaylistVideo",
                columns: table =>
                    new
                    {
                        PlaylistYoutubeId = table.Column<string>(
                            type: "nvarchar(450)",
                            nullable: false
                        ),
                        VideosYoutubeId = table.Column<string>(
                            type: "nvarchar(450)",
                            nullable: false
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_PlaylistVideo",
                        x => new { x.PlaylistYoutubeId, x.VideosYoutubeId }
                    );
                    table.ForeignKey(
                        name: "FK_PlaylistVideo_Playlists_PlaylistYoutubeId",
                        column: x => x.PlaylistYoutubeId,
                        principalTable: "Playlists",
                        principalColumn: "YoutubeId",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_PlaylistVideo_Videos_VideosYoutubeId",
                        column: x => x.VideosYoutubeId,
                        principalTable: "Videos",
                        principalColumn: "YoutubeId",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistVideo_VideosYoutubeId",
                table: "PlaylistVideo",
                column: "VideosYoutubeId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_Users_UserId",
                table: "Playlists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Playlists_Users_UserId", table: "Playlists");

            migrationBuilder.DropTable(name: "PlaylistVideo");

            migrationBuilder.AddColumn<string>(
                name: "PlaylistYoutubeId",
                table: "Videos",
                type: "nvarchar(450)",
                nullable: true
            );

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Playlists",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Videos_PlaylistYoutubeId",
                table: "Videos",
                column: "PlaylistYoutubeId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_Users_UserId",
                table: "Playlists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Playlists_PlaylistYoutubeId",
                table: "Videos",
                column: "PlaylistYoutubeId",
                principalTable: "Playlists",
                principalColumn: "YoutubeId"
            );
        }
    }
}
