using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace myideapool.Migrations
{
    public partial class AddIdea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ideas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(maxLength: 255, nullable: false),
                    Impact = table.Column<int>(nullable: false),
                    Ease = table.Column<int>(nullable: false),
                    Confidence = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ideas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ideas_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ideas_UserId",
                table: "Ideas",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ideas");
        }
    }
}
