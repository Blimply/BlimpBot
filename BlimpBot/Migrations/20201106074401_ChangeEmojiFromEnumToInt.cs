using Microsoft.EntityFrameworkCore.Migrations;

namespace BlimpBot.Migrations
{
    public partial class ChangeEmojiFromEnumToInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmojiType",
                table: "Reviews");

            migrationBuilder.AddColumn<int>(
                name: "Emoji",
                table: "Reviews",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Emoji",
                table: "Reviews");

            migrationBuilder.AddColumn<int>(
                name: "EmojiType",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
