using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaTrackerYoutubeService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "UserVideos");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table =>
                    new
                    {
                        UserId = table.Column<int>(type: "int", nullable: false),
                        CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                        UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                }
            );

            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table =>
                    new
                    {
                        YoutubeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                        Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        SubscriberCount = table.Column<int>(type: "int", nullable: true),
                        ViewCount = table.Column<int>(type: "int", nullable: true),
                        VideoCount = table.Column<int>(type: "int", nullable: true),
                        ThumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        ETag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        UserId = table.Column<int>(type: "int", nullable: true),
                        CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                        UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.YoutubeId);
                    table.ForeignKey(
                        name: "FK_Channels_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId"
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table =>
                    new
                    {
                        YoutubeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                        ETag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        UserId = table.Column<int>(type: "int", nullable: true),
                        CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                        UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.YoutubeId);
                    table.ForeignKey(
                        name: "FK_Playlists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId"
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table =>
                    new
                    {
                        YoutubeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                        ChannelYoutubeId = table.Column<string>(
                            type: "nvarchar(450)",
                            nullable: true
                        ),
                        ViewCount = table.Column<int>(type: "int", nullable: true),
                        LikeCount = table.Column<int>(type: "int", nullable: true),
                        CommentCount = table.Column<int>(type: "int", nullable: true),
                        ThumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        ETag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        PlaylistYoutubeId = table.Column<string>(
                            type: "nvarchar(450)",
                            nullable: true
                        ),
                        CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                        UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.YoutubeId);
                    table.ForeignKey(
                        name: "FK_Videos_Channels_ChannelYoutubeId",
                        column: x => x.ChannelYoutubeId,
                        principalTable: "Channels",
                        principalColumn: "YoutubeId"
                    );
                    table.ForeignKey(
                        name: "FK_Videos_Playlists_PlaylistYoutubeId",
                        column: x => x.PlaylistYoutubeId,
                        principalTable: "Playlists",
                        principalColumn: "YoutubeId"
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Channels_UserId",
                table: "Channels",
                column: "UserId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_UserId",
                table: "Playlists",
                column: "UserId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Videos_ChannelYoutubeId",
                table: "Videos",
                column: "ChannelYoutubeId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Videos_PlaylistYoutubeId",
                table: "Videos",
                column: "PlaylistYoutubeId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Videos");

            migrationBuilder.DropTable(name: "Channels");

            migrationBuilder.DropTable(name: "Playlists");

            migrationBuilder.DropTable(name: "Users");

            migrationBuilder.CreateTable(
                name: "UserVideos",
                columns: table =>
                    new
                    {
                        DataId = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        ChannelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        Genre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                        UserId = table.Column<int>(type: "int", nullable: false),
                        VideoId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        WatchTime = table.Column<double>(type: "float", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVideos", x => x.DataId);
                }
            );
        }
    }
}
