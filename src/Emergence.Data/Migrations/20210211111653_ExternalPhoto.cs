using Microsoft.EntityFrameworkCore.Migrations;

namespace Emergence.Data.Migrations
{
    public partial class ExternalPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AddColumn<string>(
                name: "ExternalUrl",
                table: "Photos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

        protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropColumn(
                name: "ExternalUrl",
                table: "Photos");
    }
}
