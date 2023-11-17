using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaTrackerYoutubeService.Migrations
{
    /// <inheritdoc />
    public partial class AddedMoreBidirectionalReferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Channels_Users_UserId", table: "Channels");

            migrationBuilder.DropIndex(name: "IX_Channels_UserId", table: "Channels");

            migrationBuilder.DropColumn(name: "UserId", table: "Channels");

            migrationBuilder.CreateTable(
                name: "ChannelUser",
                columns: table =>
                    new
                    {
                        SubscribedChannelsYoutubeId = table.Column<string>(
                            type: "nvarchar(450)",
                            nullable: false
                        ),
                        UserSubscribersUserId = table.Column<int>(type: "int", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_ChannelUser",
                        x => new { x.SubscribedChannelsYoutubeId, x.UserSubscribersUserId }
                    );
                    table.ForeignKey(
                        name: "FK_ChannelUser_Channels_SubscribedChannelsYoutubeId",
                        column: x => x.SubscribedChannelsYoutubeId,
                        principalTable: "Channels",
                        principalColumn: "YoutubeId",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_ChannelUser_Users_UserSubscribersUserId",
                        column: x => x.UserSubscribersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_ChannelUser_UserSubscribersUserId",
                table: "ChannelUser",
                column: "UserSubscribersUserId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ChannelUser");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Channels",
                type: "int",
                nullable: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Channels_UserId",
                table: "Channels",
                column: "UserId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_Users_UserId",
                table: "Channels",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId"
            );
        }
    }
}
