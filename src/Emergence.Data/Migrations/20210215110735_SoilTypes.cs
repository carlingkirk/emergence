using Microsoft.EntityFrameworkCore.Migrations;

namespace Emergence.Data.Migrations
{
    public partial class SoilTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) => _ = migrationBuilder.AddColumn<string>(
                name: "SoilTypes",
                table: "PlantInfos",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

        protected override void Down(MigrationBuilder migrationBuilder) => _ = migrationBuilder.DropColumn(
                name: "SoilTypes",
                table: "PlantInfos");
    }
}
