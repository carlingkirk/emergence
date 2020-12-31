using Microsoft.EntityFrameworkCore.Migrations;

namespace Emergence.Data.Migrations
{
    public partial class PlantSynonym : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlantSynonyms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LifeformId = table.Column<int>(type: "int", nullable: false),
                    Synonym = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantSynonyms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlantSynonyms_Lifeforms_LifeformId",
                        column: x => x.LifeformId,
                        principalTable: "Lifeforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlantSynonyms_LifeformId",
                table: "PlantSynonyms",
                column: "LifeformId");

            migrationBuilder.Sql("INSERT INTO [dbo].[PlantSynonyms] ([LifeformId],[Synonym]) " +
                                 "SELECT l.id, s.Name " +
                                 "FROM [Synonyms] s " +
                                 "left join Taxons t on t.Id = s.TaxonId " +
                                 "left join Lifeforms l on l.ScientificName = t.Genus + ' ' + t.Species " + 
                                 "where s.Rank = 'Species' and s.Name = l.CommonName ");
        }

        protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(name: "PlantSynonyms");
    }
}
