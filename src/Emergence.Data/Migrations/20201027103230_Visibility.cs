using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Emergence.Data.Migrations
{
    public partial class Visibility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_UserId",
                table: "Users");

            migrationBuilder.RenameColumn(
                            name: "DateOccured",
                            table: "Activities",
                            newName: "DateOccurred");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Users",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "ActivityVisibility",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InventoryItemVisibility",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OriginVisibility",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlantInfoVisibility",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProfileVisibility",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PlantInfos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Visibility",
                table: "PlantInfos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Origins",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Visibility",
                table: "Origins",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "InventoryItems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Visibility",
                table: "InventoryItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Activities",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Visibility",
                table: "Activities",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserContacts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    ContactUserId = table.Column<int>(nullable: true),
                    DateRequested = table.Column<DateTime>(nullable: false),
                    DateAccepted = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserContacts_Users_ContactUserId",
                        column: x => x.ContactUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserContacts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId",
                table: "Users",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PlantInfos_UserId",
                table: "PlantInfos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Origins_UserId",
                table: "Origins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_UserId",
                table: "InventoryItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_UserId",
                table: "Activities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContacts_ContactUserId",
                table: "UserContacts",
                column: "ContactUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContacts_UserId",
                table: "UserContacts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Users_UserId",
                table: "Activities",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Users_UserId",
                table: "InventoryItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Origins_Users_UserId",
                table: "Origins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlantInfos_Users_UserId",
                table: "PlantInfos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql("update Activities set UserId = u.Id from Activities a join Users u on u.UserId = a.CreatedBy");
            migrationBuilder.Sql("update InventoryItems set UserId = u.Id from InventoryItems i  join Users u on u.UserId = i.CreatedBy");
            migrationBuilder.Sql("update Origins set UserId = u.Id from Origins o  join Users u on u.UserId = o.CreatedBy");
            migrationBuilder.Sql("update PlantInfos set UserId = u.Id from PlantInfos p  join Users u on u.UserId = p.CreatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Users_UserId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Users_UserId",
                table: "InventoryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Origins_Users_UserId",
                table: "Origins");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantInfos_Users_UserId",
                table: "PlantInfos");

            migrationBuilder.DropTable(
                name: "UserContacts");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_PlantInfos_UserId",
                table: "PlantInfos");

            migrationBuilder.DropIndex(
                name: "IX_Origins_UserId",
                table: "Origins");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItems_UserId",
                table: "InventoryItems");

            migrationBuilder.DropIndex(
                name: "IX_Activities_UserId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ActivityVisibility",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "InventoryItemVisibility",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OriginVisibility",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PlantInfoVisibility",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProfileVisibility",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PlantInfos");

            migrationBuilder.DropColumn(
                name: "Visibility",
                table: "PlantInfos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Origins");

            migrationBuilder.DropColumn(
                name: "Visibility",
                table: "Origins");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "Visibility",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "DateOccurred",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Visibility",
                table: "Activities");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOccured",
                table: "Activities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId",
                table: "Users",
                column: "UserId",
                unique: true);
        }
    }
}
