using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaTrackerYoutubeService.Migrations
{
    /// <inheritdoc />
    public partial class AddUserVideoToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Platforms");

            migrationBuilder.CreateTable(
                name: "UserVideos",
                columns: table =>
                    new
                    {
                        DataId = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        UserId = table.Column<int>(type: "int", nullable: false),
                        VideoId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        ChannelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        WatchTime = table.Column<double>(type: "float", nullable: false),
                        Genre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVideos", x => x.DataId);
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "UserVideos");

            migrationBuilder.CreateTable(
                name: "Platforms",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        LicenseKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platforms", x => x.Id);
                }
            );
        }
    }
}
