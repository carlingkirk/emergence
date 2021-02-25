using Microsoft.EntityFrameworkCore.Migrations;

namespace Emergence.Data.Migrations
{
    public partial class ConvservationStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConservationStatus",
                table: "PlantLocations",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConservationStatus",
                table: "PlantLocations");
        }
    }
}
