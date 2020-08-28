using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Stores;
using Emergence.Service;
using Emergence.Service.Interfaces;
using Emergence.Test.Mocks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using SixLabors.ImageSharp;
using Xunit;
using Models = Emergence.Data.Shared.Models;

namespace Emergence.Test.Emergence.API.Services
{
    public class PhotoServiceTests
    {
        private readonly Mock<IRepository<Photo>> _mockPhotoRepository;
        private readonly Mock<IBlobService> _mockBlobService;
        private readonly Mock<IConfigurationService> _mockConfigurationService;
        private const string BlobStorageRoot = "https://blobs.com/";

        public PhotoServiceTests()
        {
            _mockPhotoRepository = RepositoryMocks.GetStandardMockPhotoRepository();
            _mockBlobService = new Mock<IBlobService>();
            _mockConfigurationService = new Mock<IConfigurationService>();
            _mockConfigurationService.Setup(cs => cs.Settings).Returns(new AppConfiguration { BlobStorageRoot = BlobStorageRoot });
        }

        [Fact]
        public async Task TestUploadPhotoAsyncWithMetadta()
        {
            var metadata = new Dictionary<string, string>
            {
                { Constants.Latitude, "38.885986" },
                { Constants.Longitude, "-77.036880" },
                { Constants.Altitude, "145" },
                { Constants.Height, "3024" },
                { Constants.Width, "4032" },
                { Constants.DateTaken, "2020-07-22 13:45:26" },
            };
            var mockProperties = new Mock<IBlobResult>();
            mockProperties.Setup(p => p.Metadata).Returns(metadata);
            mockProperties.Setup(p => p.ContentType).Returns("");

            _mockBlobService.Setup(b => b.UploadPhotoAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(mockProperties.Object);

            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.jpg");

            var photoService = new PhotoService(_mockBlobService.Object, _mockPhotoRepository.Object, _mockConfigurationService.Object);

            var result = (await photoService.UploadOriginalsAsync(new List<FormFile> { file }, PhotoType.Activity, "me")).FirstOrDefault();

            result.Filename.Should().Be("original.jpg");
            result.Location.Latitude.Should().Be(38.885986);
            result.Location.Longitude.Should().Be(-77.036880);
            result.Location.Altitude.Should().Be(145);
            result.Height.Should().Be(3024);
            result.Width.Should().Be(4032);
            result.DateTaken.Should().Be(new DateTime(2020, 7, 22, 17, 45, 26, DateTimeKind.Utc));
            result.BlobPath.Length.Should().Be(36);
            result.OriginalUri.Should().Be(BlobStorageRoot + "photos/" + result.BlobPath + "/original.jpg");
            result.LargeUri.Should().Be(BlobStorageRoot + "photos/" + result.BlobPath + "/large.png");
            result.MediumUri.Should().Be(BlobStorageRoot + "photos/" + result.BlobPath + "/medium.png");
            result.ThumbnailUri.Should().Be(BlobStorageRoot + "photos/" + result.BlobPath + "/thumb.png");
        }

        [Fact]
        public async Task TestUploadPhotoAsyncWithSomeMetadata()
        {
            var metadata = new Dictionary<string, string>
            {
                { Constants.Height, "3024" },
                { Constants.Width, "4032" },
                { Constants.DateTaken, "2020-07-22 13:45:26" },
            };
            var mockProperties = new Mock<IBlobResult>();
            mockProperties.Setup(p => p.Metadata).Returns(metadata);
            mockProperties.Setup(p => p.ContentType).Returns("");

            _mockBlobService.Setup(b => b.UploadPhotoAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(mockProperties.Object);

            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");

            var photoService = new PhotoService(_mockBlobService.Object, _mockPhotoRepository.Object, _mockConfigurationService.Object);

            var result = (await photoService.UploadOriginalsAsync(new List<FormFile> { file }, PhotoType.Activity, "me")).FirstOrDefault();

            var timezone = TimeZoneInfo.Local;
            var expectedDate = TimeZoneInfo.ConvertTimeToUtc(new DateTime(2020, 7, 22, 13, 45, 26, DateTimeKind.Unspecified), timezone);

            result.Filename.Should().Be("original.txt");
            result.Location.Should().BeNull();
            result.Height.Should().Be(3024);
            result.Width.Should().Be(4032);
            result.DateTaken.Should().Be(expectedDate);
        }

        [Fact]
        public async Task TestGetPhotosAsync()
        {
            var photoService = new PhotoService(_mockBlobService.Object, _mockPhotoRepository.Object, _mockConfigurationService.Object);

            var photos = await photoService.GetPhotosAsync(new int[] { 1, 2, 3 });

            photos.FirstOrDefault().Type.Should().Be(PhotoType.Activity);
            photos.FirstOrDefault().Location.LocationId.Should().Be(1);
        }

        [Fact(Skip = "Integration test")]
        public async Task TestResizePhoto()
        {
            var photoService = new PhotoService(_mockBlobService.Object, _mockPhotoRepository.Object, _mockConfigurationService.Object);
            using (var stream = new MemoryStream())
            using (var image = Image.Load("../../../Data/original.jpg"))
            {
                var resized = await photoService.ProcessPhotoAsync(stream, image, Models.ImageSize.Large);

                resized.Height.Should().BeLessOrEqualTo((int)Models.ImageSize.Large);
                resized.Width.Should().BeLessOrEqualTo((int)Models.ImageSize.Large);

                using (var fileStream = File.OpenWrite("../../../Data/full.png"))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    await stream.CopyToAsync(fileStream);
                }
            }
        }
    }
}
