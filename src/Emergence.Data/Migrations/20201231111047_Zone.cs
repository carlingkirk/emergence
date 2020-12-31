using Microsoft.EntityFrameworkCore.Migrations;

namespace Emergence.Data.Migrations
{
    public partial class Zone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaximumZone",
                table: "PlantInfos");

            migrationBuilder.DropColumn(
                name: "MinimumZone",
                table: "PlantInfos");

            migrationBuilder.AddColumn<int>(
                name: "MaximumZoneId",
                table: "PlantInfos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinimumZoneId",
                table: "PlantInfos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlantInfos_MaximumZoneId",
                table: "PlantInfos",
                column: "MaximumZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantInfos_MinimumZoneId",
                table: "PlantInfos",
                column: "MinimumZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantInfos_Zones_MaximumZoneId",
                table: "PlantInfos",
                column: "MaximumZoneId",
                principalTable: "Zones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlantInfos_Zones_MinimumZoneId",
                table: "PlantInfos",
                column: "MinimumZoneId",
                principalTable: "Zones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql("insert into Zones ([Name]) values ('1'),('1a'),('1b'),('2'),('2a'),('2b'),('3'),('3a'),('3b'),('4'),('4a'),('4b'),('5'),('5a'),('5b')" +
                ",('6'),('6a'),('6b'),('7'),('7a'),('7b'),('8'),('8a'),('8b'),('9'),('9a'),('9b'),('10'),('10a'),('10b'),('11'),('11a'),('11b'),('12'),('12a'),('12b'),('13'),('13a'),('13b');");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantInfos_Zones_MaximumZoneId",
                table: "PlantInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantInfos_Zones_MinimumZoneId",
                table: "PlantInfos");

            migrationBuilder.DropTable(
                name: "Zones");

            migrationBuilder.DropIndex(
                name: "IX_PlantInfos_MaximumZoneId",
                table: "PlantInfos");

            migrationBuilder.DropIndex(
                name: "IX_PlantInfos_MinimumZoneId",
                table: "PlantInfos");

            migrationBuilder.DropColumn(
                name: "MaximumZoneId",
                table: "PlantInfos");

            migrationBuilder.DropColumn(
                name: "MinimumZoneId",
                table: "PlantInfos");

            migrationBuilder.AddColumn<string>(
                name: "MaximumZone",
                table: "PlantInfos",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MinimumZone",
                table: "PlantInfos",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true);
        }
    }
}
