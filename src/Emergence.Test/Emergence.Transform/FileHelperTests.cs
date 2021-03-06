using Emergence.Transform;
using Xunit;

namespace Emergence.Test.Transform
{
    public class FileHelperTests
    {
        [Fact]
        public void TestGetFileStream()
        {
            var fileStream = FileHelpers.GetDatafileName("USDA_USA_Checklist.csv", "..\\..\\..\\..\\..\\data");

            Assert.NotNull(fileStream);
        }
    }
}
