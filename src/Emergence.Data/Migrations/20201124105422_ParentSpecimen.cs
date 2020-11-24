using Microsoft.EntityFrameworkCore.Migrations;

namespace Emergence.Data.Migrations
{
    public partial class ParentSpecimen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentSpecimenId",
                table: "Specimens",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specimens_ParentSpecimenId",
                table: "Specimens",
                column: "ParentSpecimenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Specimens_Specimens_ParentSpecimenId",
                table: "Specimens",
                column: "ParentSpecimenId",
                principalTable: "Specimens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Specimens_Specimens_ParentSpecimenId",
                table: "Specimens");

            migrationBuilder.DropIndex(
                name: "IX_Specimens_ParentSpecimenId",
                table: "Specimens");

            migrationBuilder.DropColumn(
                name: "ParentSpecimenId",
                table: "Specimens");
        }
    }
}
