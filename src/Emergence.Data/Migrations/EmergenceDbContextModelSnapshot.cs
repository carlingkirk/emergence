﻿// <auto-generated />
using System;
using Emergence.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Emergence.Data.Migrations
{
    [DbContext(typeof(EmergenceDbContext))]
    partial class EmergenceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6");

            modelBuilder.Entity("Emergence.Data.Shared.Stores.Activity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ActivityType")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateOccured")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateScheduled")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int?>("SpecimenId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SpecimenId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.Inventory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Inventories");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.InventoryItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DateAcquired")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<int>("InventoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ItemType")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int?>("OriginId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Status")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("InventoryId");

                    b.HasIndex("OriginId");

                    b.ToTable("InventoryItems");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.Lifeform", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CommonName")
                        .HasColumnType("TEXT");

                    b.Property<string>("ScientificName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Lifeforms");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AddressLine1")
                        .HasColumnType("TEXT");

                    b.Property<string>("AddressLine2")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Altitude")
                        .HasColumnType("REAL");

                    b.Property<string>("City")
                        .HasColumnType("TEXT");

                    b.Property<string>("Country")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Latitude")
                        .HasColumnType("REAL");

                    b.Property<double?>("Longitude")
                        .HasColumnType("REAL");

                    b.Property<string>("PostalCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("StateOrProvince")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.Origin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AltExternalId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Authors")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExternalId")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Latitude")
                        .HasColumnType("REAL");

                    b.Property<int?>("LocationId")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("Longitude")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ParentOriginId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.Property<string>("Uri")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("ParentOriginId");

                    b.ToTable("Origins");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ContentType")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateTaken")
                        .HasColumnType("TEXT");

                    b.Property<string>("Filename")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Height")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("LocationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.Property<int?>("TypeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Width")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.PlantInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CommonName")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("HeightUnit")
                        .HasColumnType("TEXT");

                    b.Property<int>("LifeformId")
                        .HasColumnType("INTEGER");

                    b.Property<short?>("MaximumBloomTime")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("MaximumHeight")
                        .HasColumnType("REAL");

                    b.Property<string>("MaximumLight")
                        .HasColumnType("TEXT");

                    b.Property<double?>("MaximumSpread")
                        .HasColumnType("REAL");

                    b.Property<string>("MaximumWater")
                        .HasColumnType("TEXT");

                    b.Property<string>("MaximumZone")
                        .HasColumnType("TEXT");

                    b.Property<short?>("MinimumBloomTime")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("MinimumHeight")
                        .HasColumnType("REAL");

                    b.Property<string>("MinimumLight")
                        .HasColumnType("TEXT");

                    b.Property<double?>("MinimumSpread")
                        .HasColumnType("REAL");

                    b.Property<string>("MinimumWater")
                        .HasColumnType("TEXT");

                    b.Property<string>("MinimumZone")
                        .HasColumnType("TEXT");

                    b.Property<int?>("OriginId")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("Preferred")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ScientificName")
                        .HasColumnType("TEXT");

                    b.Property<string>("SpreadUnit")
                        .HasColumnType("TEXT");

                    b.Property<string>("StratificationStages")
                        .HasColumnType("TEXT");

                    b.Property<int?>("TaxonId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LifeformId");

                    b.HasIndex("OriginId");

                    b.HasIndex("TaxonId");

                    b.ToTable("PlantInfos");
                });

            modelBuilder.Entity("Emergence.Data.Shared.Stores.Specimen", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<int>("InventoryItemId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("LifeformId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PlantInfoId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SpecimenStage")
                        .HasColumnType("TEXT");

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
                        .HasColumnType("INTEGER");

                    b.Property<string>("Class")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Epifamily")
                        .HasColumnType("TEXT");

                    b.Property<string>("Family")
                        .HasColumnType("TEXT");

                    b.Property<string>("Form")
                        .HasColumnType("TEXT");

                    b.Property<string>("Genus")
                        .HasColumnType("TEXT");

                    b.Property<string>("GenusHybrid")
                        .HasColumnType("TEXT");

                    b.Property<string>("Hybrid")
                        .HasColumnType("TEXT");

                    b.Property<string>("Infraclass")
                        .HasColumnType("TEXT");

                    b.Property<string>("Infraorder")
                        .HasColumnType("TEXT");

                    b.Property<string>("Kingdom")
                        .HasColumnType("TEXT");

                    b.Property<string>("Order")
                        .HasColumnType("TEXT");

                    b.Property<string>("Phylum")
                        .HasColumnType("TEXT");

                    b.Property<string>("Section")
                        .HasColumnType("TEXT");

                    b.Property<string>("Species")
                        .HasColumnType("TEXT");

                    b.Property<string>("Subclass")
                        .HasColumnType("TEXT");

                    b.Property<string>("Subfamily")
                        .HasColumnType("TEXT");

                    b.Property<string>("Subgenus")
                        .HasColumnType("TEXT");

                    b.Property<string>("Suborder")
                        .HasColumnType("TEXT");

                    b.Property<string>("Subphylum")
                        .HasColumnType("TEXT");

                    b.Property<string>("Subspecies")
                        .HasColumnType("TEXT");

                    b.Property<string>("Subtribe")
                        .HasColumnType("TEXT");

                    b.Property<string>("Subvariety")
                        .HasColumnType("TEXT");

                    b.Property<string>("Superclass")
                        .HasColumnType("TEXT");

                    b.Property<string>("Superfamily")
                        .HasColumnType("TEXT");

                    b.Property<string>("Superorder")
                        .HasColumnType("TEXT");

                    b.Property<string>("Supertribe")
                        .HasColumnType("TEXT");

                    b.Property<string>("Tribe")
                        .HasColumnType("TEXT");

                    b.Property<string>("Variety")
                        .HasColumnType("TEXT");

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
