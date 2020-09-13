using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Emergence.Data.Migrations
{
    public partial class Synonyms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PlantLocations_LocationId",
                table: "PlantLocations");

            migrationBuilder.AddColumn<string>(
                name: "Infrakingdom",
                table: "Taxons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subkingdom",
                table: "Taxons",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Synonyms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaxonId = table.Column<int>(nullable: false),
                    OriginId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Rank = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Synonyms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Synonyms_Origins_OriginId",
                        column: x => x.OriginId,
                        principalTable: "Origins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Synonyms_Taxons_TaxonId",
                        column: x => x.TaxonId,
                        principalTable: "Taxons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlantLocations_LocationId_PlantInfoId",
                table: "PlantLocations",
                columns: new[] { "LocationId", "PlantInfoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Synonyms_OriginId",
                table: "Synonyms",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_Synonyms_TaxonId",
                table: "Synonyms",
                column: "TaxonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Synonyms");

            migrationBuilder.DropIndex(
                name: "IX_PlantLocations_LocationId_PlantInfoId",
                table: "PlantLocations");

            migrationBuilder.DropColumn(
                name: "Infrakingdom",
                table: "Taxons");

            migrationBuilder.DropColumn(
                name: "Subkingdom",
                table: "Taxons");

            migrationBuilder.CreateIndex(
                name: "IX_PlantLocations_LocationId",
                table: "PlantLocations",
                column: "LocationId");
        }
    }
}
