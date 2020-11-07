using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Emergence.Data.Migrations
{
    public partial class ContactRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserContacts_Users_UserId",
                table: "UserContacts");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserContacts",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "UserContactRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    ContactUserId = table.Column<int>(nullable: true),
                    DateRequested = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContactRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserContactRequests_Users_ContactUserId",
                        column: x => x.ContactUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserContactRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserContactRequests_ContactUserId",
                table: "UserContactRequests",
                column: "ContactUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContactRequests_UserId",
                table: "UserContactRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserContacts_Users_UserId",
                table: "UserContacts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserContacts_Users_UserId",
                table: "UserContacts");

            migrationBuilder.DropTable(
                name: "UserContactRequests");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserContacts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_UserContacts_Users_UserId",
                table: "UserContacts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
