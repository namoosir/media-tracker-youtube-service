using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaTrackerYoutubeService.Migrations
{
    /// <inheritdoc />
    public partial class AddedSubscriptionsEtagForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubscriptionsEtag",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: ""
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "SubscriptionsEtag", table: "Users");
        }
    }
}
