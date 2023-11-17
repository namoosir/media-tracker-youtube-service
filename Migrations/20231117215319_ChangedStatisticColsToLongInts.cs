using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaTrackerYoutubeService.Migrations
{
    /// <inheritdoc />
    public partial class ChangedStatisticColsToLongInts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ViewCount",
                table: "Videos",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<long>(
                name: "LikeCount",
                table: "Videos",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<long>(
                name: "CommentCount",
                table: "Videos",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<long>(
                name: "ViewCount",
                table: "Channels",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<long>(
                name: "VideoCount",
                table: "Channels",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<long>(
                name: "SubscriberCount",
                table: "Channels",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ViewCount",
                table: "Videos",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<int>(
                name: "LikeCount",
                table: "Videos",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<int>(
                name: "CommentCount",
                table: "Videos",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<int>(
                name: "ViewCount",
                table: "Channels",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<int>(
                name: "VideoCount",
                table: "Channels",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<int>(
                name: "SubscriberCount",
                table: "Channels",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true
            );
        }
    }
}
