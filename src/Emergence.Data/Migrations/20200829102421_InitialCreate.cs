using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Emergence.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lifeforms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScientificName = table.Column<string>(nullable: true),
                    CommonName = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    StateOrProvince = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    Latitude = table.Column<double>(nullable: true),
                    Altitude = table.Column<double>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Taxons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    Section = table.Column<string>(nullable: true),
                    Subgenus = table.Column<string>(nullable: true),
                    Hybrid = table.Column<string>(nullable: true),
                    Species = table.Column<string>(nullable: true),
                    Subspecies = table.Column<string>(nullable: true),
                    Variety = table.Column<string>(nullable: true),
                    Subvariety = table.Column<string>(nullable: true),
                    Form = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Origins",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentOriginId = table.Column<int>(nullable: true),
                    LocationId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Uri = table.Column<string>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    Latitude = table.Column<double>(nullable: true),
                    Authors = table.Column<string>(nullable: true),
                    ExternalId = table.Column<string>(nullable: true),
                    AltExternalId = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Origins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Origins_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Origins_Origins_ParentOriginId",
                        column: x => x.ParentOriginId,
                        principalTable: "Origins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(nullable: true),
                    TypeId = table.Column<int>(nullable: true),
                    LocationId = table.Column<int>(nullable: true),
                    Filename = table.Column<string>(nullable: true),
                    BlobPath = table.Column<string>(nullable: true),
                    ContentType = table.Column<string>(nullable: true),
                    Width = table.Column<int>(nullable: true),
                    Height = table.Column<int>(nullable: true),
                    DateTaken = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryId = table.Column<int>(nullable: false),
                    OriginId = table.Column<int>(nullable: true),
                    ItemType = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    DateAcquired = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItems_Inventories_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryItems_Origins_OriginId",
                        column: x => x.OriginId,
                        principalTable: "Origins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlantInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlantInfos_Lifeforms_LifeformId",
                        column: x => x.LifeformId,
                        principalTable: "Lifeforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlantInfos_Origins_OriginId",
                        column: x => x.OriginId,
                        principalTable: "Origins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlantInfos_Taxons_TaxonId",
                        column: x => x.TaxonId,
                        principalTable: "Taxons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Specimens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryItemId = table.Column<int>(nullable: false),
                    LifeformId = table.Column<int>(nullable: true),
                    SpecimenStage = table.Column<string>(nullable: true),
                    PlantInfoId = table.Column<int>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specimens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Specimens_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Specimens_Lifeforms_LifeformId",
                        column: x => x.LifeformId,
                        principalTable: "Lifeforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Specimens_PlantInfos_PlantInfoId",
                        column: x => x.PlantInfoId,
                        principalTable: "PlantInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ActivityType = table.Column<string>(nullable: true),
                    CustomActivityType = table.Column<string>(nullable: true),
                    SpecimenId = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    AssignedTo = table.Column<string>(nullable: true),
                    DateScheduled = table.Column<DateTime>(nullable: true),
                    DateOccured = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_Specimens_SpecimenId",
                        column: x => x.SpecimenId,
                        principalTable: "Specimens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_SpecimenId",
                table: "Activities",
                column: "SpecimenId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_InventoryId",
                table: "InventoryItems",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_OriginId",
                table: "InventoryItems",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_Origins_LocationId",
                table: "Origins",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Origins_ParentOriginId",
                table: "Origins",
                column: "ParentOriginId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_LocationId",
                table: "Photos",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantInfos_LifeformId",
                table: "PlantInfos",
                column: "LifeformId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantInfos_OriginId",
                table: "PlantInfos",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantInfos_TaxonId",
                table: "PlantInfos",
                column: "TaxonId");

            migrationBuilder.CreateIndex(
                name: "IX_Specimens_InventoryItemId",
                table: "Specimens",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Specimens_LifeformId",
                table: "Specimens",
                column: "LifeformId");

            migrationBuilder.CreateIndex(
                name: "IX_Specimens_PlantInfoId",
                table: "Specimens",
                column: "PlantInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "Specimens");

            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.DropTable(
                name: "PlantInfos");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "Lifeforms");

            migrationBuilder.DropTable(
                name: "Origins");

            migrationBuilder.DropTable(
                name: "Taxons");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
