﻿// <auto-generated />
using System;
using Emergence.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Emergence.Data.Migrations
{
    [DbContext(typeof(EmergenceDbContext))]
    [Migration("20200906124028_PlantLocations")]
    partial class PlantLocations
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Emergence.Data.Shared.Stores.Activity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ActivityType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssignedTo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomActivityType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateOccured")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateScheduled")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.Property<int?>("SpecimenId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SpecimenId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.Inventory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Inventories");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.InventoryItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateAcquired")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<int>("InventoryId")
                        .HasColumnType("int");

                    b.Property<string>("ItemType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("OriginId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("InventoryId");

                    b.HasIndex("OriginId");

                    b.ToTable("InventoryItems");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.Lifeform", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CommonName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ScientificName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Lifeforms");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddressLine1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AddressLine2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Altitude")
                        .HasColumnType("float");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<double?>("Latitude")
                        .HasColumnType("float");

                    b.Property<double?>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Region")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StateOrProvince")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.Origin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AltExternalId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Authors")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExternalId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Latitude")
                        .HasColumnType("float");

                    b.Property<int?>("LocationId")
                        .HasColumnType("int");

                    b.Property<double?>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParentOriginId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Uri")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("ParentOriginId");

                    b.ToTable("Origins");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BlobPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateTaken")
                        .HasColumnType("datetime2");

                    b.Property<string>("Filename")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Height")
                        .HasColumnType("int");

                    b.Property<int?>("LocationId")
                        .HasColumnType("int");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TypeId")
                        .HasColumnType("int");

                    b.Property<int?>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.PlantInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CommonName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("HeightUnit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LifeformId")
                        .HasColumnType("int");

                    b.Property<short?>("MaximumBloomTime")
                        .HasColumnType("smallint");

                    b.Property<double?>("MaximumHeight")
                        .HasColumnType("float");

                    b.Property<string>("MaximumLight")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("MaximumSpread")
                        .HasColumnType("float");

                    b.Property<string>("MaximumWater")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaximumZone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<short?>("MinimumBloomTime")
                        .HasColumnType("smallint");

                    b.Property<double?>("MinimumHeight")
                        .HasColumnType("float");

                    b.Property<string>("MinimumLight")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("MinimumSpread")
                        .HasColumnType("float");

                    b.Property<string>("MinimumWater")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MinimumZone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("OriginId")
                        .HasColumnType("int");

                    b.Property<bool?>("Preferred")
                        .HasColumnType("bit");

                    b.Property<string>("ScientificName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SpreadUnit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StratificationStages")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TaxonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LifeformId");

                    b.HasIndex("OriginId");

                    b.HasIndex("TaxonId");

                    b.ToTable("PlantInfos");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.PlantLocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PlantInfoId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("PlantInfoId");

                    b.ToTable("PlantLocations");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.Specimen", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<int>("InventoryItemId")
                        .HasColumnType("int");

                    b.Property<int?>("LifeformId")
                        .HasColumnType("int");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PlantInfoId")
                        .HasColumnType("int");

                    b.Property<string>("SpecimenStage")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("InventoryItemId");

                    b.HasIndex("LifeformId");

                    b.HasIndex("PlantInfoId");

                    b.ToTable("Specimens");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.Taxon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Class")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Epifamily")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Family")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Form")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Genus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GenusHybrid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Hybrid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Infraclass")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Infraorder")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Kingdom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Order")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phylum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Section")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Species")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subclass")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subfamily")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subgenus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Suborder")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subphylum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subspecies")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subtribe")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subvariety")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Superclass")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Superfamily")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Superorder")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Supertribe")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tribe")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Variety")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Taxons");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.Activity", b =>
                {
                    b.HasOne("Emergence.Data.Shared.Stores.Specimen", "Specimen")
                        .WithMany()
                        .HasForeignKey("SpecimenId");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.InventoryItem", b =>
                {
                    b.HasOne("Emergence.Data.Shared.Stores.Inventory", "Inventory")
                        .WithMany()
                        .HasForeignKey("InventoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Emergence.Data.Shared.Stores.Origin", "Origin")
                        .WithMany()
                        .HasForeignKey("OriginId");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.Origin", b =>
                {
                    b.HasOne("Emergence.Data.Shared.Stores.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.HasOne("Emergence.Data.Shared.Stores.Origin", "ParentOrigin")
                        .WithMany()
                        .HasForeignKey("ParentOriginId");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.Photo", b =>
                {
                    b.HasOne("Emergence.Data.Shared.Stores.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.PlantInfo", b =>
                {
                    b.HasOne("Emergence.Data.Shared.Stores.Lifeform", "Lifeform")
                        .WithMany()
                        .HasForeignKey("LifeformId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Emergence.Data.Shared.Stores.Origin", "Origin")
                        .WithMany()
                        .HasForeignKey("OriginId");

                    b.HasOne("Emergence.Data.Shared.Stores.Taxon", "Taxon")
                        .WithMany()
                        .HasForeignKey("TaxonId");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.PlantLocation", b =>
                {
                    b.HasOne("Emergence.Data.Shared.Stores.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Emergence.Data.Shared.Stores.PlantInfo", "PlantInfo")
                        .WithMany()
                        .HasForeignKey("PlantInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.Specimen", b =>
                {
                    b.HasOne("Emergence.Data.Shared.Stores.InventoryItem", "InventoryItem")
                        .WithMany()
                        .HasForeignKey("InventoryItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Emergence.Data.Shared.Stores.Lifeform", "Lifeform")
                        .WithMany()
                        .HasForeignKey("LifeformId");

                    b.HasOne("Emergence.Data.Shared.Stores.PlantInfo", "PlantInfo")
                        .WithMany()
                        .HasForeignKey("PlantInfoId");
                });
#pragma warning restore 612, 618
        }
    }
}
