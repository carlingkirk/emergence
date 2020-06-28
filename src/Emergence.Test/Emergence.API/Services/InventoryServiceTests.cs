using System.Threading.Tasks;
using Emergence.API.Services;
using Emergence.Data;
using Emergence.Data.Shared.Stores;
using Models = Emergence.Data.Shared.Models;
using FluentAssertions;
using Moq;
using System.Linq;
using Xunit;
using System;
using System.Linq.Expressions;

namespace Emergence.Test.API.Services
{
    public class InventoryServiceTests
    {
        [Fact]
        public async Task TestGet()
        {
            var mockInventoryRepo = new Mock<IRepository<Inventory>>();
            var mockInventoryItemRepo = new Mock<IRepository<InventoryItem>>();

            mockInventoryRepo.Setup(i => i.GetAsync(It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(Data.Fakes.Stores.FakeInventories.Get().First());
            mockInventoryItemRepo.Setup(i => i.GetSomeAsync(It.IsAny<Expression<Func<InventoryItem, bool>>>(), It.IsAny<bool>()))
                .Returns(Data.Fakes.Stores.FakeInventories.GetItems().ToAsyncEnumerable());

            var inventoryService = new InventoryService(mockInventoryRepo.Object, mockInventoryItemRepo.Object);
            var inventory = await inventoryService.GetInventoryAsync(0);

            inventory.Should().NotBeNull("it exists");
            inventory.Items.Should().HaveCount(3);
            inventory.Items.Where(i => i.Status == Models.Status.Available).Should().HaveCount(2);
            inventory.Items.Where(i => i.Status == Models.Status.Wishlist).Should().HaveCount(1);
            inventory.Items.Where(i => i.ItemType == Models.ItemType.Supply).Should().HaveCount(1);
        }
    }
}
