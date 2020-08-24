using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Emergence.Data.Migrations
{
    public partial class Incremental : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaxonsCopy",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Kingdom = table.Column<string>(nullable: true),
                    Phylum = table.Column<string>(nullable: true),
                    Subphylum = table.Column<string>(nullable: true),
                    Superclass = table.Column<string>(nullable: true),
                    Class = table.Column<string>(nullable: true),
                    Subclass = table.Column<string>(nullable: true),
                    Infraclass = table.Column<string>(nullable: true),
                    Superorder = table.Column<string>(nullable: true),
                    Order = table.Column<string>(nullable: true),
                    Suborder = table.Column<string>(nullable: true),
                    Infraorder = table.Column<string>(nullable: true),
                    Epifamily = table.Column<string>(nullable: true),
                    Superfamily = table.Column<string>(nullable: true),
                    Family = table.Column<string>(nullable: true),
                    Subfamily = table.Column<string>(nullable: true),
                    Supertribe = table.Column<string>(nullable: true),
                    Tribe = table.Column<string>(nullable: true),
                    Subtribe = table.Column<string>(nullable: true),
                    GenusHybrid = table.Column<string>(nullable: true),
                    Genus = table.Column<string>(nullable: true),
                    Subgenus = table.Column<string>(nullable: true),
                    Hybrid = table.Column<string>(nullable: true),
                    Species = table.Column<string>(nullable: true),
                    Subspecies = table.Column<string>(nullable: true),
                    Variety = table.Column<string>(nullable: true),
                    Subvariety = table.Column<string>(nullable: true),
                    Form = table.Column<string>(nullable: true),
                    Section = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxons", x => x.Id);
                });

            migrationBuilder.Sql("insert into TaxonsCopy (Id,Kingdom,Phylum,Subphylum,Superclass,Class,Subclass,Infraclass,Superorder,[Order],Suborder," +
                "Infraorder,Epifamily,Superfamily,Family,Subfamily,Supertribe,Tribe,Subtribe,GenusHybrid,Genus,Subgenus,Hybrid,Species,Subspecies,Variety," +
                "Form,Section,DateCreated,DateModified) select Id,Kingdom,Phylum,Subphylum,Superclass,Class,Subclass,Infraclass,Superorder,[Order],Suborder," +
                "Infraorder,Epifamily,Superfamily,Family,Subfamily,Supertribe,Tribe,Subtribe,GenusHybrid,Genus,Subgenus,Hybrid,Species,Subspecies,Variety," +
                "Form,Section,DateCreated,DateModified from Taxons;");

            migrationBuilder.DropTable("Taxons");

            migrationBuilder.RenameTable("TaxonsCopy", newName: "Taxons");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subvariety",
                table: "Taxons");
        }
    }
}
