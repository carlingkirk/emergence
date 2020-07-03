using Emergence.API.Services.Interfaces;
using Moq;

namespace Emergence.Test.Mocks
{
    public static class ServiceMocks
    {
        public static Mock<ISpecimenService> GetStandardMockSpecimenService()
        {
            var mockSpecimenService = new Mock<ISpecimenService>();

            return mockSpecimenService;
        }

        public static Mock<IInventoryService> GetStandardMockInventoryService()
        {
            var mockInventoryService = new Mock<IInventoryService>();

            return mockInventoryService;
        }
    }
}
