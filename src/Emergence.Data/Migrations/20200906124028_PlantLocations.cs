using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Emergence.Data.Migrations
{
    public partial class PlantLocations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Locations",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PlantLocations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantInfoId = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlantLocations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlantLocations_PlantInfos_PlantInfoId",
                        column: x => x.PlantInfoId,
                        principalTable: "PlantInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlantLocations_LocationId",
                table: "PlantLocations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantLocations_PlantInfoId",
                table: "PlantLocations",
                column: "PlantInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlantLocations");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Locations");
        }
    }
}
