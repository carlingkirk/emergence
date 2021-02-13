using System;
using System.Linq;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Extensions;
using Emergence.Test.Data.Fakes.Stores;
using FluentAssertions;
using Xunit;

namespace Emergence.Test.Emergence.Data.Shared.Extensions
{
    public class TestSpecimenExtensions
    {
        [Fact]
        public void TestAsModel()
        {
            var specimens = FakeSpecimens.Get();
            var specimenModels = specimens.Select(s => s.AsModel());

            var firstSpecimen = specimenModels.First();
            firstSpecimen.Name.Should().Be("Liatris spicata seeds");
            firstSpecimen.Quantity.Should().Be(25);
            firstSpecimen.SpecimenStage.Should().Be(SpecimenStage.Seed);
            firstSpecimen.Lifeform.ScientificName.Should().Be("Liatris spicata");
            firstSpecimen.DateCreated.Should().NotBeNull();
            firstSpecimen.DateModified.Should().BeAfter(firstSpecimen.DateCreated.Value);
            firstSpecimen.Notes.Contains("feather").Should().BeTrue();
            firstSpecimen.ParentSpecimen.Should().BeNull();
            firstSpecimen.CreatedBy.Should().Be(Helpers.UserId);
            firstSpecimen.ModifiedBy.Should().Be(null);
            firstSpecimen.InventoryItem.Name.Should().Be("Liatris spicata seeds");
            firstSpecimen.InventoryItem.Inventory.InventoryId.Should().Be(1);
            firstSpecimen.InventoryItem.Origin.OriginId.Should().Be(2);
            firstSpecimen.InventoryItem.ItemType.Should().Be(ItemType.Specimen);
            firstSpecimen.InventoryItem.Quantity.Should().Be(25);
            firstSpecimen.InventoryItem.Status.Should().Be(ItemStatus.Available);
            firstSpecimen.InventoryItem.Visibility.Should().Be(Visibility.Public);
            firstSpecimen.InventoryItem.CreatedBy.Should().Be(Helpers.UserId);
            firstSpecimen.InventoryItem.ModifiedBy.Should().Be(null);
            firstSpecimen.InventoryItem.DateAcquired.Should().Be(new DateTime(2020, 03, 31));
            firstSpecimen.InventoryItem.DateCreated.Should().Be(new DateTime(2020, 06, 15));
            firstSpecimen.InventoryItem.DateModified.Should().Be(null);
            firstSpecimen.InventoryItem.User.Id.Should().Be(1);
        }

        [Fact]
        public void TestAsSearchModel()
        {
            var specimens = FakeSpecimens.Get();
            var specimenModels = specimens.Select(s => s.AsSearchModel());

            var firstSpecimen = specimenModels.First();
            firstSpecimen.Name.Should().Be("Liatris spicata seeds");
            firstSpecimen.Quantity.Should().Be(25);
            firstSpecimen.SpecimenStage.Should().Be(SpecimenStage.Seed);
            firstSpecimen.Lifeform.ScientificName.Should().Be("Liatris spicata");
            firstSpecimen.DateCreated.Should().NotBeNull();
            firstSpecimen.DateModified.Should().BeAfter(firstSpecimen.DateCreated.Value);
            firstSpecimen.Notes.Contains("feather").Should().BeTrue();
            firstSpecimen.ParentSpecimen.Should().BeNull();
            firstSpecimen.CreatedBy.Should().Be(Helpers.UserId);
            firstSpecimen.ModifiedBy.Should().Be(null);
            firstSpecimen.InventoryItem.Name.Should().Be("Liatris spicata seeds");
            firstSpecimen.InventoryItem.Inventory.Id.Should().Be(1);
            firstSpecimen.InventoryItem.Origin.Id.Should().Be(2);
            firstSpecimen.InventoryItem.ItemType.Should().Be(ItemType.Specimen);
            firstSpecimen.InventoryItem.Quantity.Should().Be(25);
            firstSpecimen.InventoryItem.Status.Should().Be(ItemStatus.Available);
            firstSpecimen.InventoryItem.Visibility.Should().Be(Visibility.Public);
            firstSpecimen.InventoryItem.CreatedBy.Should().Be(Helpers.UserId);
            firstSpecimen.InventoryItem.ModifiedBy.Should().Be(null);
            firstSpecimen.InventoryItem.DateAcquired.Should().Be(new DateTime(2020, 03, 31));
            firstSpecimen.InventoryItem.DateCreated.Should().Be(new DateTime(2020, 06, 15));
            firstSpecimen.InventoryItem.DateModified.Should().Be(null);
            firstSpecimen.InventoryItem.User.Id.Should().Be(1);
        }
    }
}
