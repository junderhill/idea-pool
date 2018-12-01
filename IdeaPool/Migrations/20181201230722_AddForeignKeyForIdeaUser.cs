using Microsoft.EntityFrameworkCore.Migrations;

namespace myideapool.Migrations
{
    public partial class AddForeignKeyForIdeaUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Ideas",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Ideas",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255);
        }
    }
}
