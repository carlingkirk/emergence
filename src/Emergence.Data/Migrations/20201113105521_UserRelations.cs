using Microsoft.EntityFrameworkCore.Migrations;

namespace Emergence.Data.Migrations
{
    public partial class UserRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserContacts_UserId",
                table: "UserContacts");

            migrationBuilder.DropIndex(
                name: "IX_UserContactRequests_UserId",
                table: "UserContactRequests");

            migrationBuilder.CreateIndex(
                name: "IX_UserContacts_UserId_ContactUserId",
                table: "UserContacts",
                columns: new[] { "UserId", "ContactUserId" },
                unique: true,
                filter: "[ContactUserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserContactRequests_UserId_ContactUserId",
                table: "UserContactRequests",
                columns: new[] { "UserId", "ContactUserId" },
                unique: true,
                filter: "[ContactUserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserContacts_UserId_ContactUserId",
                table: "UserContacts");

            migrationBuilder.DropIndex(
                name: "IX_UserContactRequests_UserId_ContactUserId",
                table: "UserContactRequests");

            migrationBuilder.CreateIndex(
                name: "IX_UserContacts_UserId",
                table: "UserContacts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContactRequests_UserId",
                table: "UserContactRequests",
                column: "UserId");
        }
    }
}
