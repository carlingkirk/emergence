using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Emergence.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ActivityType = table.Column<string>(nullable: true),
                    SpecimenId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InventoryId = table.Column<int>(nullable: false),
                    OriginId = table.Column<int>(nullable: true),
                    ItemType = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    DateAcquired = table.Column<DateTime>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lifeforms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ScientificName = table.Column<string>(nullable: true),
                    CommonName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lifeforms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    StateOrProvince = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Longitude = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Origins",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParentId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Uri = table.Column<string>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    Latitude = table.Column<double>(nullable: true),
                    Authors = table.Column<string>(nullable: true),
                    ExternalId = table.Column<string>(nullable: true),
                    AltExternalId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Origins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlantInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LifeformId = table.Column<int>(nullable: false),
                    OriginId = table.Column<int>(nullable: true),
                    TaxonId = table.Column<int>(nullable: true),
                    ScientificName = table.Column<string>(nullable: true),
                    CommonName = table.Column<string>(nullable: true),
                    Preferred = table.Column<bool>(nullable: true),
                    MinimumBloomTime = table.Column<short>(nullable: true),
                    MaximumBloomTime = table.Column<short>(nullable: true),
                    MinimumHeight = table.Column<double>(nullable: true),
                    MaximumHeight = table.Column<double>(nullable: true),
                    HeightUnit = table.Column<string>(nullable: true),
                    MinimumSpread = table.Column<double>(nullable: true),
                    MaximumSpread = table.Column<double>(nullable: true),
                    SpreadUnit = table.Column<string>(nullable: true),
                    MinimumWater = table.Column<string>(nullable: true),
                    MaximumWater = table.Column<string>(nullable: true),
                    MinimumLight = table.Column<string>(nullable: true),
                    MaximumLight = table.Column<string>(nullable: true),
                    StratificationStages = table.Column<string>(nullable: true),
                    MinimumZone = table.Column<string>(nullable: true),
                    MaximumZone = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specimens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InventoryItemId = table.Column<int>(nullable: false),
                    LifeformId = table.Column<int>(nullable: true),
                    SpecimenStage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specimens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Taxons",
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
                    Form = table.Column<string>(nullable: true),
                    Section = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxons", x => x.Id);
                });

            MigrationSeeds.AddEmergenceSeedData(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.DropTable(
                name: "Lifeforms");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Origins");

            migrationBuilder.DropTable(
                name: "PlantInfos");

            migrationBuilder.DropTable(
                name: "Specimens");

            migrationBuilder.DropTable(
                name: "Taxons");
        }
    }
}
