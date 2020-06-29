using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.API.Services;
using Emergence.Data;
using Emergence.Data.Shared.Stores;
using FluentAssertions;
using Moq;
using Xunit;
using Models = Emergence.Data.Shared.Models;

namespace Emergence.Test.API.Services
{
    public class InventoryServiceTests
    {
        private readonly Mock<IRepository<Inventory>> _mockInventoryRepository;
        private readonly Mock<IRepository<InventoryItem>> _mockInventoryItemRepository;

        public InventoryServiceTests()
        {
            _mockInventoryRepository = GetStandardMockInventoryRepository();
            _mockInventoryItemRepository = GetStandardMockInventoryItemRepository();
        }

        [Fact]
        public async Task TestGet()
        {
            var inventoryService = new InventoryService(_mockInventoryRepository.Object, _mockInventoryItemRepository.Object);
            var inventory = await inventoryService.GetInventoryAsync(0);

            inventory.Should().NotBeNull("it exists");
            inventory.Items.Should().HaveCount(3);
            inventory.Items.Where(i => i.Status == Models.Status.Available).Should().HaveCount(2);
            inventory.Items.Where(i => i.Status == Models.Status.Wishlist).Should().HaveCount(1);
            inventory.Items.Where(i => i.ItemType == Models.ItemType.Supply).Should().HaveCount(1);
        }

        private Mock<IRepository<Inventory>> GetStandardMockInventoryRepository()
        {
            var mockInventoryRepo = new Mock<IRepository<Inventory>>();

            mockInventoryRepo.Setup(i => i.GetAsync(It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(Data.Fakes.Stores.FakeInventories.Get().First());

            return mockInventoryRepo;
        }

        private Mock<IRepository<InventoryItem>> GetStandardMockInventoryItemRepository()
        {
            var mockInventoryItemRepo = new Mock<IRepository<InventoryItem>>();

            mockInventoryItemRepo.Setup(i => i.GetSomeAsync(It.IsAny<Expression<Func<InventoryItem, bool>>>(), It.IsAny<bool>()))
                .Returns(Data.Fakes.Stores.FakeInventories.GetItems().ToAsyncEnumerable());

            return mockInventoryItemRepo;
        }
    }
}
