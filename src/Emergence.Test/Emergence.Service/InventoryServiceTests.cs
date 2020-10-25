using System.Linq;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Stores;
using Emergence.Service;
using Emergence.Service.Interfaces;
using Emergence.Test.Mocks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Emergence.Test.API.Services
{
    public class InventoryServiceTests
    {
        private readonly Mock<IRepository<Inventory>> _mockInventoryRepository;
        private readonly Mock<IRepository<InventoryItem>> _mockInventoryItemRepository;
        private readonly Mock<IOriginService> _mockOriginService;

        public InventoryServiceTests()
        {
            _mockInventoryRepository = RepositoryMocks.GetStandardMockInventoryRepository();
            _mockInventoryItemRepository = RepositoryMocks.GetStandardMockInventoryItemRepository();
            _mockOriginService = ServiceMocks.GetStandardMockOriginService();
        }

        [Fact]
        public async Task TestGet()
        {
            var inventoryService = new InventoryService(_mockInventoryRepository.Object, _mockInventoryItemRepository.Object, _mockOriginService.Object);
            var inventory = await inventoryService.GetInventoryAsync(0);

            inventory.Should().NotBeNull("it exists");
            inventory.Items.Should().HaveCount(3);
            inventory.Items.Where(i => i.Status == ItemStatus.Available).Should().HaveCount(2);
            inventory.Items.Where(i => i.Status == ItemStatus.Wishlist).Should().HaveCount(1);
            inventory.Items.Where(i => i.ItemType == ItemType.Supply).Should().HaveCount(1);
            inventory.Items.Where(i => i.Visibility == Visibility.Contacts).Should().HaveCount(1);
        }
    }
}
