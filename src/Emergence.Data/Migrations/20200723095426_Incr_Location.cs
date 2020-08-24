using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Emergence.Data.Migrations
{
    public partial class Incr_Location : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OriginsCopy",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParentOriginId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Uri = table.Column<string>(nullable: true),
                    LocationId = table.Column<int>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    Latitude = table.Column<double>(nullable: true),
                    Authors = table.Column<string>(nullable: true),
                    ExternalId = table.Column<string>(nullable: true),
                    AltExternalId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Origins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Origins_Origins_ParentOriginId",
                        column: x => x.ParentOriginId,
                        principalTable: "Origins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Origins_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.Sql("insert into OriginsCopy (Id,ParentOriginId,Name,Type,Description,Uri,Longitude,Latitude,Authors,ExternalId,AltExternalId," +
                "UserId,DateCreated,DateModified) " + "select Id,ParentId,Name,Type,Description,Uri,Longitude,Latitude,Authors,ExternalId,AltExternalId," +
                "UserId,DateCreated,DateModified from Origins;");

            migrationBuilder.Sql("PRAGMA foreign_keys=\"0\"", true);

            migrationBuilder.DropTable("Origins");

            migrationBuilder.RenameTable("OriginsCopy", newName: "Origins");

            migrationBuilder.Sql("PRAGMA foreign_keys=\"1\"", true);

            migrationBuilder.CreateIndex(
                name: "IX_Origins_LocationId",
                table: "Origins",
                column: "LocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Origins_OriginId",
                table: "InventoryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Origins_Locations_LocationId",
                table: "Origins");

            migrationBuilder.DropIndex(
                name: "IX_Origins_LocationId",
                table: "Origins");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItems_OriginId",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Origins");
        }
    }
}
