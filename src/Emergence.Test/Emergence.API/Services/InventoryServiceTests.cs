using System.Linq;
using System.Threading.Tasks;
using Emergence.API.Services;
using Emergence.Data;
using Emergence.Data.Shared.Stores;
using Emergence.Test.Mocks;
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
            _mockInventoryRepository = RepositoryMocks.GetStandardMockInventoryRepository();
            _mockInventoryItemRepository = RepositoryMocks.GetStandardMockInventoryItemRepository();
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
    }
}
